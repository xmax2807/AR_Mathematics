using System.Collections.Generic;
using System;
using System.Linq;
namespace Project.Utils.ExtensionMethods
{
    public class ListComparer<T> : IEqualityComparer<T[]>
    {
        private IEqualityComparer<T> m_comparerT;
        public ListComparer(IEqualityComparer<T> comparer)
        {
            m_comparerT = comparer;
        }

        public bool Equals(T[] x, T[] y)
        {
            return x.SequenceEqual(y, m_comparerT);
        }

        public int GetHashCode(T[] obj)
        {
            int hash = 17;
            foreach (T item in obj)
            {
                hash = hash * 23 + item.GetHashCode();
            }
            return hash;
        }
    }
    public static class ComparerExtensionMethods
    {
        public static void EnsureComparer<T>(this IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new NullReferenceException("comparer cannot be null");
            }
        }
    }
}