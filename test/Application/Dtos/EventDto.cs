using test.Application.Dtos.Enums;

namespace test.Application.Dtos
{
    public class EventDto
    {
        public string UserId { get; set; }
        public EventType Type { get; set; }
        public object Data { get; set; }
    }
}