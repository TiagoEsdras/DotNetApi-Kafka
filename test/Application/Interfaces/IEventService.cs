using test.Application.Dtos;
using test.Application.Helpers;

namespace test.Application.Interfaces
{
    public interface IEventService
    {
        public Task<Result> PublishEvent(EventDto eventDto);
    }
}