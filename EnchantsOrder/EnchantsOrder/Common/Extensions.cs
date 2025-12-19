using System;

namespace EnchantsOrder.Common
{
    /// <summary>
    /// Provides extension methods for various functionalities.
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Converts the specified integer to its Roman numeral representation.
        /// </summary>
        /// <param name="num">The integer to convert. Must be a positive number.</param>
        /// <param name="maxValue">The maximum value for which a Roman numeral will be generated. If <paramref name="num"/> exceeds this value,
        /// the method returns the string representation of <paramref name="num"/> instead. The default is 2000.</param>
        /// <returns>A string containing the Roman numeral representation of <paramref name="num"/> if it is less than or equal
        /// to  <paramref name="maxValue"/>; otherwise, the string representation of <paramref name="num"/>.</returns>
        public static string GetRomanNumber(this int num, int maxValue = 2000)
        {
            if (num > maxValue) { return num.ToString(); }

            int[] Arabic = [1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1];
            string[] roman = ["M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"];

            int i = 0;
            string results = string.Empty;
            while (num > 0)
            {
                while (num >= Arabic[i])
                {
                    num -= Arabic[i];
                    results += roman[i];
                }
                i++;
            }
            return results;
        }

        /// <summary>
        /// Converts a penalty value to its corresponding experience value.
        /// </summary>
        /// <param name="penalty">The penalty value to convert. Must be a non-negative integer.</param>
        /// <returns>The experience value corresponding to the penalty.</returns>
        public static long PenaltyToLevel(int penalty) => penalty switch
        {
            <= 0 => 0,
            >= 63 => long.MaxValue,
            _ => (1 << penalty) - 1
        };

        /// <summary>
        /// Converts a level to its corresponding experience value.
        /// </summary>
        /// <param name="level">The level to convert. Must be a non-negative integer.</param>
        /// <returns>The experience value corresponding to the level.</returns>
        public static long LevelToExperience(long level) => level switch
        {
            <= 16 => (level * level) + (6 * level),
            <= 31 => ((5L * level * level) - (81L * level) + 720L) / 2L,
            _ => ((9L * level * level) - (325L * level) + 4440L) / 2L
        };
    }
}
