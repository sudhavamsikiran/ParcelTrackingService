using Confluent.Kafka;
using ParcelTracking.Application.Commands;
using ParcelTracking.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ParcelTracking.Worker.Consumers
{
    public class ParcelRetryConsumer
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly ParcelScanConsumer _scanConsumer;

        public ParcelRetryConsumer(ParcelScanConsumer scanConsumer)
        {
            _scanConsumer = scanConsumer;

            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "parcel-retry-worker",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
        }

        public async Task StartAsync()
        {
            _consumer.Subscribe(KafkaTopics.ParcelScanRetry);

            Console.WriteLine("Retry worker started");

            while (true)
            {
                var result = _consumer.Consume();

                var scanEvent =
                    JsonSerializer.Deserialize<SubmitScanCommand>(
                        result.Message.Value);

                await _scanConsumer.ProcessEvent(scanEvent);
            }
        }
    }
}
