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

using System.Collections.Generic;

using IronSmarkets.Extensions;

namespace IronSmarkets.Data
{
    public interface IOrderMap : IReadOnlyMap<Uid, Order>
    {
        void Merge(IEnumerable<KeyValuePair<Uid, Order>> orders);
    }

    internal sealed class OrderMap : ReadOnlyDictionaryWrapper<Uid, Order>, IOrderMap
    {
        private OrderMap() : base(new Dictionary<Uid, Order>())
        {
        }

        private OrderMap(IDictionary<Uid, Order> orders) : base(orders)
        {
        }

        public void Merge(IEnumerable<KeyValuePair<Uid, Order>> orders)
        {
            orders.ForAll(Add);
        }

        internal static IOrderMap FromSeto(Proto.Seto.OrdersForAccount orders)
        {
            return new OrderMap { orders };
        }

        internal static IOrderMap FromSeto(Proto.Seto.OrdersForMarket orders)
        {
            return new OrderMap { orders };
        }

        private void Add(Proto.Seto.OrdersForAccount orders)
        {
            orders.Markets.ForAll(Add);
        }

        private void Add(Proto.Seto.OrdersForMarket orders)
        {
            orders.Contracts.ForAll(x => Add(x, orders.PriceType));
        }

        private void Add(Proto.Seto.OrdersForContract orders, Proto.Seto.PriceType priceType)
        {
            orders.Bids.ForAll(x => Add(x, priceType));
            orders.Offers.ForAll(x => Add(x, priceType));
        }

        private void Add(Proto.Seto.OrdersForPrice orders, Proto.Seto.PriceType priceType)
        {
            orders.Orders.ForAll(x => Add(Order.FromSeto(x, priceType, orders.Price)));
        }

        private void Add(Order order)
        {
            _inner.Add(order.Uid, order);
        }
    }
}
