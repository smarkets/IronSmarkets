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

namespace IronSmarkets.Data
{
    public class BiDictionary<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        private readonly IDictionary<T1, T2> _direct = new Dictionary<T1, T2>();
        private readonly IDictionary<T2, T1> _inverse = new Dictionary<T2, T1>();

        public T2 this[T1 key] { get { return _direct[key]; } }
        public T1 this[T2 key] { get { return _inverse[key]; } }

        public void Add(T1 first, T2 second)
        {
            if (_direct.ContainsKey(first))
                throw new ArgumentException("first is a duplicate");
            if (_inverse.ContainsKey(second))
                throw new ArgumentException("second is a duplicate");
            _direct.Add(first, second);
            _inverse.Add(second, first);
        }

        public bool TryGetValue(T1 key, out T2 value)
        {
            return _direct.TryGetValue(key, out value);
        }

        public bool TryGetValue(T2 key, out T1 value)
        {
            return _inverse.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return _direct.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_direct as IEnumerable).GetEnumerator();
        }
    }
}
