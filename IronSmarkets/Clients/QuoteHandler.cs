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
using System.Diagnostics;

using IronSmarkets.Data;
#if NET35
using IronSmarkets.System;
#endif

using Seto = IronSmarkets.Proto.Seto;

namespace IronSmarkets.Clients
{
    internal sealed class UidPair<T> : Tuple<Uid, T>
    {
        public Uid Uid { get { return Item1; } }
        public T Payload { get { return Item2; } }

        public UidPair(Uid uid, T payload) : base(uid, payload)
        {
        }
    }

    internal abstract class QuoteHandler<T>
    {
        private readonly IDictionary<Uid, EventHandler<QuotesReceivedEventArgs<T>>> _handlers =
            new Dictionary<Uid, EventHandler<QuotesReceivedEventArgs<T>>>();
        private readonly object _lock = new object();

        public void AddHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<T>> handler)
        {
            lock (_lock)
            {
                EventHandler<QuotesReceivedEventArgs<T>> md;
                if (!_handlers.TryGetValue(uid, out md))
                {
                    _handlers[uid] = handler;
                }
                else
                {
                    _handlers[uid] = md + handler;
                }
            }
        }

        public void RemoveHandler(Uid uid, EventHandler<QuotesReceivedEventArgs<T>> handler)
        {
            lock (_lock)
            {
                EventHandler<QuotesReceivedEventArgs<T>> md = _handlers[uid];
                Debug.Assert(handler != null, "handler != null");
                md -= handler;
                if (md == null)
                {
                    _handlers.Remove(uid);
                }
                else
                {
                    _handlers[uid] = md;
                }
            }
        }

        public void Handle(Seto.Payload payload)
        {
            Handle(Extract(payload), payload);
        }

        public void Handle(UidPair<T> pair, Seto.Payload payload)
        {
            EventHandler<QuotesReceivedEventArgs<T>> handler;
            lock (_lock)
            {
                _handlers.TryGetValue(pair.Uid, out handler);
            }
            if (handler != null)
                handler(this, PayloadArgs(pair, payload));
        }

        public static QuotesReceivedEventArgs<T> PayloadArgs(UidPair<T> pair, Seto.Payload payload)
        {
            return new QuotesReceivedEventArgs<T>(payload.EtoPayload.Seq, pair.Payload);
        }

        public abstract UidPair<T> Extract(Seto.Payload payload);
    }
}
