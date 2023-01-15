using System;
using System.Collections.Generic;

namespace Project.Utils
{
    public abstract class DefaultComparer<T> : IComparer<T>
    {
        public abstract int Compare(T x, T y);
    }
    public class DefaultComparableComparer<T> : DefaultComparer<T> where T : IComparable<T>
    {
        private static DefaultComparableComparer<T> _instance;
        public static DefaultComparableComparer<T> Instance
        {
            get
            {
                _instance ??= new();
                return _instance;
            }
        }

        public override int Compare(T x, T y)
        {
            return x.CompareTo(y);
        }
    }
}
