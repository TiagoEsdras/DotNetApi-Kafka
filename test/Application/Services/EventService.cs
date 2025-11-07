using test.Application.Dtos;
using test.Application.Helpers;
using test.Application.Interfaces;

namespace test.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IKafkaProducer _producer;
        private const string _topicName = "event-topic";

        public EventService(IKafkaProducer producer)
        {
            _producer = producer;
        }

        public async Task<Result> PublishEvent(EventDto eventDto)
        {
            var result = new Result();
            try
            {
                await _producer.ProduceAsync(_topicName, new(eventDto.UserId, eventDto.Type, eventDto.Data));
                result.IsSuccess = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
        }
    }
}