// Copyright (c) 2012 Smarkets Limited
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

using System.Collections.Generic;

namespace IronSmarkets.Data
{
    internal class UpdatableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IReadOnlyMap<TKey, TValue>
        where TValue : IUpdatable<TValue>
    {
        public UpdatableDictionary(IDictionary<TKey, TValue> inner) : base(inner)
        {
        }

        public void MergeLeft(params IDictionary<TKey, TValue>[] others)
        {
            foreach (IDictionary<TKey, TValue> src in others)
            {
                foreach (KeyValuePair<TKey, TValue> p in src)
                {
                    if (ContainsKey(p.Key))
                        this[p.Key].Update(p.Value);
                    else
                        this[p.Key] = p.Value;
                }
            }
        }

        bool IReadOnlyMap<TKey, TValue>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>) this).Contains(item);
        }

        void IReadOnlyMap<TKey, TValue>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) this).CopyTo(array, arrayIndex);
        }

        ICollection<TKey> IReadOnlyMap<TKey, TValue>.Keys
        {
            get { return Keys; }
        }

        ICollection<TValue> IReadOnlyMap<TKey, TValue>.Values
        {
            get { return Values; }
        }
    }
}
