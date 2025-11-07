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
            Timestamp = DateTime.UtcNow;
        }

        public string UserId { get; private set; }
        public EventType Type { get; private set; }
        public object Data { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}