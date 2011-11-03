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

namespace IronSmarkets.Data
{
    public class MarketQuotesUpdatedEventArgs : EventArgs
    {
        public ulong Sequence { get; private set; }
        public MarketQuotes Quotes { get; private set; }

        internal MarketQuotesUpdatedEventArgs(ulong sequence, MarketQuotes quotes)
        {
            Sequence = sequence;
            Quotes = quotes;
        }
    }

    public class Market
    {
        private readonly MarketInfo _info;
        private readonly HashSet<Uid> _contracts;

        private MarketQuotes _quotes;
        private ulong _subscriptionSeq;
        private ISmarketsClient _client;

        public MarketInfo Info { get { return _info; } }
        public MarketQuotes Quotes { get { return _quotes; } }

        public EventHandler<MarketQuotesUpdatedEventArgs> MarketQuotesUpdated;

        private Market(MarketInfo info, MarketQuotes quotes = null)
        {
            _info = info;
            _contracts = new HashSet<Uid>(info.Contracts.Keys);
            _quotes = quotes;
        }

        public void SubscribeQuotes(ISmarketsClient client)
        {
            if (_client != null)
            {
                throw new InvalidOperationException("Already subscribed");
            }
            _client = client;
            _client.PayloadReceived += OnPayloadReceived;
            _subscriptionSeq = _client.SubscribeMarket(_info.Uid);
        }

        public void UnsubscribeQuotes()
        {
            _client.PayloadReceived -= OnPayloadReceived;
            _client = null;
            _subscriptionSeq = 0;
        }

        private void OnPayloadReceived(object sender, Events.PayloadReceivedEventArgs<Proto.Seto.Payload> e)
        {
            switch(e.Payload.Type)
            {
                case Proto.Seto.PayloadType.PAYLOADMARKETQUOTES:
                    var incoming = Uid.FromUuid128(e.Payload.MarketQuotes.Market);
                    if (incoming == _info.Uid)
                    {
                        _quotes = MarketQuotes.FromSeto(e.Payload.MarketQuotes);
                        OnMarketQuotesUpdated(e.Payload.EtoPayload.Seq, _quotes);
                    }
                    break;
                case Proto.Seto.PayloadType.PAYLOADCONTRACTQUOTES:
                    var incomingContract = Uid.FromUuid128(e.Payload.ContractQuotes.Contract);
                    if (_contracts.Contains(incomingContract))
                    {
                        _quotes.RWContractQuotes[incomingContract] = ContractQuotes.FromSeto(e.Payload.ContractQuotes);
                        OnMarketQuotesUpdated(e.Payload.EtoPayload.Seq, _quotes);
                    }
                    break;
            }
        }

        private void OnMarketQuotesUpdated(ulong sequence, MarketQuotes quotes)
        {
            // TODO: Offer a copy constructor on MarketQuotes so it
            // can be sent between threads.
            EventHandler<MarketQuotesUpdatedEventArgs> ev = MarketQuotesUpdated;
            if (ev != null)
                ev(this, new MarketQuotesUpdatedEventArgs(sequence, quotes));
        }

        internal static Market FromSeto(Proto.Seto.MarketInfo setoInfo)
        {
            return new Market(MarketInfo.FromSeto(setoInfo));
        }
    }
}
