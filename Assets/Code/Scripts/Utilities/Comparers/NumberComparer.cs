using System;
using System.Collections.Generic;
using Project.Utils.MathProvider;

namespace Project.Utils
{
    public abstract class NumberComparer<T> : IComparer<T> where T : IComparable<T>
    {
        private bool m_isDescending;
        private MathProvider<T> m_mathProvider;
        public NumberComparer(bool IsDescending, MathProvider<T> mathProvider){
            m_mathProvider = mathProvider;
            m_isDescending = IsDescending;
        }
        public int Compare(T x, T y)
        {
            if(m_isDescending){
                return m_mathProvider.Compare(y,x);
            }
            return m_mathProvider.Compare(x,y);
        }
    }

    public class IntNumberComparer : NumberComparer<int>{
        public IntNumberComparer(bool IsDescending = false): base(IsDescending, new IntMathProvider()){}
        
    }
}
