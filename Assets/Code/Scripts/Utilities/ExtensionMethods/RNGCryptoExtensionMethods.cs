using System;
using System.Security.Cryptography;
namespace Project.Utils.ExtensionMethods
{
    public static class RNGCryptoExtensionMethods
    {
        public static int GetNextInt32(this RNGCryptoServiceProvider random)
        {
            var buffer = new byte[sizeof(int)];
            random.GetBytes(buffer);
            return Math.Abs(BitConverter.ToInt32(buffer, 0));
        }
        public static int GetNextInt32(this RNGCryptoServiceProvider random, int min, int max)
        {
            if (min > max) throw new ArgumentOutOfRangeException("min");
            if (min == max) return min;
            long diff = (long)max - min;
            long upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                var buffer = new byte[sizeof(uint)];
                random.GetBytes(buffer);
                ui = BitConverter.ToUInt32(buffer, 0);
            } while (ui >= upperBound);
            return (int)(min + (ui % diff));
        }
        public static double GetNextDouble(this RNGCryptoServiceProvider random, double min, double max)
        {
            if (min > max) throw new ArgumentOutOfRangeException("min");
            if (min == max) return min;
            var buffer = new byte[sizeof(uint)];
            random.GetBytes(buffer);
            uint ui = BitConverter.ToUInt32(buffer, 0);
            return min + (max - min) * (ui / (double)uint.MaxValue);
        }
    }
}