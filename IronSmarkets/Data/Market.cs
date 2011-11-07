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

using IronSmarkets.Clients;
using IronSmarkets.Extensions;

namespace IronSmarkets.Data
{
    public class Market
    {
        private readonly MarketInfo _info;
        private readonly IContractMap _contracts;

        private MarketQuotes _quotes;
        private ulong _subscriptionSeq;
        private IQuoteSink _sink;

        public MarketInfo Info { get { return _info; } }
        public IContractMap Contracts { get { return _contracts; } }
        public MarketQuotes Quotes { get { return _quotes; } }

        public EventHandler<QuotesUpdatedEventArgs<MarketQuotes>> MarketQuotesUpdated;

        private Market(
            MarketInfo info,
            IContractMap contracts,
            MarketQuotes quotes = null)
        {
            _info = info;
            _contracts = contracts;
            _quotes = quotes;
        }

        public void SubscribeQuotes(IQuoteSink sink)
        {
            if (_sink != null)
                throw new InvalidOperationException("Already subscribed");
            _sink = sink;
            _sink.AddMarketQuotesHandler(_info.Uid, OnMarketQuotesReceived);
            _contracts.Values.ForAll(contract => contract.SubscribeQuotes(_sink));
            _subscriptionSeq = _sink.SubscribeMarket(_info.Uid);
        }

        public void UnsubscribeQuotes()
        {
            if (_sink == null)
                throw new InvalidOperationException("Not subscribed");
            _sink.UnsubscribeMarket(_info.Uid);
            _sink.RemoveMarketQuotesHandler(_info.Uid, OnMarketQuotesReceived);
            _contracts.Values.ForAll(contract => contract.UnsubscribeQuotes());
            _sink = null;
            _subscriptionSeq = 0;
        }

        private void OnMarketQuotesReceived(object sender, QuotesReceivedEventArgs<Proto.Seto.MarketQuotes> e)
        {
            _quotes = MarketQuotes.FromSeto(e.Payload);
            OnMarketQuotesUpdated(e.Sequence);
        }

        private void OnMarketQuotesUpdated(ulong seq)
        {
            var ev = MarketQuotesUpdated;
            if (ev != null)
                ev(this, new QuotesUpdatedEventArgs<MarketQuotes>(seq, _quotes));
        }

        internal static Market FromSeto(Proto.Seto.MarketInfo setoInfo)
        {
            return new Market(
                MarketInfo.FromSeto(setoInfo),
                ContractMap.FromContracts(setoInfo.Contracts));
        }
    }
}
