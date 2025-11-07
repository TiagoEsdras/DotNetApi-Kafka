using System.Net;
using Microsoft.AspNetCore.Mvc;
using test.Application.Dtos;
using test.Application.Interfaces;
using test.Domain;

namespace test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventService _eventService;
        private readonly IEventStore _eventStore;

        public EventsController(ILogger<EventsController> logger, IEventService eventService, IEventStore eventStore)
        {
            _logger = logger;
            _eventService = eventService;
            _eventStore = eventStore;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto eventDto)
        {
            try
            {
                var result = await _eventService.PublishEvent(eventDto);
                if (!result.IsSuccess)
                {
                    _logger.LogError(result.Message);
                    return new ObjectResult(result.Message)
                    {
                        StatusCode = (int)HttpStatusCode.UnprocessableEntity
                    };
                }

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        [HttpGet]
        public IActionResult GetEvents([FromQuery] int limit = 50)
        {
            try
            {
                if (limit <= 0) limit = 1;
                if (limit > 1000) limit = 1000;
                IEnumerable<Event> events = _eventStore.GetRecent(limit);
                return Ok(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get events");
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}