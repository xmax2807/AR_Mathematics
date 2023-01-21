using System;
using System.Linq;
using System.Collections.Generic;

namespace Project.Utils.ExtensionMethods
{
    public static class ListExtensionMethods{
        /// <summary>
        /// Find the largest element in 
        /// </summary>
        /// <param name="List"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindLargest<T>(this IEnumerable<T> List, IComparer<T> comparer){
            IEnumerator<T> iterator = List.GetEnumerator();
            T valueToCompare = iterator.Current;
            while(iterator.MoveNext()){
                if(comparer.Compare(valueToCompare, iterator.Current) < 0){
                    valueToCompare = iterator.Current;
                }
            }
            return valueToCompare;
        }
        public static T FindSmallest<T>(this IEnumerable<T> List, IComparer<T> comparer){
            IEnumerator<T> iterator = List.GetEnumerator();
            T valueToCompare = iterator.Current;
            while(iterator.MoveNext()){
                if(comparer.Compare(valueToCompare, iterator.Current) > 0){
                    valueToCompare = iterator.Current;
                }
            }
            return valueToCompare;
        }

        /// <summary>
        /// Traverse the list and check condition. If condition tells to stop, return method with result if traversed whole list or not
        /// </summary>
        /// <param name="List"></param>
        /// <param name="stopCondition"></param>
        /// <param name="result"> is the list traversed fully or not</param>
        /// <typeparam name="T"></typeparam>
        public static void TraverseWithCondition<T>(this T[] List, Func<int,bool> stopCondition, out bool result){
            for(int i = 0; i < List.Length; i++){
                if(stopCondition != null && stopCondition.Invoke(i) == true){
                    result = false;
                    return;
                }
            }
            result = true;
        }

        /// <summary>
        /// Check if an array is sorted or not.
        /// </summary>
        /// <param name="List">Given List</param>
        /// <param name="comparer">Comparer</param>
        /// <param name="isDescending">Check sort order</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>true - Array is Sort. false - Array is not sorted</returns>
        public static bool IsSorted<T>(this IEnumerable<T> List, IComparer<T> comparer, bool isDescending = false){
            comparer.EnsureComparer();

            T[] array = List.ToArray();
            
            //Local function
            bool stopCondition(int index)
            {
                if (index - 1 < 0) return false;// 

                int compareResult = isDescending ? 1 : -1;

                return comparer.Compare(array[index - 1], array[index]) == compareResult;
            }

            TraverseWithCondition(array, stopCondition, out bool result);
            return result;
        }
    }
}