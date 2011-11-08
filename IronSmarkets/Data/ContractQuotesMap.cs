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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IronSmarkets.Data
{
    internal interface IRwContractQuotesMap : IContractQuotesMap
    {
        ContractQuotes this[Uid key] { set; }
    }

    public interface IContractQuotesMap : IReadOnlyMap<Uid, ContractQuotes>
    {
    }

    internal class ContractQuotesMap : IRwContractQuotesMap
    {
        private readonly IDictionary<Uid, ContractQuotes> _contractQuotes;

        private ContractQuotesMap(IDictionary<Uid, ContractQuotes> contractQuotes)
        {
            _contractQuotes = contractQuotes;
        }

        internal static IRwContractQuotesMap FromSeto(
            IEnumerable<Proto.Seto.ContractQuotes> setoContractQuotes,
            QuantityType quantityType)
        {
            return new ContractQuotesMap(
                setoContractQuotes.Aggregate(
                    new Dictionary<Uid, ContractQuotes>(),
                    (dict, contractQuotes) => {
                        var cq = ContractQuotes.FromSeto(contractQuotes, quantityType);
                        dict[cq.Uid] = cq;
                        return dict;
                    }));
        }

        public IEnumerator<KeyValuePair<Uid, ContractQuotes>> GetEnumerator()
        {
            return _contractQuotes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(KeyValuePair<Uid, ContractQuotes> item)
        {
            return _contractQuotes.Contains(item);
        }

        public void CopyTo(KeyValuePair<Uid, ContractQuotes>[] array, int arrayIndex)
        {
            _contractQuotes.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _contractQuotes.Count; }
        }

        public bool ContainsKey(Uid key)
        {
            return _contractQuotes.ContainsKey(key);
        }

        public bool TryGetValue(Uid key, out ContractQuotes value)
        {
            return _contractQuotes.TryGetValue(key, out value);
        }

        public ContractQuotes this[Uid key]
        {
            get { return _contractQuotes[key]; }
            set { _contractQuotes[key] = value; }
        }

        public ICollection<Uid> Keys
        {
            get { return _contractQuotes.Keys; }
        }

        public ICollection<ContractQuotes> Values
        {
            get { return _contractQuotes.Values; }
        }
    }
}
