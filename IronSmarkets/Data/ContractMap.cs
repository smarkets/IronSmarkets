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
    public interface IContractMap : IDictionary<Uuid, Contract>
    {
    }

    internal class ContractMap : IContractMap
    {
        private readonly IDictionary<Uuid, Contract> _contracts;

        private ContractMap(IDictionary<Uuid, Contract> contracts)
        {
            _contracts = contracts;
        }

        public static IContractMap FromContracts(IEnumerable<ContractInfo> setoContracts)
        {
            return new ContractMap(
                setoContracts.Aggregate(
                    new Dictionary<Uuid, Contract>(),
                    (dict, contractInfo) => {
                        var contract = Contract.FromSeto(contractInfo);
                        dict[contract.Uuid] = contract;
                        return dict;
                    }));
        }

        public IEnumerator<KeyValuePair<Uuid, Contract>> GetEnumerator()
        {
            return _contracts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<Uuid, Contract> item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(KeyValuePair<Uuid, Contract> item)
        {
            return _contracts.Contains(item);
        }

        public void CopyTo(KeyValuePair<Uuid, Contract>[] array, int arrayIndex)
        {
            _contracts.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<Uuid, Contract> item)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return _contracts.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool ContainsKey(Uuid key)
        {
            return _contracts.ContainsKey(key);
        }

        public void Add(Uuid key, Contract value)
        {
            throw new NotSupportedException();
        }

        public bool Remove(Uuid key)
        {
            throw new NotSupportedException();
        }

        public bool TryGetValue(Uuid key, out Contract value)
        {
            return _contracts.TryGetValue(key, out value);
        }

        public Contract this[Uuid key]
        {
            get { return _contracts[key]; }
            set { throw new NotSupportedException(); }
        }

        public ICollection<Uuid> Keys
        {
            get { return _contracts.Keys; }
        }

        public ICollection<Contract> Values
        {
            get { return _contracts.Values; }
        }
    }
}
