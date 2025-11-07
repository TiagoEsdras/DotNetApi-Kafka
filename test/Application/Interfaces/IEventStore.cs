using System.Collections.Generic;
using test.Domain;

namespace test.Application.Interfaces
{
    public interface IEventStore
    {
        IEnumerable<Event> GetRecent(int limit);
        void Add(Event evt);
    }
}