using System;

namespace Project.ExtensionMethods{
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
    }
}