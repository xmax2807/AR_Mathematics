using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Project.Utils
{
    public static class SecurePlayerPrefs
    {
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");
        public static byte[] GenerateRandomIV()
        {
            using (RNGCryptoServiceProvider rng = new())
            {
                byte[] initVectorBytes = new byte[16];
                rng.GetBytes(initVectorBytes);
                return initVectorBytes;
            }
        }

        public static void SetString(string key, string value, string passphrase)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(value);
            using (RijndaelManaged symmetricKey = new())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(GetKey(passphrase), initVectorBytes))
                {
                    byte[] cipherTextBytes;
                    using (System.IO.MemoryStream memoryStream = new())
                    {
                        using (CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherTextBytes = memoryStream.ToArray();
                        }
                    }
                    PlayerPrefs.SetString(key, Convert.ToBase64String(cipherTextBytes));
                }
            }
        }

        public static string GetString(string key, string passphrase)
        {
            string value = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(value))
                return "";

            byte[] cipherTextBytes = Convert.FromBase64String(value);
            using (RijndaelManaged symmetricKey = new())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(GetKey(passphrase), initVectorBytes))
                {
                    using (System.IO.MemoryStream memoryStream = new(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                        }
                    }
                }
            }
        }

        private static byte[] GetKey(string passphrase)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(passphrase));
            }
        }
    }

}