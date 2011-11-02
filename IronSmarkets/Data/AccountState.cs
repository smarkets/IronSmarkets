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
    public struct AccountState : IEquatable<AccountState>
    {
        private readonly Uid _uid;
        private readonly Currency _currency;
        private readonly decimal _cash;
        private readonly decimal _bonus;
        private readonly decimal _exposure;

        public Uid Uid { get { return _uid; } }
        public Currency Currency { get { return _currency; } }
        public decimal Cash { get { return _cash; } }
        public decimal Bonus { get { return _bonus; } }
        public decimal Exposure { get { return _exposure; } }
        public decimal Available { get { return _cash - _exposure; } }

        private AccountState(
            Uid uid,
            Currency currency,
            decimal cash,
            decimal bonus,
            decimal exposure)
        {
            _uid = uid;
            _currency = currency;
            _cash = cash;
            _bonus = bonus;
            _exposure = exposure;
        }

        public override int GetHashCode()
        {
            return _uid.GetHashCode()
                ^ _currency.GetHashCode()
                ^ _cash.GetHashCode()
                ^ _bonus.GetHashCode()
                ^ _exposure.GetHashCode();
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
                return false;

            if (GetType() != right.GetType())
                return false;

            return Equals((AccountState)right);
        }

        public bool Equals(AccountState other)
        {
            return _uid == other._uid
                && _currency == other._currency
                && _cash == other._cash
                && _bonus == other._bonus
                && _exposure == other._exposure;
        }

        public override string ToString()
        {
            return string.Format(
                "AccountState(uuid: {0}, "
                + "currency: {1}, "
                + "cash: {2}, "
                + "bonus: {3}, "
                + "exposure: {4})",
                _uid, _currency, _cash, _bonus, _exposure);
        }

        public static bool operator==(AccountState left, AccountState right)
        {
            return left.Equals(right);
        }

        public static bool operator!=(AccountState left, AccountState right)
        {
            return !left.Equals(right);
        }

        internal static AccountState FromSeto(Proto.Seto.AccountState info)
        {
            return new AccountState(
                Uid.FromUuid128(info.Account),
                Currency.FromSeto(info.Currency),
                SetoMap.FromDecimal(info.Cash),
                SetoMap.FromDecimal(info.Bonus),
                SetoMap.FromDecimal(info.Exposure));
        }
    }
}
