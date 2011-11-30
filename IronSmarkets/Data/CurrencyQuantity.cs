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
using System.Collections.Generic;
using System.Diagnostics;

namespace IronSmarkets.Data
{
    public struct CurrencyQuantity : IEquatable<CurrencyQuantity>
    {
        private readonly Quantity _quantity;
        private readonly Currency _currency;

        public Quantity Quantity { get { return _quantity; } }
        public Currency Currency { get { return _currency; } }

        public CurrencyQuantity(Quantity quantity, Currency currency)
        {
            if (quantity.Type != QuantityType.PayoffCurrency)
                throw new ArgumentException(
                    "Quantity must be of type PayoffCurrency");
            _quantity = quantity;
            _currency = currency;
        }

        /// <summary>
        ///   Calculates the buyer's liability for this quantity.
        /// </summary>
        public Money BuyLiability(Price price)
        {
            return new Money(price.Percent * _quantity.MoneyUnits, _currency);
        }

        public Money SellLiability(Price price)
        {
            var moneyUnits = new Money(_quantity.MoneyUnits, _currency);
            var buyLiability = BuyLiability(price);
            return moneyUnits  - buyLiability;
        }

        public override int GetHashCode()
        {
            return _quantity.GetHashCode() ^ _currency.GetHashCode();
        }

        public override string ToString()
        {
            return new Money(_quantity.MoneyUnits, _currency).ToString();
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((CurrencyQuantity)right);
        }

        public bool Equals(CurrencyQuantity other)
        {
            return _quantity == other._quantity
                && _currency == other._currency;
        }

        public static bool operator==(CurrencyQuantity left, CurrencyQuantity right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(CurrencyQuantity left, CurrencyQuantity right)
        {
            return !left.Equals(right);
        }
    }
}
