using System.Globalization;
using System;
using System.ComponentModel;

namespace Project.Utils.ExtensionMethods {
    public static class StringExtensionMethods{

        /// <summary>
        /// Uppercase
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string UppercaseAtIndex(this string Str, int index = 0){
            index.EnsureInRange(Str.Length);
            
            char[] chars = Str.ToCharArray();
            chars[index] = char.ToUpper(chars[index]);
            return chars.ToString();
        }

        /// <summary>
        /// Try check if given string is numeric
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value){
            return long.TryParse(value, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var result);
        }
        public static T ToEnum<T>(this string value) where T : Enum{
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Try Convert a string to any type (includes nullable)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryParse<T>(this string value, out T result){
            if(string.IsNullOrEmpty(value)) {
                result = default;
                return false;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            
            result = (T) converter.ConvertFrom(value);

            return true;
        }
    }
}
