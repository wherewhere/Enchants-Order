using System.Collections.Generic;
using System.Linq;
using System.Text;

#if WINRT
using Windows.Foundation;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The result of ordering.
    /// </summary>
    /// <param name="steps">The steps of enchant.</param>
    /// <param name="penalty">The penalty of item.</param>
    /// <param name="maxExperience">The max experience level request during enchant.</param>
    /// <param name="totalExperience">The total experience level request during enchant.</param>
#if WINRT
    sealed
#endif
    public class OrderingResults(IList<IEnchantmentStep> steps, int penalty, double maxExperience, double totalExperience) : IOrderingResults
#if WINRT
        , IStringable
#endif
    {
        /// <summary>
        /// Gets or sets the penalty of item.
        /// </summary>
        public int Penalty => penalty;

        /// <summary>
        /// Gets or sets the max experience level request during enchant.
        /// </summary>
        public double MaxExperience => maxExperience;

        /// <summary>
        /// Gets or sets the total experience level request during enchant.
        /// </summary>
        public double TotalExperience => totalExperience;

        /// <summary>
        /// Gets or sets the steps of enchant.
        /// </summary>
        public IList<IEnchantmentStep> Steps => steps;

        /// <summary>
        /// Gets the status whether it is too expensive.
        /// </summary>
        /// <remarks>Too expensive because max experience level max than 39.</remarks>
        public bool IsTooExpensive => MaxExperience > max_experience;

        /// <summary>
        /// Gets the status whether it is too many penalty.
        /// </summary>
        /// <remarks>Max penalty max than 6 so that you cannot enchant any more.</remarks>
        public bool IsTooManyPenalty => Penalty > max_penalty;

        /// <inheritdoc/>
        public override string ToString()
        {
            StringBuilder builder = new();
            foreach (IEnchantmentStep step in Steps)
            {
                _ = builder.AppendLine(step.ToString());
            }
            return builder.AppendLine($"Penalty Level: {Penalty}")
                          .AppendLine($"Max Experience Level: {MaxExperience}")
                          .Append($"Total Experience Level: {TotalExperience}")
                          .ToString();
        }

        /// <inheritdoc/>
        public int CompareTo(IOrderingResults? other)
        {
            if (other == null) { return -1; }
            int value;
            if ((value = Penalty.CompareTo(other.Penalty)) == 0)
            {
                if ((value = TotalExperience.CompareTo(other.TotalExperience)) == 0)
                {
                    if ((value = MaxExperience.CompareTo(other.MaxExperience)) == 0)
                    {
                        static int GetStepNum(IList<IEnchantmentStep> steps) => steps.Sum(step => step.Count);
                        value = GetStepNum(Steps).CompareTo(GetStepNum(other.Steps));
                    }
                }
            }
            return value;
        }

#if !WINRT
        /// <inheritdoc/>
        public static bool operator >(OrderingResults left, OrderingResults right) => left.CompareTo(right) == 1;

        /// <inheritdoc/>
        public static bool operator >=(OrderingResults left, OrderingResults right) => left.CompareTo(right) != -1;

        /// <inheritdoc/>
        public static bool operator <(OrderingResults left, OrderingResults right) => left.CompareTo(right) == -1;

        /// <inheritdoc/>
        public static bool operator <=(OrderingResults left, OrderingResults right) => left.CompareTo(right) != 1;
#endif

        /// <summary>
        /// The max penalty of item.
        /// </summary>
        private const short max_penalty = 6;

        /// <summary>
        /// The max experience level during enchant.
        /// </summary>
        private const short max_experience = 39;
    }
}
