using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using test.Application.Interfaces;
using test.Domain;

namespace test.Infra.Producer
{
    public class KafkaProducer : IKafkaProducer, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            Converters = { new JsonStringEnumConverter() }
        };

        public KafkaProducer(ProducerConfig config)
        {
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task ProduceAsync(string topic, Event message)
        {
            string json = JsonSerializer.Serialize(message, _jsonOptions);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
        }

        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}