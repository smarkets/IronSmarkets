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

using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Data
{
    public struct Currency : IEquatable<Currency>
    {
        private static readonly IDictionary<Proto.Seto.Currency, Currency> Currencies =
            new Dictionary<Proto.Seto.Currency, Currency>
            {
                { Proto.Seto.Currency.CURRENCYGBP, new Currency("Pounds Sterling", "GBP") },
                { Proto.Seto.Currency.CURRENCYEUR, new Currency("Euros", "EUR") }
            };

        private readonly string _name;
        private readonly string _iso4217;

        public string Name { get { return _name; } }
        public string Iso4217 { get { return _iso4217; } }

        internal Currency(string name, string iso4217)
        {
            _name = name;
            _iso4217 = iso4217;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode() ^ _iso4217.GetHashCode();
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((Currency)right);
        }

        public bool Equals(Currency other)
        {
            return _name == other._name && _iso4217 == other._iso4217;
        }

        public override string ToString()
        {
            return _name;
        }

        public static Currency FromSeto(Proto.Seto.Currency currency)
        {
            Currency cur;
            if (!Currencies.TryGetValue(currency, out cur))
            {
                throw new ArgumentException("Invalid currency.");
            }
            return cur;
        }
    }
}
