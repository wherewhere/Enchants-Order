namespace OrderEnchants
{
    internal static class Extensions
    {
        public static string GetLoumaNumber(this int num)
        {
            int[] arabic = new int[13];
            string[] roman = new string[13];
            int i = 0;
            string o = "";

            arabic[0] = 1000;
            arabic[1] = 900;
            arabic[2] = 500;
            arabic[3] = 400;
            arabic[4] = 100;
            arabic[5] = 90;
            arabic[6] = 50;
            arabic[7] = 40;
            arabic[8] = 10;
            arabic[9] = 9;
            arabic[10] = 5;
            arabic[11] = 4;
            arabic[12] = 1;

            roman[0] = "M";
            roman[1] = "CM";
            roman[2] = "D";
            roman[3] = "CD";
            roman[4] = "C";
            roman[5] = "XC";
            roman[6] = "L";
            roman[7] = "XL";
            roman[8] = "X";
            roman[9] = "IX";
            roman[10] = "V";
            roman[11] = "IV";
            roman[12] = "I";

            while (num > 0)
            {
                while (num >= arabic[i])
                {
                    num -= arabic[i];
                    o += roman[i];
                }
                i++;
            }
            return o;
        }
    }
}
