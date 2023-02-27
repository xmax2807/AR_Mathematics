using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Project.Utils.ExtensionMethods
{
    public static class ListExtensionMethods{
        /// <summary>
        /// Find the largest item in list
        /// </summary>
        /// <param name="List"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindLargest<T>(this IEnumerable<T> List, IComparer<T> comparer){
            IEnumerator<T> iterator = List.GetEnumerator();

            if(!iterator.MoveNext()) return default;
            
            T valueToCompare = iterator.Current;

            while(iterator.MoveNext()){
                if(valueToCompare == null){
                    valueToCompare = iterator.Current;
                    continue;
                }
                if(comparer.Compare(valueToCompare, iterator.Current) < 0){
                    valueToCompare = iterator.Current;
                }
            }
            return valueToCompare;
        }

        public static TProperty FindLargestPropertyInObjects<T,TProperty>(this IEnumerable<T> List, Func<T, TProperty> getProperty) where TProperty :IComparable<TProperty>{
            IEnumerator<T> iterator = List.GetEnumerator();

            if(!iterator.MoveNext() || getProperty == null) return default;
            
            TProperty valueToCompare = getProperty(iterator.Current);

            while(iterator.MoveNext()){
                TProperty newValue = getProperty(iterator.Current);
                
                if(valueToCompare == null){
                    valueToCompare = newValue;
                    continue;
                }
                if(newValue.CompareTo(valueToCompare) < 0){
                    valueToCompare = newValue;
                }
            }
            return valueToCompare;
        }
        /// <summary>
        /// Find the smallest item in list
        /// </summary>
        /// <param name="List">given list</param>
        /// <param name="comparer"> Comparer<T> </param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindSmallest<T>(this IEnumerable<T> List, IComparer<T> comparer){
            IEnumerator<T> iterator = List.GetEnumerator();
            
            if(!iterator.MoveNext()) return default;

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

        /// <summary>
        /// Find Closet Match base on condition
        /// </summary>
        /// <param name="List">given List</param>
        /// <param name="condition"> Delegate invokes the function </param>
        /// <param name="closestMatch"> reference parameter as result </param>
        /// <typeparam name="T"></typeparam>
        public static void FindMatch<T>(this IEnumerable<T> List, Func<T, bool> condition, ref T closestMatch){
            if(condition == null) return;

            IEnumerator<T> iterator = List.GetEnumerator();

            if(!iterator.MoveNext()) return;

            closestMatch = iterator.Current;
            while(iterator.MoveNext()){
                if(condition.Invoke(iterator.Current)){
                    closestMatch = iterator.Current;
                }
            }
        }

        public static void SplitLoop(int count, int loopCount, Action eachLoopAction){
            for(int i = 0; i < count;){
                int limit = Math.Min(loopCount, count - i) + i;
                while(i < limit){
                    eachLoopAction?.Invoke();
                    i++;
                }
            }
        }
    }
}