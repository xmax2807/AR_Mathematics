using System.Globalization;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Project.Utils.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        private const string MatchEmailPattern =
		@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
		+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
		+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
		+ @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        /// <summary>
        /// Uppercase
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string UppercaseAtIndex(this string Str, int index = 0)
        {
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
        public static bool IsNumeric(this string value)
        {
            return long.TryParse(value, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out var result);
        }
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Try Convert a string to any type (includes nullable)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryParse<T>(this string value, out T result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = default;
                return false;
            }

            result = Parse<T>(value);

            return !EqualityComparer<T>.Default.Equals(result, default);
        }

        /// <summary>
        /// Convert a string to any type (includes nullable)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Parse<T>(this string value)
        {
            if (value is T variable) return variable;

            try
            {
                //Handling Nullable types i.e, int?, double?, bool? .. etc
                if (Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
                }

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static bool IsEmail(this string email){
            if(email != null){
                return Regex.IsMatch(email, MatchEmailPattern);
            }
            return false;
        }

        public static string RandomString(int length)
        {
            System.Random random = new();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        const int lengthLimit = 16;

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, };
            // byte[] iv = new byte[] { (byte) 0x00, (byte) 0x00,
            //                     (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
            //                     (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
            //                     (byte) 0x00, (byte) 0x01, (byte) 0x23, (byte) 0x45,
            //                     (byte) 0x67, (byte) 0x89};

            byte[] array = null;

            if(key.Length <= lengthLimit){
                    while (key.Length < lengthLimit)
                {
                    key += '*';
                }
            }
            else{
               key = key.Substring(0, lengthLimit);
            }
            
            try
            {
                Aes aes = Aes.Create();

                KeySizes[] ks = aes.LegalKeySizes;
                foreach (KeySizes item in ks)
                {
                    Debug.Log("Legal min key size = " + item.MinSize);
                    Debug.Log("Legal max key size = " + item.MaxSize);
                    //Output
                    // Legal min key size = 128
                    // Legal max key size = 256
                }
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                Debug.Log(encryptor);
                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[lengthLimit] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

            // byte[] iv = new byte[] { (byte) 0x00, (byte) 0x00,
            //                     (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
            //                     (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
            //                     (byte) 0x00, (byte) 0x01, (byte) 0x23, (byte) 0x45,
            //                     (byte) 0x67, (byte) 0x89};
            byte[] buffer = Convert.FromBase64String(cipherText);

            if(key.Length <= lengthLimit){
                    while (key.Length < lengthLimit)
                {
                    key += '*';
                }
            }
            else{
               key = key.Substring(0, lengthLimit);
            }

            try
            {
                Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new(cryptoStream);
                return streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return null;
        }
    }
}
