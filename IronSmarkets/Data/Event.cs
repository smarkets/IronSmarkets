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

using System;
using System.Collections.Generic;

using IronSmarkets.Clients;

namespace IronSmarkets.Data
{
    public class Event : IUpdatable<Event>
    {
        private EventInfo _info;
        private readonly List<Event> _children =
            new List<Event>();

        public EventInfo Info { get { return _info; } }
        public ICollection<Event> Children { get { return _children.AsReadOnly(); } }

        public event EventHandler<EventArgs> Updated;

        // Optional
        public Event Parent { get; set; }
        public bool HasParent { get { return Parent != null; } }

        private Event(EventInfo info)
        {
            _info = info;
        }

        public void AddChild(Event child)
        {
            _children.Add(child);
        }

        void IUpdatable<Event>.Update(Event source)
        {
            UpdateInfo(source.Info);
            OnUpdated();
        }

        internal void UpdateInfo(EventInfo info)
        {
            _info = info;
        }

        internal static Event FromSeto(ISmarketsClient client, Proto.Seto.EventInfo info)
        {
            var newEvent = new Event(EventInfo.FromSeto(info));
            client.MarketMap.MergeFromMarkets(client, info.Markets, newEvent);
            return newEvent;
        }

        private void OnUpdated()
        {
            var ev = Updated;
            if (ev != null)
                ev(this, new EventArgs());
        }
    }
}
