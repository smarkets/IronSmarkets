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
            var fromSeto = QuoteFromSeto();
            setoQuotes.Bids.Select(fromSeto).ForAll(QuoteUpdateVisitor(_bids));
            setoQuotes.Offers.Select(fromSeto).ForAll(QuoteUpdateVisitor(_offers));
            var newExecutions = setoQuotes.Executions.Select(ExecutionFromSeto());
            if (!newExecutions.IsEmpty())
            {
                _executions.AddRange(newExecutions);
                _lastExecution = newExecutions.Last();
            }
        }

        private Func<Proto.Seto.Quote, Quote> QuoteFromSeto()
        {
            return QuoteFromSeto(_priceType, _quantityType);
        }

        private Func<Proto.Seto.Execution, Execution> ExecutionFromSeto()
        {
            return ExecutionFromSeto(_priceType, _quantityType);
        }

        private static Action<Quote> QuoteUpdateVisitor(IDictionary<Price, Quote> quotes)
        {
            return quote => {
                if (quote.Quantity.Raw == 0)
                    quotes.Remove(quote.Price);
                else
                    quotes[quote.Price] = quote;
            };
        }

        internal static ContractQuotes FromSeto(
            Proto.Seto.ContractQuotes setoQuotes,
            PriceType priceType,
            QuantityType quantityType)
        {
            return new ContractQuotes(
                Uid.FromUuid128(setoQuotes.Contract),
                setoQuotes.Bids.Select(QuoteFromSeto(priceType, quantityType)),
                setoQuotes.Offers.Select(QuoteFromSeto(priceType, quantityType)),
                setoQuotes.Executions.Select(ExecutionFromSeto(priceType, quantityType)),
                Execution.MaybeFromSeto(setoQuotes.LastExecution, priceType, quantityType),
                priceType,
                quantityType);
        }

        private static Func<Proto.Seto.Quote, Quote> QuoteFromSeto(
            PriceType priceType,
            QuantityType quantityType)
        {
            return x => Quote.FromSeto(x, priceType, quantityType);
        }

        private static Func<Proto.Seto.Execution, Execution> ExecutionFromSeto(
            PriceType priceType,
            QuantityType quantityType)
        {
            return x => Execution.FromSeto(x, priceType, quantityType);
        }
    }
}
