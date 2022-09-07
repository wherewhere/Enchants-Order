using System.Collections.Generic;
using System.Linq;

namespace EnchantsOrder.Models
{
    public class EnchantmentStep : List<Enchantment>
    {
        public int Step { get; set; }

        public EnchantmentStep(int step)
        {
            Step = step;
        }

        public override string ToString()
        {
            string str = $"Step {Step}:";
            int half = Count / 2;
            for (int i = half; i > 0; i--)
            {
                bool istwo = Count == 2;
                int index = (half * 2) - (i * 2);
                str += $" {(istwo ? "" : "(")}{this[index]} + {this[index + 1]}{(istwo ? "" : ")")}";
            }
            if (Count % 2 == 1)
            {
                str += $"{(Count == 1 ? "" : " +")} {this.Last()}";
            }
            return str;
        }
    }
}
