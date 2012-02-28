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
    internal class OrdersForMarketRequestHandler : UidQueueRpcHandler<Seto.OrdersForMarket, IOrderMap>
    {
        private readonly OrderMap _orderMap;

        public OrdersForMarketRequestHandler(ISmarketsClient client, OrderMap orderMap) : base(client)
        {
            _orderMap = orderMap;
        }

        protected override IOrderMap Map(ISmarketsClient client, Seto.OrdersForMarket state)
        {
            return _orderMap.MergeFromSeto(client, state);
        }

        protected override void Extract(SyncRequest<Seto.OrdersForMarket> request, Seto.Payload payload)
        {
            request.Response = payload.OrdersForMarket;
        }
    }
}
