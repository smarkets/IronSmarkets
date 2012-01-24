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

using IronSmarkets.Clients;

namespace IronSmarkets.Data
{
    public class Contract
    {
        private readonly ContractInfo _info;

        private IQuoteSink _sink;
        private Market _parent;

        public ContractInfo Info { get { return _info; } }
        public ContractQuotes Quotes {
            get { return Parent.Quotes.ContractQuotes[Info.Uid]; }
        }
        public Market Parent { get { return _parent; } }

        public EventHandler<QuotesUpdatedEventArgs<ContractQuotes>> ContractQuotesUpdated;

        private Contract(ContractInfo info, Market parent)
        {
            _info = info;
            _parent = parent;
        }

        internal void SubscribeQuotes(IQuoteSink sink)
        {
            if (_sink != null)
                throw new InvalidOperationException("Already subscribed");
            _sink = sink;
            _sink.AddContractQuotesHandler(_info.Uid, OnContractQuotesReceived);
        }

        internal void UnsubscribeQuotes()
        {
            if (_sink == null)
                throw new InvalidOperationException("Not subscribed");
            _sink.RemoveContractQuotesHandler(_info.Uid, OnContractQuotesReceived);
            _sink = null;
        }

        private void OnContractQuotesReceived(object sender, QuotesReceivedEventArgs<Proto.Seto.ContractQuotes> e)
        {
            Quotes.UpdateFromSeto(e.Payload);
            OnContractQuotesUpdated(e.Sequence);
        }

        private void OnContractQuotesUpdated(ulong seq)
        {
            var ev = ContractQuotesUpdated;
            if (ev != null)
                ev(this, new QuotesUpdatedEventArgs<ContractQuotes>(seq, Quotes));
        }

        internal static Contract FromSeto(Proto.Seto.ContractInfo setoInfo, Market parent)
        {
            return new Contract(ContractInfo.FromSeto(setoInfo), parent);
        }
    }
}
