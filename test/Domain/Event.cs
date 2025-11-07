using test.Application.Dtos.Enums;

namespace test.Domain
{
    public class Event
    {
        public Event(string userId, EventType type, object data)
        {
            UserId = userId;
            Type = type;
            Data = data;
        }

        public string UserId { get; set; }
        public EventType Type { get; set; }
        public object Data { get; set; }
    }
}