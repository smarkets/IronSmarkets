// Copyright (c) 2011-2012 Smarkets Limited
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

using IronSmarkets.Clients;

namespace IronSmarkets.Data
{
    public class Market : IUpdatable<Market>
    {
        private readonly MarketInfo _info;
        private readonly Event _parent;

        private MarketQuotes _quotes;
        private IQuoteSink _sink;

        public Event Parent { get { return _parent; } }
        public MarketInfo Info { get { return _info; } }
        public MarketQuotes Quotes { get { return _quotes; } }

        public event EventHandler<QuotesUpdatedEventArgs<MarketQuotes>> MarketQuotesUpdated;
        public event EventHandler<EventArgs> Updated;

        private Market(MarketInfo info, Event parent)
        {
            _info = info;
            _parent = parent;
        }

        public void SubscribeQuotes(IQuoteSink sink)
        {
            if (_sink != null)
                throw new InvalidOperationException("Already subscribed");
            if (sink == null)
                throw new ArgumentNullException("sink");
            _sink = sink;
            _sink.SubscribeMarket(_info.Uid);
        }

        public void UnsubscribeQuotes()
        {
            if (_sink == null)
                throw new InvalidOperationException("Not subscribed");
            _sink.UnsubscribeMarket(_info.Uid);
            _sink = null;
        }

        void IUpdatable<Market>.Update(Market source)
        {
            OnUpdated();
        }

        internal void OnMarketQuotesReceived(object sender, QuotesReceivedEventArgs<Proto.Seto.MarketQuotes> e)
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

        internal static Market FromSeto(
            ISmarketsClient client,
            Proto.Seto.MarketInfo info,
            Event parent)
        {
            var market = new Market(MarketInfo.FromSeto(info), parent);
            client.ContractMap.MergeFromContracts(client, info.Contracts, market);
            return market;
        }

        private void OnUpdated()
        {
            var ev = Updated;
            if (ev != null)
                ev(this, new EventArgs());
        }
    }
}
