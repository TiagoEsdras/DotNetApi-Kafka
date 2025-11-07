using test.Domain;

namespace test.Application.Interfaces
{
    public interface IKafkaProducer
    {
        Task ProduceAsync(string topic, Event message);
    }
}
