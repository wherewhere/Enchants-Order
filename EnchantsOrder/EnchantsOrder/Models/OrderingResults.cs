using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

#if WINRT
using Windows.Foundation;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The results of ordering enchantments, which contains the penalty, max experience, total experience and steps.
    /// </summary>
    /// <param name="penalty">The penalty of item.</param>
    /// <param name="maxExperience">The max experience level request during enchant.</param>
    /// <param name="totalExperience">The total experience level request during enchant.</param>
    /// <param name="steps">The steps of enchant.</param>
    public sealed class OrderingResults(int penalty, double maxExperience, double totalExperience, params IList<EnchantmentStep> steps) :
#if WINRT
        IStringable
#else
        System.IComparable<OrderingResults>
#endif
#if NET7_0_OR_GREATER
        , System.Numerics.IComparisonOperators<OrderingResults, OrderingResults, bool>
#endif
    {
        /// <summary>
        /// Gets the penalty of item.
        /// </summary>
        public int Penalty => penalty;

        /// <summary>
        /// Gets the max experience level request during enchant.
        /// </summary>
        public double MaxExperience => maxExperience;

        /// <summary>
        /// Gets the total experience level request during enchant.
        /// </summary>
        public double TotalExperience => totalExperience;

        /// <summary>
        /// Gets the steps of enchant.
        /// </summary>
        public IList<EnchantmentStep> Steps => steps;

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
            foreach (EnchantmentStep step in steps)
            {
                _ = builder.AppendLine(step.ToString());
            }
            return builder.AppendLine($"Penalty Level: {penalty}")
                          .AppendLine($"Max Experience Level: {maxExperience}")
                          .Append($"Total Experience Level: {totalExperience}")
                          .ToString();
        }

        /// <inheritdoc/>
        public int CompareTo(OrderingResults? other)
        {
            if (other == null) { return -1; }
            int value;
            if ((value = penalty.CompareTo(other.Penalty)) == 0)
            {
                if ((value = totalExperience.CompareTo(other.TotalExperience)) == 0)
                {
                    if ((value = maxExperience.CompareTo(other.MaxExperience)) == 0)
                    {
                        [MethodImpl((MethodImplOptions)0x100)]
                        static int GetStepNum(params IList<EnchantmentStep> steps) => steps.Sum(step => step.Count);
                        value = GetStepNum(steps).CompareTo(GetStepNum(other.Steps));
                    }
                }
            }
            return value;
        }

#if !WINRT
        /// <summary>
        /// Determines whether one <see cref="OrderingResults"/> instance is greater than another.
        /// </summary>
        /// <param name="left">The first <see cref="OrderingResults"/> instance to compare.</param>
        /// <param name="right">The second <see cref="OrderingResults"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >(OrderingResults left, OrderingResults right) => left.CompareTo(right) == 1;

        /// <summary>
        /// Determines whether one <see cref="OrderingResults"/> instance is greater than or equal to another instance.
        /// </summary>
        /// <param name="left">The first <see cref="OrderingResults"/> instance to compare.</param>
        /// <param name="right">The second <see cref="OrderingResults"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(OrderingResults left, OrderingResults right) => left.CompareTo(right) != -1;

        /// <summary>
        /// Determines whether one <see cref="OrderingResults"/> instance is less than another instance.
        /// </summary>
        /// <param name="left">The first <see cref="OrderingResults"/> instance to compare.</param>
        /// <param name="right">The second <see cref="OrderingResults"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <(OrderingResults left, OrderingResults right) => left.CompareTo(right) == -1;

        /// <summary>
        /// Determines whether one <see cref="OrderingResults"/> instance is less than or equal to another instance.
        /// </summary>
        /// <param name="left">The first <see cref="OrderingResults"/> instance to compare.</param>
        /// <param name="right">The second <see cref="OrderingResults"/> instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(OrderingResults left, OrderingResults right) => left.CompareTo(right) != 1;
#endif

#if NET7_0_OR_GREATER
        /// <inheritdoc/>
        static bool System.Numerics.IEqualityOperators<OrderingResults, OrderingResults, bool>.operator ==(OrderingResults? left, OrderingResults? right) => Comparer<OrderingResults>.Default.Compare(left, right) == 0;

        /// <inheritdoc/>
        static bool System.Numerics.IEqualityOperators<OrderingResults, OrderingResults, bool>.operator !=(OrderingResults? left, OrderingResults? right) => !(left == right);
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
