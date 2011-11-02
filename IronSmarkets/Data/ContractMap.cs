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
    public interface IContractMap : IReadOnlyMap<Uid, ContractInfo>
    {
    }

    internal class ContractMap : IContractMap
    {
        private readonly IDictionary<Uid, ContractInfo> _contracts;

        private ContractMap(IDictionary<Uid, ContractInfo> contracts)
        {
            _contracts = contracts;
        }

        public static IContractMap FromContracts(
            IEnumerable<Proto.Seto.ContractInfo> setoContracts)
        {
            return new ContractMap(
                setoContracts.Aggregate(
                    new Dictionary<Uid, ContractInfo>(),
                    (dict, contractInfo) => {
                        var contract = ContractInfo.FromSeto(contractInfo);
                        dict[contract.Uid] = contract;
                        return dict;
                    }));
        }

        public IEnumerator<KeyValuePair<Uid, ContractInfo>> GetEnumerator()
        {
            return _contracts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(KeyValuePair<Uid, ContractInfo> item)
        {
            return _contracts.Contains(item);
        }

        public void CopyTo(KeyValuePair<Uid, ContractInfo>[] array, int arrayIndex)
        {
            _contracts.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _contracts.Count; }
        }

        public bool ContainsKey(Uid key)
        {
            return _contracts.ContainsKey(key);
        }

        public bool TryGetValue(Uid key, out ContractInfo value)
        {
            return _contracts.TryGetValue(key, out value);
        }

        public ContractInfo this[Uid key]
        {
            get { return _contracts[key]; }
        }

        public ICollection<Uid> Keys
        {
            get { return _contracts.Keys; }
        }

        public ICollection<ContractInfo> Values
        {
            get { return _contracts.Values; }
        }
    }
}
