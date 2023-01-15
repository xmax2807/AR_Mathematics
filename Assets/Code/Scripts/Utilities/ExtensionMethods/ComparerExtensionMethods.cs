using System.Collections.Generic;
using System;
namespace Project.Utils.ExtensionMethods{
    public static class ComparerExtensionMethods{
        public static void EnsureComparer<T>(ref IComparer<T> comparer) where T : IComparable<T>{
            if(comparer == null){
                if(typeof(T).BaseType is IComparable<T>) {
                    comparer = DefaultComparableComparer<T>.Instance;
                }
            }
        }
    }
}