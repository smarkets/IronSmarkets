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

using IronSmarkets.Clients;
using IronSmarkets.Extensions;

namespace IronSmarkets.Data
{
    public interface IOrderMap : IReadOnlyMap<Uid, Order>
    {
        void Merge(IEnumerable<KeyValuePair<Uid, Order>> orders);
    }

    internal sealed class OrderMap : Dictionary<Uid, Order>, IOrderMap
    {
        public OrderMap() : base(new Dictionary<Uid, Order>())
        {
        }

        ICollection<Uid> IReadOnlyMap<Uid, Order>.Keys
        {
            get { return Keys; }
        }

        ICollection<Order> IReadOnlyMap<Uid, Order>.Values
        {
            get { return Values; }
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

        public OrderMap MergeFromSeto(ISmarketsClient client, Proto.Seto.OrdersForAccount orders)
        {
            Add(orders);
            return this;
        }

        public OrderMap MergeFromSeto(ISmarketsClient client, Proto.Seto.OrdersForMarket orders)
        {
            Add(orders);
            return this;
        }

        private void Add(Proto.Seto.OrdersForAccount orders)
        {
            orders.Markets.ForAll(Add);
        }

        private void Add(Proto.Seto.OrdersForMarket orders)
        {
            orders.Contracts.ForAll(x => Add(x, orders.Market, orders.PriceType));
        }

        private void Add(Proto.Seto.OrdersForContract orders, Proto.Seto.Uuid128 market, Proto.Seto.PriceType priceType)
        {
            orders.Bids.ForAll(x => Add(x, market, orders.Contract, Proto.Seto.Side.SIDEBUY, priceType));
            orders.Offers.ForAll(x => Add(x, market, orders.Contract, Proto.Seto.Side.SIDESELL, priceType));
        }

        private void Add(
            Proto.Seto.OrdersForPrice orders,
            Proto.Seto.Uuid128 market,
            Proto.Seto.Uuid128 contract,
            Proto.Seto.Side side,
            Proto.Seto.PriceType priceType)
        {
            orders.Orders.ForAll(x => Add(Order.FromSeto(x, priceType, orders.Price, market, contract, side)));
        }

        private void Add(KeyValuePair<Uid, Order> obj)
        {
            Add(obj.Value);
        }

        internal void Add(Order order)
        {
            if (ContainsKey(order.Uid))
            {
                var dest = this[order.Uid] as IUpdatable<Order>;
                if (dest != null)
                    dest.Update(order);
            }
            else
            {
                this[order.Uid] = order;
            }
        }

        bool IReadOnlyMap<Uid, Order>.Contains(KeyValuePair<Uid, Order> item)
        {
            return ((ICollection<KeyValuePair<Uid, Order>>) this).Contains(item);
        }

        void IReadOnlyMap<Uid, Order>.CopyTo(KeyValuePair<Uid, Order>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<Uid, Order>>) this).CopyTo(array, arrayIndex);
        }
    }
}
