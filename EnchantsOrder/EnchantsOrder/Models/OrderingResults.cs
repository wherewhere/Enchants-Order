using System;
using System.Collections.Generic;
using System.Text;

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The result of ordering.
    /// </summary>
    public class OrderingResults : IComparable<OrderingResults>
    {
        /// <summary>
        /// The penalty of item.
        /// </summary>
        public int Penalty { get; set; }

        /// <summary>
        /// The max experience level request during enchant.
        /// </summary>
        public double MaxExperience { get; set; }

        /// <summary>
        /// The total experience level request during enchant.
        /// </summary>
        public double TotalExperience { get; set; }

        /// <summary>
        /// The steps of enchant.
        /// </summary>
        public IList<EnchantmentStep> Steps { get; set; }

        /// <summary>
        /// Too expensive because max experience level max than 39.
        /// </summary>
        public bool TooExpensive => MaxExperience > max_experience;

        /// <summary>
        /// Max penalty max than 6 so that you cannot enchant any more.
        /// </summary>
        public bool TooManyPenalty => Penalty > max_penalty;

        /// <summary>
        /// Initializes a new instance of <see cref="OrderingResults" />.
        /// </summary>
        public OrderingResults(IList<EnchantmentStep> steps, int penalty, double maxExperience, double totalExperience)
        {
            Steps = steps;
            Penalty = penalty;
            MaxExperience = maxExperience;
            TotalExperience = totalExperience;
        }

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
            builder.Append($"Total Experience Level: {TotalExperience}");
            return builder.ToString();
        }

        /// <inheritdoc/>
        public int CompareTo(OrderingResults other)
        {
            int value = Penalty.CompareTo(other.Penalty);
            if (value == 0)
            {
                value = TotalExperience.CompareTo(other.TotalExperience);
                if (value == 0)
                {
                    value = MaxExperience.CompareTo(other.MaxExperience);
                    if (value == 0)
                    {
                        int GetStepNum(IList<EnchantmentStep> steps)
                        {
                            int num = 0;
                            foreach (EnchantmentStep step in Steps)
                            {
                                num += step.Count;
                            }
                            return num;
                        }
                        value = GetStepNum(Steps).CompareTo(GetStepNum(other.Steps));
                    }
                }
            }
            return value;
        }

        /// <inheritdoc/>
        public static bool operator >(OrderingResults left, OrderingResults right) => left.CompareTo(right) == 1;

        /// <inheritdoc/>
        public static bool operator >=(OrderingResults left, OrderingResults right) => left.CompareTo(right) != -1;

        /// <inheritdoc/>
        public static bool operator <(OrderingResults left, OrderingResults right) => left.CompareTo(right) == -1;

        /// <inheritdoc/>
        public static bool operator <=(OrderingResults left, OrderingResults right) => left.CompareTo(right) != 1;

        internal const short max_penalty = 6;
        internal const short max_experience = 39;
    }
}
