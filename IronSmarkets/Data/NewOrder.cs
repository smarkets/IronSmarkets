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

namespace IronSmarkets.Data
{
    public struct NewOrder
    {
        public OrderCreateType Type { get; set; }
        public Uid Market { get; set; }
        public Uid Contract { get; set; }
        public Side Side { get; set; }
        public Quantity Quantity { get; set; }
        public Price Price { get; set; }

        internal Proto.Seto.OrderCreate ToOrderCreate()
        {
            return new Proto.Seto.OrderCreate {
                Type = OrderState.FromOrderCreateType(Type),
                    Market = Market.ToUuid128(),
                    Contract = Contract.ToUuid128(),
                    Side = Side.ToSeto(),
                    QuantityType = Quantity.SetoType,
                    Quantity = Quantity.Raw,
                    PriceType = Price.SetoType,
                    Price = Price.Raw
            };
        }

        public Order ToOrder(Uid orderUid)
        {
            return new Order(
                Price, new OrderState(
                    orderUid,
                    Type,
                    OrderStatus.Pending,
                    Quantity,
                    SetoMap.ToMicroseconds(DateTime.UtcNow),
                    new Quantity(Quantity.Type, 0)),
                Market, Contract, Side);
        }
    }
}
