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
        public TooExpensiveException Exception { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="OrderingResults" />.
        /// </summary>
        public OrderingResults(IList<EnchantmentStep> steps, int penalty, double maxExperience, double totalExperience, TooExpensiveException exception = null)
        {
            Steps = steps;
            Penalty = penalty;
            Exception = exception;
            MaxExperience = maxExperience;
            TotalExperience = totalExperience;
        }

        /// <summary>
        /// Throw Exception.
        /// </summary>
        public void Throw() { throw Exception; }

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (EnchantmentStep step in Steps)
            {
                builder.AppendLine(step.ToString());
            }
            builder.AppendLine($"Penalty Level: {Penalty}");
            builder.AppendLine($"Max Experience Level: {MaxExperience}");
            builder.AppendLine($"Total Experience Level: {TotalExperience}");
            return builder.ToString();
        }
    }
}
