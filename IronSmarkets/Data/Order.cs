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

namespace IronSmarkets.Data
{
    public class Order
    {
        public Uid Uid { get { return State.Uid; } }
        public Price Price { get; private set; }
        public OrderState State { get; private set; }

        public bool Cancellable
        {
            get
            {
                return State.Status == OrderStatus.Pending
                    || State.Status == OrderStatus.Live
                    || State.Status == OrderStatus.PartiallyFilled;
            }
        }

        private Order(Price price, OrderState state)
        {
            Price = price;
            State = state;
        }

        internal static Order FromSeto(
            Proto.Seto.OrderState state,
            Proto.Seto.PriceType priceType,
            uint price)
        {
            return new Order(
                new Price(priceType, price),
                OrderState.FromSeto(state));
        }
    }
}
