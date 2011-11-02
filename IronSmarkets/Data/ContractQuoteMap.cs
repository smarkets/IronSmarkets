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
    public interface IContractQuotesMap : IReadOnlyMap<Uuid, ContractQuotes>
    {
    }

    internal class ContractQuotesMap : IContractQuotesMap
    {
        private readonly IDictionary<Uuid, ContractQuotes> _contractQuotes;

        private ContractQuotesMap(IDictionary<Uuid, ContractQuotes> contractQuotes)
        {
            _contractQuotes = contractQuotes;
        }

        public static IContractQuotesMap FromSeto(
            IEnumerable<Proto.Seto.ContractQuotes> setoContractQuotes)
        {
            return new ContractQuotesMap(
                setoContractQuotes.Aggregate(
                    new Dictionary<Uuid, ContractQuotes>(),
                    (dict, contractQuotes) => {
                        var cq = ContractQuotes.FromSeto(contractQuotes);
                        dict[cq.Uuid] = cq;
                        return dict;
                    }));
        }

        public IEnumerator<KeyValuePair<Uuid, ContractQuotes>> GetEnumerator()
        {
            return _contractQuotes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(KeyValuePair<Uuid, ContractQuotes> item)
        {
            return _contractQuotes.Contains(item);
        }

        public void CopyTo(KeyValuePair<Uuid, ContractQuotes>[] array, int arrayIndex)
        {
            _contractQuotes.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _contractQuotes.Count; }
        }

        public bool ContainsKey(Uuid key)
        {
            return _contractQuotes.ContainsKey(key);
        }

        public bool TryGetValue(Uuid key, out ContractQuotes value)
        {
            return _contractQuotes.TryGetValue(key, out value);
        }

        public ContractQuotes this[Uuid key]
        {
            get { return _contractQuotes[key]; }
        }

        public ICollection<Uuid> Keys
        {
            get { return _contractQuotes.Keys; }
        }

        public ICollection<ContractQuotes> Values
        {
            get { return _contractQuotes.Values; }
        }
    }
}
