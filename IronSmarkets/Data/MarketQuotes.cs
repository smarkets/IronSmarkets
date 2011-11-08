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

namespace IronSmarkets.Data
{
    public class MarketQuotes
    {
        private readonly Uid _uid;
        private readonly IRwContractQuotesMap _contractQuotes;
        private readonly PriceType _priceType;
        private readonly QuantityType _quantityType;

        private IRwContractQuotesMap RwContractQuotes { get { return _contractQuotes; } }

        public Uid Uid { get { return _uid; } }
        public IContractQuotesMap ContractQuotes { get { return RwContractQuotes; } }
        public PriceType PriceType { get { return _priceType; } }
        public QuantityType QuantityType { get { return _quantityType; } }

        private MarketQuotes(
            Uid uid,
            IRwContractQuotesMap contractQuotes,
            PriceType priceType,
            QuantityType quantityType)
        {
            _uid = uid;
            _contractQuotes = contractQuotes;
            _priceType = priceType;
            _quantityType = quantityType;
        }

        internal static MarketQuotes FromSeto(Proto.Seto.MarketQuotes setoQuotes)
        {
            var quantityType = Quantity.QuantityTypeFromSeto(setoQuotes.QuantityType);
            var priceType = Price.PriceTypeFromSeto(setoQuotes.PriceType);
            return new MarketQuotes(
                Uid.FromUuid128(setoQuotes.Market),
                ContractQuotesMap.FromSeto(
                    setoQuotes.ContractQuotes, priceType, quantityType),
                priceType,
                quantityType);
        }
    }
}
