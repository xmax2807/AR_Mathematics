using System;
using System.Collections.Generic;
namespace Project.Utils.ExtensionMethods
{
    public static class FlagExtensionMethods
    {
        public static bool HasValue<TFlag>(this TFlag flags, TFlag flagToCheck) where TFlag : System.Enum
        {
            if (!Attribute.IsDefined(typeof(TFlag), typeof(FlagsAttribute)))
                return false;

            if (flagToCheck.Equals(0)) throw new ArgumentOutOfRangeException(nameof(flagToCheck), "Value must not be 0");

            int flagsInt = Convert.ToInt32(flags);
            int flagInt = Convert.ToInt32(flagToCheck);

            return (flagsInt & flagInt) != 0;
        }
        public static int[] ReturnSelectedElements<T>(this T flags) where T : Enum
        {
            List<int> selectedElements = new();
            int count = Enum.GetValues(typeof(T)).Length;
            int flagValue = Convert.ToInt32(flags);

            for (int i = 0; i < count; i++)
            {
                int layer = 1 << i;
                if ((flagValue & layer) != 0)
                {
                    selectedElements.Add(i);
                }
            }

            return selectedElements.ToArray();

        }
    }
}