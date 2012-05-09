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

using System;

using log4net;

using IronSmarkets.Data;
using IronSmarkets.Exceptions;

namespace IronSmarkets.Clients
{
    internal sealed class OrderCreateRequestHandler
        : SeqRpcHandler<Proto.Seto.OrderAccepted, Order, Tuple<NewOrder, OrderMap>>
    {
        private class OrderCreateSyncRequest : SyncRequest<Proto.Seto.OrderAccepted, Order, Tuple<NewOrder, OrderMap>>
        {
            public OrderCreateSyncRequest(ulong sequence, Tuple<NewOrder, OrderMap> state) : base(sequence, state)
            {
            }

            protected override Order Map(ISmarketsClient client, Proto.Seto.OrderAccepted message)
            {
                var uid = Uid.FromUuid128(message.Order);
                var order = State.Item1.ToOrder(uid);
                State.Item2.Add(order);
                return order;
            }
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(OrderCreateRequestHandler));

        public OrderCreateRequestHandler(ISmarketsClient client) : base(client)
        {
        }

        protected override void Extract(
            SyncRequest<Proto.Seto.OrderAccepted, Order, Tuple<NewOrder, OrderMap>> request,
            Proto.Seto.Payload payload)
        {
            switch (payload.Type)
            {
                case Proto.Seto.PayloadType.PAYLOADORDERACCEPTED:
                    // Normal case
                    request.SetResponse(Client, payload.OrderAccepted);
                    break;
                case Proto.Seto.PayloadType.PAYLOADORDERREJECTED:
                    request.SetException(OrderRejectedException.FromSeto(payload.OrderRejected));
                    break;
                case Proto.Seto.PayloadType.PAYLOADORDERINVALID:
                    request.SetException(OrderInvalidException.FromSeto(payload.OrderInvalid));
                    break;
                default:
                    Log.Error(
                        string.Format(
                            "Somehow a payload of type {0}" +
                            " was dispatched to an order create" +
                            " handler.", payload.Type));
                    break;
            }
        }

        protected override ulong ExtractSeq(Proto.Seto.Payload payload)
        {
            switch (payload.Type)
            {
                case Proto.Seto.PayloadType.PAYLOADORDERACCEPTED:
                    // Normal case
                    return payload.OrderAccepted.Seq;
                case Proto.Seto.PayloadType.PAYLOADORDERREJECTED:
                    return payload.OrderRejected.Seq;
                case Proto.Seto.PayloadType.PAYLOADORDERINVALID:
                    return payload.OrderInvalid.Seq;
                default:
                    throw new InvalidOperationException(
                        string.Format(
                            "Somehow a payload of type {0}" +
                            " was dispatched to an order create" +
                            " handler.", payload.Type));
            }
        }

        protected override SyncRequest<Proto.Seto.OrderAccepted, Order, Tuple<NewOrder, OrderMap>> NewRequest(ulong sequence, Tuple<NewOrder, OrderMap> state)
        {
            return new OrderCreateSyncRequest(sequence, state);
        }
    }
}
