using Confluent.Kafka;
using ParcelTracking.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ParcelTracking.Infrastructure.Messaging
{
    public class KafkaEventPublisher : IEventPublisher
    {
        private readonly IProducer<string, string> _producer;

        public KafkaEventPublisher()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync(string topic, string key, object message)
        {
            var json = JsonSerializer.Serialize(message);

            await _producer.ProduceAsync(
                topic,
                new Message<string, string>
                {
                    Key = key,
                    Value = json
                });

            Console.WriteLine($"Kafka event published for key: {key}");
        }
    }
}
