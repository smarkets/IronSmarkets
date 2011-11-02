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
using System.Linq;

namespace IronSmarkets.Data
{
    public class MarketQuotes
    {
        private static readonly IDictionary<Proto.Seto.PriceType, string> PriceTypeStrings =
            new Dictionary<Proto.Seto.PriceType, string> {
            { Proto.Seto.PriceType.PRICEPERCENTODDS, "percent-odds" }
        };
        private static readonly IDictionary<Proto.Seto.QuantityType, string> QuantityTypeStrings =
            new Dictionary<Proto.Seto.QuantityType, string> {
            { Proto.Seto.QuantityType.QUANTITYPAYOFFCURRENCY, "payoff-currency" }
        };

        private readonly Uid _uid;
        private readonly IContractQuotesMap _contractQuotes;
        private readonly string _priceType;
        private readonly string _quantityType;

        public Uid Uid { get { return _uid; } }
        public IContractQuotesMap ContractQuotes { get { return _contractQuotes; } }
        public string PriceType { get { return _priceType; } }
        public string QuantityType { get { return _quantityType; } }

        private MarketQuotes(
            Uid uid,
            IContractQuotesMap contractQuotes,
            string priceType,
            string quantityType)
        {
            _uid = uid;
            _contractQuotes = contractQuotes;
            _priceType = priceType;
            _quantityType = quantityType;
        }

        internal static MarketQuotes FromSeto(Proto.Seto.MarketQuotes setoQuotes)
        {
            return new MarketQuotes(
                Uid.FromUuid128(setoQuotes.Market),
                ContractQuotesMap.FromSeto(setoQuotes.ContractQuotes),
                PriceTypeStrings[setoQuotes.PriceType],
                QuantityTypeStrings[setoQuotes.QuantityType]);
        }
    }
}
