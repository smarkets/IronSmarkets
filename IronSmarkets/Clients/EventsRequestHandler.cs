// Copyright (c) 2012 Smarkets Limited
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

using log4net;

using IronSmarkets.Data;
using IronSmarkets.Exceptions;

using Seto = IronSmarkets.Proto.Seto;

namespace IronSmarkets.Clients
{
    internal class EventsRequestHandler : SeqRpcHandler<Seto.Events, IEventMap, EventMap>
    {
        private class EventSyncRequest : SyncRequest<Seto.Events, IEventMap, EventMap>
        {
            public EventSyncRequest(ulong sequence, EventMap eventMap) : base(sequence, eventMap)
            {
            }

            protected override IEventMap Map(ISmarketsClient client, Seto.Events events)
            {
                return _state.MergeFromSeto(client, events);
            }
        }

        private readonly IAsyncHttpFoundHandler<Seto.Events> _httpHandler;

        public EventsRequestHandler(
            ISmarketsClient client,
            IAsyncHttpFoundHandler<Seto.Events> httpHandler)
            : base(client)
        {
            _httpHandler = httpHandler;
        }

        protected override SyncRequest<Seto.Events, IEventMap, EventMap> NewRequest(ulong sequence, EventMap eventMap)
        {
            return new EventSyncRequest(sequence, eventMap);
        }

        protected override void Extract(
            SyncRequest<Seto.Events, IEventMap, EventMap> request, Seto.Payload payload)
        {
            switch (payload.Type)
            {
                case Seto.PayloadType.PAYLOADINVALIDREQUEST:
                    request.SetException(InvalidRequestException.FromSeto(payload.InvalidRequest));
                    break;
                case Seto.PayloadType.PAYLOADHTTPFOUND:
                    _httpHandler.BeginFetchHttpFound(request, payload);
                    break;
            }
        }

        protected override ulong ExtractSeq(Seto.Payload payload)
        {
            switch (payload.Type)
            {
                case Seto.PayloadType.PAYLOADINVALIDREQUEST:
                    return payload.InvalidRequest.Seq;
                case Seto.PayloadType.PAYLOADHTTPFOUND:
                    return payload.HttpFound.Seq;
                default:
                    throw new InvalidOperationException(
                        string.Format(
                            "Somehow a payload of type {0}" +
                            " was dispatched to an events request" +
                            " handler.", payload.Type));
            }
        }
    }
}
