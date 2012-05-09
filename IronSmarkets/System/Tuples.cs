//
// Tuples.cs
//
// Authors:
//  Zoltan Varga (vargaz@gmail.com)
//  Marek Safar (marek.safar@gmail.com)
//
// Copyright (C) 2009 Novell
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;

using IronSmarkets.System.Collections;

namespace IronSmarkets.System
{
    [Serializable]
    public class Tuple<T1> : IStructuralEquatable, IStructuralComparable, IComparable
    {
        T1 item1;

        public Tuple (T1 item1)
        {
            this.item1 = item1;
        }

        public T1 Item1 {
            get { return item1; }
        }

        int IComparable.CompareTo (object obj)
        {
            return ((IStructuralComparable) this).CompareTo (obj, Comparer<object>.Default);
        }

        int IStructuralComparable.CompareTo (object other, IComparer comparer)
        {
            var t = other as Tuple<T1>;
            if (t == null) {
                if (other == null) return 1;
                throw new ArgumentException ();
            }

            return comparer.Compare (item1, t.item1);
        }

        public override bool Equals (object obj)
        {
            return ((IStructuralEquatable) this).Equals (obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals (object other, IEqualityComparer comparer)
        {
            var t = other as Tuple<T1>;
            if (t == null) {
                if (other == null) return false;
                throw new ArgumentException ();
            }

            return comparer.Equals (item1, t.item1);
        }

        public override int GetHashCode ()
        {
            return ((IStructuralEquatable) this).GetHashCode (EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode (IEqualityComparer comparer)
        {
            return comparer.GetHashCode (item1);
        }

        public override string ToString ()
        {
            return String.Format ("({0})", item1);
        }
    }

    [Serializable]
    public class Tuple<T1, T2> : IStructuralEquatable, IStructuralComparable, IComparable
    {
        T1 item1;
        T2 item2;

        public Tuple (T1 item1, T2 item2)
        {
            this.item1 = item1;
            this.item2 = item2;
        }

        public T1 Item1 {
            get { return item1; }
        }

        public T2 Item2 {
            get { return item2; }
        }

        int IComparable.CompareTo (object obj)
        {
            return ((IStructuralComparable) this).CompareTo (obj, Comparer<object>.Default);
        }

        int IStructuralComparable.CompareTo (object other, IComparer comparer)
        {
            var t = other as Tuple<T1, T2>;
            if (t == null) {
                if (other == null) return 1;
                throw new ArgumentException ();
            }

            int res = comparer.Compare (item1, t.item1);
            if (res != 0) return res;
            return comparer.Compare (item2, t.item2);
        }

        public override bool Equals (object obj)
        {
            return ((IStructuralEquatable) this).Equals (obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals (object other, IEqualityComparer comparer)
        {
            var t = other as Tuple<T1, T2>;
            if (t == null) {
                if (other == null) return false;
                throw new ArgumentException ();
            }

            return comparer.Equals (item1, t.item1) &&
                comparer.Equals (item2, t.item2);
        }

        public override int GetHashCode ()
        {
            return ((IStructuralEquatable) this).GetHashCode (EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode (IEqualityComparer comparer)
        {
            int h = comparer.GetHashCode (item1);
            h = (h << 5) - h + comparer.GetHashCode (item2);
            return h;
        }

        public override string ToString ()
        {
            return String.Format ("({0}, {1})", item1, item2);
        }
    }

    [Serializable]
    public class Tuple<T1, T2, T3> : IStructuralEquatable, IStructuralComparable, IComparable
    {
        T1 item1;
        T2 item2;
        T3 item3;

        public Tuple (T1 item1, T2 item2, T3 item3)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
        }

        public T1 Item1 {
            get { return item1; }
        }

        public T2 Item2 {
            get { return item2; }
        }

        public T3 Item3 {
            get { return item3; }
        }

        int IComparable.CompareTo (object obj)
        {
            return ((IStructuralComparable) this).CompareTo (obj, Comparer<object>.Default);
        }

        int IStructuralComparable.CompareTo (object other, IComparer comparer)
        {
            var t = other as Tuple<T1, T2, T3>;
            if (t == null) {
                if (other == null) return 1;
                throw new ArgumentException ();
            }

            int res = comparer.Compare (item1, t.item1);
            if (res != 0) return res;
            res = comparer.Compare (item2, t.item2);
            if (res != 0) return res;
            return comparer.Compare (item3, t.item3);
        }

        public override bool Equals (object obj)
        {
            return ((IStructuralEquatable) this).Equals (obj, EqualityComparer<object>.Default);
        }

        bool IStructuralEquatable.Equals (object other, IEqualityComparer comparer)
        {
            var t = other as Tuple<T1, T2, T3>;
            if (t == null) {
                if (other == null) return false;
                throw new ArgumentException ();
            }

            return comparer.Equals (item1, t.item1) &&
                comparer.Equals (item2, t.item2) &&
                comparer.Equals (item3, t.item3);
        }

        public override int GetHashCode ()
        {
            return ((IStructuralEquatable) this).GetHashCode (EqualityComparer<object>.Default);
        }

        int IStructuralEquatable.GetHashCode (IEqualityComparer comparer)
        {
            int h = comparer.GetHashCode (item1);
            h = (h << 5) - h + comparer.GetHashCode (item2);
            h = (h << 5) - h + comparer.GetHashCode (item3);
            return h;
        }

        public override string ToString ()
        {
            return String.Format ("({0}, {1}, {2})", item1, item2, item3);
        }
    }
}

