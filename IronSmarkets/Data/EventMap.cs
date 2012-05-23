// Copyright (c) 2011-2012 Smarkets Limited
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

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using IronSmarkets.Clients;
using IronSmarkets.Extensions;

namespace IronSmarkets.Data
{
    public interface IEventMap : IReadOnlyMap<Uid, Event>
    {
        IEnumerable<Event> Roots { get; }
    }

    internal class EventMap : UpdatableDictionary<Uid, Event>, IEventMap
    {
        private IList<Event> _roots;

        public IEnumerable<Event> Roots
        {
            get
            {
                return _roots ?? (_roots = new List<Event>(
                                      Values.Where(ev => !ev.Info.ParentUid.HasValue)).AsReadOnly());
            }
        }

        public EventMap() : base(new Dictionary<Uid, Event>())
        {
        }

        private EventMap(IDictionary<Uid, Event> events) : base(events)
        {
        }

        public EventMap MergeFromSeto(ISmarketsClient client, Proto.Seto.Events setoEvents)
        {
            MergeLeft(FromSeto(client, setoEvents));
            _roots = null;
            return this;
        }

        public static EventMap FromSeto(ISmarketsClient client, Proto.Seto.Events setoEvents)
        {
            var eventDict = setoEvents.WithMarkets.Concat(setoEvents.Parents).Aggregate(
                new Dictionary<Uid, Event>(),
                (dict, eventInfo) => {
                    var ev = Event.FromSeto(client, eventInfo);
                    dict[ev.Info.Uid] = ev;
                    return dict;
                });
            eventDict.Values
                .Where(childEvent => childEvent.Info.ParentUid.HasValue)
                .ForAll(childEvent => {
                        Debug.Assert(
                            childEvent.Info.ParentUid != null,
                            "childEvent.Info.ParentUid != null");
                        var parent = eventDict[childEvent.Info.ParentUid.Value];
                        parent.AddChild(childEvent);
                    });
            return new EventMap(eventDict);
        }
    }
}
