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
using System.Linq;

namespace IronSmarkets.Data
{
    public interface IContractQuotesMap : IReadOnlyMap<Uid, ContractQuotes>
    {
    }

    internal class ContractQuotesMap : Dictionary<Uid, ContractQuotes>, IContractQuotesMap
    {
        private readonly PriceType _priceType;
        private readonly QuantityType _quantityType;

        private ContractQuotesMap(
            PriceType priceType,
            QuantityType quantityType,
            IDictionary<Uid, ContractQuotes> contractQuotes) : base(contractQuotes)
        {
            _priceType = priceType;
            _quantityType = quantityType;
        }

        ICollection<Uid> IReadOnlyMap<Uid, ContractQuotes>.Keys
        {
            get { return Keys; }
        }

        ICollection<ContractQuotes> IReadOnlyMap<Uid, ContractQuotes>.Values
        {
            get { return Values; }
        }

        internal static ContractQuotesMap FromSeto(
            IEnumerable<Proto.Seto.ContractQuotes> setoContractQuotes,
            PriceType priceType,
            QuantityType quantityType)
        {
            return new ContractQuotesMap(
                priceType,
                quantityType,
                setoContractQuotes.Aggregate(
                    new Dictionary<Uid, ContractQuotes>(),
                    (dict, contractQuotes) => {
                        var cq = ContractQuotes.FromSeto(
                            contractQuotes, priceType, quantityType);
                        dict[cq.Uid] = cq;
                        return dict;
                    }));
        }

        public void Add(Uid uid)
        {
            if (ContainsKey(uid))
            {
                throw new ArgumentException(
                    string.Format(
                        "Uid {0} already exists in map", uid));
            }
            this[uid] = ContractQuotes.Empty(uid, _priceType, _quantityType);
        }

        bool IReadOnlyMap<Uid, ContractQuotes>.Contains(KeyValuePair<Uid, ContractQuotes> item)
        {
            return ((ICollection<KeyValuePair<Uid, ContractQuotes>>) this).Contains(item);
        }

        void IReadOnlyMap<Uid, ContractQuotes>.CopyTo(KeyValuePair<Uid, ContractQuotes>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<Uid, ContractQuotes>>) this).CopyTo(array, arrayIndex);
        }
    }
}
