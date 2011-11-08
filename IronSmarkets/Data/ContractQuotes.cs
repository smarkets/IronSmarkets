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
using System.Linq;

using IronSmarkets.Extensions;

namespace IronSmarkets.Data
{
    public class ContractQuotes
    {
        private readonly Uid _uid;
        private readonly IDictionary<Price, Quote> _bids;
        private readonly IDictionary<Price, Quote> _offers;
        private readonly List<Execution> _executions;
        private readonly PriceType _priceType;
        private readonly QuantityType _quantityType;

        private Execution? _lastExecution;

        public Uid Uid { get { return _uid; } }
        public IEnumerable<Quote> Bids { get { return _bids.Values; } }
        public IEnumerable<Quote> Offers { get { return _offers.Values; } }
        public IEnumerable<Execution> Executions { get { return _executions; } }
        public Execution? LastExecution { get { return _lastExecution; } }
        public PriceType PriceType { get { return _priceType; } }
        public QuantityType QuantityType { get { return _quantityType; } }

        private ContractQuotes(
            Uid uid,
            IEnumerable<Quote> bids,
            IEnumerable<Quote> offers,
            IEnumerable<Execution> executions,
            Execution? lastExecution,
            PriceType priceType,
            QuantityType quantityType)
        {
            _uid = uid;
            _bids = bids.ToDictionary(bid => bid.Price, bid => bid);
            _offers = offers.ToDictionary(offer => offer.Price, offer => offer);
            _executions = new List<Execution>(executions);
            _lastExecution = lastExecution;
            _priceType = priceType;
            _quantityType = quantityType;
        }

        internal void UpdateFromSeto(Proto.Seto.ContractQuotes setoQuotes)
        {
            foreach (var bid in setoQuotes.Bids.Select(
                         x => Quote.FromSeto(x, _priceType, _quantityType)))
            {
                if (bid.Quantity.Raw == 0)
                {
                    _bids.Remove(bid.Price);
                }
                else
                {
                    _bids[bid.Price] = bid;
                }
            }
            foreach (var offer in setoQuotes.Offers.Select(
                         x => Quote.FromSeto(x, _priceType, _quantityType)))
            {
                if (offer.Quantity.Raw == 0)
                {
                    _offers.Remove(offer.Price);
                }
                else
                {
                    _offers[offer.Price] = offer;
                }
            }
            var newExecutions = setoQuotes.Executions.Select(
                x => Execution.FromSeto(x, _priceType, _quantityType));
            if (!newExecutions.IsEmpty())
            {
                _executions.AddRange(newExecutions);
                _lastExecution = newExecutions.Last();
            }
        }

        internal static ContractQuotes FromSeto(
            Proto.Seto.ContractQuotes setoQuotes,
            PriceType priceType,
            QuantityType quantityType)
        {
            return new ContractQuotes(
                Uid.FromUuid128(setoQuotes.Contract),
                setoQuotes.Bids.Select(x => Quote.FromSeto(x, priceType, quantityType)),
                setoQuotes.Offers.Select(x => Quote.FromSeto(x, priceType, quantityType)),
                setoQuotes.Executions.Select(x => Execution.FromSeto(x, priceType, quantityType)),
                Execution.MaybeFromSeto(setoQuotes.LastExecution, priceType, quantityType),
                priceType,
                quantityType);
        }
    }
}
