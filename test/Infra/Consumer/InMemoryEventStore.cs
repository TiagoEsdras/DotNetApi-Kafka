using System.Collections.Concurrent;
using test.Application.Interfaces;
using test.Domain;

namespace test.Infra.Consumer
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentQueue<Event> _queue = new();
        private readonly int _capacity;

        public InMemoryEventStore(int capacity = 1000)
        {
            _capacity = capacity;
        }

        public void Add(Event evt)
        {
            _queue.Enqueue(evt);
            while (_queue.Count > _capacity && _queue.TryDequeue(out _)) { }
        }

        public IEnumerable<Event> GetRecent(int limit)
        {
            if (limit <= 0) yield break;
            var items = _queue.ToArray();
            int count = items.Length;
            int start = count > limit ? count - limit : 0;
            for (int i = start; i < count; i++)
            {
                yield return items[i];
            }
        }
    }
}