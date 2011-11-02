// Copyright (c) 2011 Smarkets Limited
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS
// BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IronSmarkets.Data
{
    public interface IEventMap : IReadOnlyMap<Uid, EventInfo>
    {
    }

    internal class EventMap : IEventMap
    {
        private readonly IDictionary<Uid, EventInfo> _events;

        private EventMap(IDictionary<Uid, EventInfo> events)
        {
            _events = events;
        }

        public static EventMap FromSeto(Proto.Seto.Events setoEvents)
        {
            return new EventMap(
                setoEvents.WithMarkets.Concat(setoEvents.Parents).Aggregate(
                    new Dictionary<Uid, EventInfo>(),
                    (dict, eventInfo) => {
                        var ev = EventInfo.FromSeto(eventInfo);
                        dict[ev.Uid] = ev;
                        return dict;
                    }));
        }

        public IEnumerator<KeyValuePair<Uid, EventInfo>> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(KeyValuePair<Uid, EventInfo> item)
        {
            return _events.Contains(item);
        }

        public void CopyTo(KeyValuePair<Uid, EventInfo>[] array, int arrayIndex)
        {
            _events.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _events.Count; }
        }

        public bool ContainsKey(Uid key)
        {
            return _events.ContainsKey(key);
        }

        public bool TryGetValue(Uid key, out EventInfo value)
        {
            return _events.TryGetValue(key, out value);
        }

        public EventInfo this[Uid key]
        {
            get { return _events[key]; }
        }

        public ICollection<Uid> Keys
        {
            get { return _events.Keys; }
        }

        public ICollection<EventInfo> Values
        {
            get { return _events.Values; }
        }
    }
}
