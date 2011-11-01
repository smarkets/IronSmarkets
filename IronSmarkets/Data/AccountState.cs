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
    public struct AccountState
    {
        private readonly Uuid _uuid;
        private readonly Currency _currency;
        private readonly decimal _cash;
        private readonly decimal _bonus;
        private readonly decimal _exposure;

        public Uuid Uuid { get { return _uuid; } }
        public Currency Currency { get { return _currency; } }
        public decimal Cash { get { return _cash; } }
        public decimal Bonus { get { return _bonus; } }
        public decimal Exposure { get { return _exposure; } }
        public decimal Available { get { return _cash - _exposure; } }

        private AccountState(
            Uuid uuid,
            Currency currency,
            decimal cash,
            decimal bonus,
            decimal exposure)
        {
            _uuid = uuid;
            _currency = currency;
            _cash = cash;
            _bonus = bonus;
            _exposure = exposure;
        }

        internal static AccountState FromSeto(Proto.Seto.AccountState info)
        {
            return new AccountState(
                Uuid.FromUuid128(info.Account),
                Currency.FromSeto(info.Currency),
                SetoMap.FromDecimal(info.Cash),
                SetoMap.FromDecimal(info.Bonus),
                SetoMap.FromDecimal(info.Exposure));
        }
    }
}
