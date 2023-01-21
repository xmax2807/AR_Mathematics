using System;
using System.Collections.Generic;

namespace Project.Utils
{
    public class DefaultComparableComparer<T> : IComparer<T> where T : IComparable<T>
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

        public int Compare(T x, T y)
        {
            return x.CompareTo(y);
        }
    }
}
