using System;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using System.Text.Json;
namespace ParcelTracking.Infrastructure.Messaging
{
    public class KafkaProducer
    {
        private readonly IProducer<string, string> _producer;

        public KafkaProducer()
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

            await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = key,
                Value = json
            });
        }
    }
}
