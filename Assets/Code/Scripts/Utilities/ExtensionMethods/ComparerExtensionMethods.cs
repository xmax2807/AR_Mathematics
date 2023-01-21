using System.Collections.Generic;
using System;
namespace Project.Utils.ExtensionMethods{
    public static class ComparerExtensionMethods{
        public static void EnsureComparer<T>(this IComparer<T> comparer){
            if(comparer == null){
                throw new NullReferenceException("comparer cannot be null");
            }
        }
    }
}