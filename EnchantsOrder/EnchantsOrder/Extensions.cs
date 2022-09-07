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
    }
}
