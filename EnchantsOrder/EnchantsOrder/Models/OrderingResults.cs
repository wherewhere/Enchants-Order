using System.Collections.Generic;
using System.Text;

namespace EnchantsOrder.Models
{
    public class OrderingResults
    {
        public int Penalty { get; set; }
        public double MaxExperience { get; set; }
        public double TotalExperience { get; set; }
        public IList<EnchantmentStep> Steps { get; set; }

        public OrderingResults(IList<EnchantmentStep> steps, int penalty, double maxExperience, double totalExperience)
        {
            Steps = steps;
            Penalty = penalty;
            MaxExperience = maxExperience;
            TotalExperience = totalExperience;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (EnchantmentStep step in Steps)
            {
                builder.AppendLine(step.ToString());
            }
            builder.AppendLine($"Penalty: {Penalty}");
            builder.AppendLine($"Max Experience: {MaxExperience}");
            builder.AppendLine($"Total Experience: {TotalExperience}");
            return builder.ToString();
        }
    }
}
