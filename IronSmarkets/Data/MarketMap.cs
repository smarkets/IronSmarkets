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

namespace IronSmarkets.Data
{
    public interface IMarketMap : IReadOnlyMap<Uuid, MarketInfo>
    {
    }

    internal class MarketMap : IMarketMap
    {
        private readonly IDictionary<Uuid, MarketInfo> _markets;

        private MarketMap(IDictionary<Uuid, MarketInfo> markets)
        {
            _markets = markets;
        }

        public static IMarketMap FromMarkets(IEnumerable<Proto.Seto.MarketInfo> setoMarkets)
        {
            return new MarketMap(
                setoMarkets.Aggregate(
                    new Dictionary<Uuid, MarketInfo>(),
                    (dict, marketInfo) => {
                        var market = MarketInfo.FromSeto(marketInfo);
                        dict[market.Uuid] = market;
                        return dict;
                    }));
        }

        public IEnumerator<KeyValuePair<Uuid, MarketInfo>> GetEnumerator()
        {
            return _markets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(KeyValuePair<Uuid, MarketInfo> item)
        {
            return _markets.Contains(item);
        }

        public void CopyTo(KeyValuePair<Uuid, MarketInfo>[] array, int arrayIndex)
        {
            _markets.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _markets.Count; }
        }

        public bool ContainsKey(Uuid key)
        {
            return _markets.ContainsKey(key);
        }

        public bool TryGetValue(Uuid key, out MarketInfo value)
        {
            return _markets.TryGetValue(key, out value);
        }

        public MarketInfo this[Uuid key]
        {
            get { return _markets[key]; }
        }

        public ICollection<Uuid> Keys
        {
            get { return _markets.Keys; }
        }

        public ICollection<MarketInfo> Values
        {
            get { return _markets.Values; }
        }
    }
}
