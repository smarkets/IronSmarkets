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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using IronSmarkets.Proto.Seto;

namespace IronSmarkets.Data
{
    internal class MarketMap : IDictionary<Uuid, Market>
    {
        private readonly IDictionary<Uuid, Market> _markets;

        private MarketMap(IDictionary<Uuid, Market> markets)
        {
            _markets = markets;
        }

        public static MarketMap FromMarkets(IEnumerable<MarketInfo> setoMarkets)
        {
            return new MarketMap(
                setoMarkets.Aggregate(
                    new Dictionary<Uuid, Market>(),
                    (dict, marketInfo) => {
                        var market = Market.FromSeto(marketInfo);
                        dict[market.Uuid] = market;
                        return dict;
                    }));
        }

        public IEnumerator<KeyValuePair<Uuid, Market>> GetEnumerator()
        {
            return _markets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<Uuid, Market> item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(KeyValuePair<Uuid, Market> item)
        {
            return _markets.Contains(item);
        }

        public void CopyTo(KeyValuePair<Uuid, Market>[] array, int arrayIndex)
        {
            _markets.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<Uuid, Market> item)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return _markets.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool ContainsKey(Uuid key)
        {
            return _markets.ContainsKey(key);
        }

        public void Add(Uuid key, Market value)
        {
            throw new NotSupportedException();
        }

        public bool Remove(Uuid key)
        {
            throw new NotSupportedException();
        }

        public bool TryGetValue(Uuid key, out Market value)
        {
            return _markets.TryGetValue(key, out value);
        }

        public Market this[Uuid key]
        {
            get { return _markets[key]; }
            set { throw new NotSupportedException(); }
        }

        public ICollection<Uuid> Keys
        {
            get { return _markets.Keys; }
        }

        public ICollection<Market> Values
        {
            get { return _markets.Values; }
        }
    }
}
