using System;

namespace EnchantsOrder
{
    internal static class Extensions
    {
        public static string GetLoumaNumber(this int num, int maxvalue = 2000)
        {
            if (num > maxvalue) { return num.ToString(); }

            int[] arabic = new int[13] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] roman = new string[13] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            int i = 0;
            string results = string.Empty;

            while (num > 0)
            {
                while (num >= arabic[i])
                {
                    num -= arabic[i];
                    results += roman[i];
                }
                i++;
            }
            return results;
        }

        public static long PenaltyToExperience(long penalty) => Math.Max(0, Convert.ToInt64(Math.Pow(2, penalty)) - 1);

        public static double LevelToExperience(long level) =>
            level <= 16
                ? Math.Pow(level, 2) + (6 * level) : level <= 31
                    ? (2.5 * Math.Pow(level, 2)) - (40.5 * level) + 360 : (4.5 * Math.Pow(level, 2)) - (162.5 * level) + 2220;
    }
}
