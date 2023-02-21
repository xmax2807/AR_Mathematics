using System;
using System.Collections.Generic;

namespace Project.Utils.ExtensionMethods{
    public static class IntegerExtensionMethods{

        /// <summary>
        /// Ensure given value in range from min to max - 1 (max is excluded)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max">This value is excluded</param>
        /// <param name="min"></param>
        /// <returns>a new integer value in range</returns>
        public static void EnsureInRange(this ref int value, int max, int min = 0){
            value = value < min ? min : Math.Min(value, max);
        }

        /// <summary>
        /// Find pairs that sums equals to the given number
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="numOfPairs"></param>
        /// <returns>Get list of pair</returns>
        public static Tuple<int,int>[] GetPairsWithSum(this ref int sum, int numOfPairs = 1){
            Random rand = new();
            List<int> UniqueNums = new (numOfPairs);
            for(int i = 0; i < numOfPairs; i++){
                int randNum = rand.Next(1, sum);
                if(UniqueNums.Contains(randNum)){
                    continue;
                }

                UniqueNums.Add(randNum);
            }

            Tuple<int,int>[] result = new Tuple<int,int>[numOfPairs];
            for(int i = 0; i < numOfPairs; i++){
                result[i] = new Tuple<int, int>(UniqueNums[i], sum - UniqueNums[i]);
            }

            return result;
        }

        /// <summary>
        /// Find pair that sums equals to the given number
        /// </summary>
        /// <param name="sum"></param>
        /// <returns>random pair</returns>
        public static Tuple<int,int> GetPairWithSum(this ref int sum){
            Random rand = new();
            int num = rand.Next(1, sum);
            return new Tuple<int, int>(num, sum - num);
        }
    }
}