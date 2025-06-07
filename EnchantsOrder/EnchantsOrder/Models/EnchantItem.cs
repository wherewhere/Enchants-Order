using EnchantsOrder.Common;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EnchantsOrder.Models
{
    /// <summary>
    /// Represents an item of enchantment with level, penalty, and history experience.
    /// </summary>
    /// <param name="level">The current level of this enchantment item.</param>
    /// <param name="penalty">The current penalty of this enchantment item.</param>
    /// <param name="stepLevel">The level cost for this step.</param>
    /// <param name="historyLevel">The history level cost of this enchantment item.</param>
    /// <param name="historyExperience">The history experience cost of this enchantment item.</param>
    internal readonly struct EnchantItem(long level, long penalty, long stepLevel, long historyLevel, long historyExperience) : IComparable<EnchantItem>, IEquatable<EnchantItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnchantItem"/> class with specified level and penalty.
        /// </summary>
        /// <param name="level">The current level of this enchantment item.</param>
        /// <param name="penalty">The current penalty of this enchantment item.</param>
        public EnchantItem(long level = 0, long penalty = 0) : this(level, penalty, 0, 0, 0)
        {
        }

        /// <summary>
        /// Gets or sets the current level of this enchantment item.
        /// </summary>
        public long Level => level;

        /// <summary>
        /// Gets or sets the current penalty of this enchantment item.
        /// </summary>
        public long Penalty => penalty;

        /// <summary>
        /// Gets or sets the level cost for this step.
        /// </summary>
        public long StepLevel => stepLevel;

        /// <summary>
        /// Gets or sets the history level cost of this enchantment item.
        /// </summary>
        public long HistoryLevel => historyLevel;

        /// <summary>
        /// Gets or sets the history experience cost of this enchantment item.
        /// </summary>
        public long HistoryExperience => historyExperience;

        /// <inheritdoc/>
        public override string ToString() => $"Level:{level} Penalty:{penalty}";

        /// <inheritdoc/>
        public int CompareTo(EnchantItem other)
        {
            long leftValue = level + Extensions.PenaltyToExperience(penalty);
            long rightValue = other.Level + Extensions.PenaltyToExperience(other.Penalty);
            return leftValue.CompareTo(rightValue);
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is EnchantItem item && Equals(item);

        /// <inheritdoc/>
        public bool Equals(EnchantItem other) =>
            level == other.Level &&
            penalty == other.Penalty &&
            stepLevel == other.StepLevel &&
            historyLevel == other.HistoryLevel &&
            historyExperience == other.HistoryExperience;

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(level, penalty, stepLevel, historyLevel, historyExperience);

        /// <inheritdoc/>
        public static bool operator >(EnchantItem left, EnchantItem right)
        {
            long leftValue = left.Level + Extensions.PenaltyToExperience(left.Penalty);
            long rightValue = right.Level + Extensions.PenaltyToExperience(right.Penalty);
            return leftValue > rightValue;
        }

        /// <inheritdoc/>
        public static bool operator >=(EnchantItem left, EnchantItem right)
        {
            long leftValue = left.Level + Extensions.PenaltyToExperience(left.Penalty);
            long rightValue = right.Level + Extensions.PenaltyToExperience(right.Penalty);
            return leftValue >= rightValue;
        }

        /// <inheritdoc/>
        public static bool operator <(EnchantItem left, EnchantItem right)
        {
            long leftValue = left.Level + Extensions.PenaltyToExperience(left.Penalty);
            long rightValue = right.Level + Extensions.PenaltyToExperience(right.Penalty);
            return leftValue < rightValue;
        }

        /// <inheritdoc/>
        public static bool operator <=(EnchantItem left, EnchantItem right)
        {
            long leftValue = left.Level + Extensions.PenaltyToExperience(left.Penalty);
            long rightValue = right.Level + Extensions.PenaltyToExperience(right.Penalty);
            return leftValue <= rightValue;
        }

        /// <inheritdoc/>
        public static bool operator ==(EnchantItem left, EnchantItem right)
        {
            long leftValue = left.Level + Extensions.PenaltyToExperience(left.Penalty);
            long rightValue = right.Level + Extensions.PenaltyToExperience(right.Penalty);
            return leftValue == rightValue;
        }

        /// <inheritdoc/>
        public static bool operator !=(EnchantItem left, EnchantItem right)
        {
            long leftValue = left.Level + Extensions.PenaltyToExperience(left.Penalty);
            long rightValue = right.Level + Extensions.PenaltyToExperience(right.Penalty);
            return leftValue != rightValue;
        }

        /// <inheritdoc/>
        public static EnchantItem operator +(EnchantItem left, EnchantItem right)
        {
            long level = left.Level + right.Level;
            long penalty = Math.Max(left.Penalty, right.Penalty) + 1;
            long stepLevel = right.Level + Extensions.PenaltyToExperience(left.Penalty) + Extensions.PenaltyToExperience(right.Penalty);
            long historyLevel = stepLevel + left.HistoryLevel + right.HistoryLevel;
            long historyExperience = Convert.ToInt64(Extensions.LevelToExperience(stepLevel)) + left.HistoryExperience + right.HistoryExperience;
            return new EnchantItem(level, penalty, stepLevel, historyLevel, historyExperience);
        }

        /// <inheritdoc/>
        public static EnchantItem operator *(EnchantItem left, long right)
        {
            long PenaltyLevel = 0;
            for (long i = 2; i < right; i++)
            {
                PenaltyLevel += Extensions.PenaltyToExperience(left.Penalty + i - 2) + Extensions.PenaltyToExperience(left.Penalty);
            }
            long level = left.Level * right;
            long penalty = Math.Max(left.Penalty + right - 1, 0);
            long stepLevel = (left.Level * Math.Max(right - 1, 0)) + PenaltyLevel;
            long historyLevel = stepLevel + (left.HistoryLevel * right);
            long historyExperience = Convert.ToInt64(Extensions.LevelToExperience(stepLevel)) + (left.HistoryExperience * right);
            return new EnchantItem(level, penalty, stepLevel, historyLevel, historyExperience);
        }

        /// <inheritdoc/>
        public static EnchantItem operator *(long right, EnchantItem left) => left * right;
    }
}
