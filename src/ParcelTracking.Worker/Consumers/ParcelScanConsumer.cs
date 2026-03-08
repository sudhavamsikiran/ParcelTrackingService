using Confluent.Kafka;
using ParcelTracking.Application.Commands;
using ParcelTracking.Application.Interfaces;
using ParcelTracking.Application.ReadModels;
using ParcelTracking.Domain.Entities;
using ParcelTracking.Domain.Enums;
using ParcelTracking.Domain.Interfaces;
using ParcelTracking.Infrastructure.Messaging;
using ParcelTracking.Worker.Metrics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;


namespace ParcelTracking.Worker.Consumers
{
    public class ParcelScanConsumer
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly IParcelRepository _parcelRepository;
        private readonly IParcelEventRepository _eventRepository;
        private readonly IProcessedEventRepository _processedEventRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ITrackingQueryRepository _trackingQueryRepository;
        public ParcelScanConsumer(IParcelRepository repository, IParcelEventRepository eventRepository, IProcessedEventRepository processedEventRepository,  IEventPublisher eventPublisher, ITrackingQueryRepository trackingQueryRepository)
        {
            _parcelRepository = repository;
            _eventRepository = eventRepository;
            _processedEventRepository = processedEventRepository;
            _processedEventRepository = processedEventRepository;
            _eventPublisher = eventPublisher;
            _trackingQueryRepository = trackingQueryRepository;
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "parcel-worker-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            
        }

        public async Task StartAsync()
        {
            _consumer.Subscribe(KafkaTopics.ParcelScanEvents);

            Console.WriteLine("ParcelScanConsumer started...");

            while (true)
            {
                try
                {
                    var result = _consumer.Consume();

                    if (result == null)
                        continue;

                    Console.WriteLine($"Event received: {result.Message.Value}");

                    var scanEvent = JsonSerializer.Deserialize<SubmitScanCommand>(
                        result.Message.Value,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    if (scanEvent == null)
                        continue;

                    await ProcessEvent(scanEvent);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Consumer error: {ex.Message}");
                }
            }
        }

        public async Task ProcessEvent(SubmitScanCommand scanEvent)
        {
            try
            {
                // -----------------------------
                // IDEMPOTENCY CHECK
                // -----------------------------

                var alreadyProcessed =
                    await _processedEventRepository.ExistsAsync(scanEvent.EventId);

                if (alreadyProcessed)
                {
                    Console.WriteLine($"Duplicate event ignored: {scanEvent.EventId}");
                    return;
                }

                // -----------------------------
                // STORE EVENT HISTORY
                // -----------------------------

                var parcelEvent = new ParcelEvent
                {
                   Id = Guid.NewGuid().ToString(),
                    EventId = scanEvent.EventId,
                    TrackingId = scanEvent.TrackingId,
                    Status = scanEvent.EventType,
                    LocationId = scanEvent.LocationId,
                    ActorId = scanEvent.ActorId,
                    EventTimeUtc = scanEvent.EventTimeUtc
                };

                await _eventRepository.AddEventAsync(parcelEvent);

                // -----------------------------
                // UPDATE PARCEL STATE
                // -----------------------------

                var parcel =
                    await _parcelRepository.GetAsync(scanEvent.TrackingId);

                if (parcel == null)
                {
                    parcel = new Parcel(
                        scanEvent.Id,
                        scanEvent.TrackingId,
                        null,
                        null,
                        null
                   );

                    await _parcelRepository.CreateAsync(parcel);
                }

                var newStatus =
                    Enum.Parse<ParcelStatus>(scanEvent.EventType);

                parcel.UpdateStatus(newStatus);

                await _parcelRepository.UpdateAsync(parcel);

                // -----------------------------
                // UPDATE CQRS READ MODEL
                // -----------------------------

                var view =
                    await _trackingQueryRepository.GetAsync(scanEvent.TrackingId);

                if (view == null)
                {
                    view = new ParcelTrackingView
                    {
                        Id = scanEvent.TrackingId,
                        TrackingId = scanEvent.TrackingId,
                        CurrentStatus = scanEvent.EventType,
                        LastLocation = scanEvent.LocationId,
                        LastUpdated = scanEvent.EventTimeUtc
                    };
                }

                view.CurrentStatus = scanEvent.EventType;
                view.LastLocation = scanEvent.LocationId;
                view.LastUpdated = scanEvent.EventTimeUtc;

                view.Events.Add(new TrackingEventView
                {
                    Status = scanEvent.EventType,
                    Location = scanEvent.LocationId,
                    EventTime = scanEvent.EventTimeUtc
                });

                await _trackingQueryRepository.UpsertAsync(view);

                // -----------------------------
                // SEND NOTIFICATION EVENT
                // -----------------------------

                await _eventPublisher.PublishAsync(
                    KafkaTopics.ParcelNotifications,
                    parcel.TrackingId,
                    new ParcelNotificationEvent
                    {
                        TrackingId = parcel.TrackingId,
                        Status = parcel.CurrentStatus.ToString(),
                        LocationId = scanEvent.LocationId,
                        EventTimeUtc = scanEvent.EventTimeUtc
                    });

                // -----------------------------
                // MARK EVENT PROCESSED
                // -----------------------------

                await _processedEventRepository.MarkProcessedAsync(
                    scanEvent.EventId);
                // -----------------------------
                // METRICS
                // -----------------------------

                ParcelMetrics.EventsProcessed.Add(1);

                Console.WriteLine(
                    $"Parcel updated: {parcel.TrackingId} → {parcel.CurrentStatus}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Processing failed: {ex.Message}");

                scanEvent.RetryCount++;

                if (scanEvent.RetryCount <= 3)
                {
                    await _eventPublisher.PublishAsync(
                        KafkaTopics.ParcelScanRetry,
                        scanEvent.TrackingId,
                        scanEvent);
                    ParcelMetrics.EventsProcessed.Add(1);
                    Console.WriteLine(
                        $"Event sent to retry topic. RetryCount={scanEvent.RetryCount}");
                }
                else
                {
                    await _eventPublisher.PublishAsync(
                        KafkaTopics.ParcelScanDLQ,
                        scanEvent.TrackingId,
                        scanEvent);
                    ParcelMetrics.RetryEvents.Add(1);
                    Console.WriteLine("Event moved to DLQ");
                }
            }
        }
    }
}
