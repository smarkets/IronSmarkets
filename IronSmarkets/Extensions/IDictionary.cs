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

using IronSmarkets.Data;

namespace IronSmarkets.Extensions
{
    internal static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionaryWrapper<TKey, TValue>(dictionary);
        }

        public static void MergeLeft<T, TKey, TValue>(this T me, params IDictionary<TKey, TValue>[] others)
            where T : IDictionary<TKey, TValue>
        {
            foreach (IDictionary<TKey, TValue> src in others)
            {
                foreach (KeyValuePair<TKey, TValue> p in src)
                {
                    me[p.Key] = p.Value;
                }
            }
        }
    }

    internal class ReadOnlyDictionaryWrapper<TKey, TValue> :
        IDictionary<TKey, TValue>, IReadOnlyMap<TKey, TValue>
    {
        protected readonly IDictionary<TKey, TValue> Inner;

        public ReadOnlyDictionaryWrapper(IDictionary<TKey, TValue> inner)
        {
            Inner = inner;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Inner.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Inner.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Inner.Count; }
        }

        public bool ContainsKey(TKey key)
        {
            return Inner.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Inner.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return Inner[key]; }
            set
            {
                throw new NotSupportedException("Dictionary is read-only");
            }
        }

        public ICollection<TKey> Keys
        {
            get { return Inner.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return Inner.Values; }
        }

        public bool IsReadOnly { get { return true; } }

        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException("Dictionary is read-only");
        }

        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            throw new NotSupportedException("Dictionary is read-only");
        }

        public bool Remove(TKey key)
        {
            throw new NotSupportedException("Dictionary is read-only");
        }

        public bool Remove(KeyValuePair<TKey, TValue> pair)
        {
            throw new NotSupportedException("Dictionary is read-only");
        }

        public void Clear()
        {
            throw new NotSupportedException("Dictionary is read-only");
        }
    }
}
