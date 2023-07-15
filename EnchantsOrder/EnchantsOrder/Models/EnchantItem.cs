using EnchantsOrder.Common;
using System;

namespace EnchantsOrder.Models
{
    internal class EnchantItem(long num = 0, long penalty = 0, long historyExperience = 0) : IComparable<EnchantItem>
    {
        public long Level { get; set; } = num;
        public long Penalty { get; set; } = penalty;
        public long StepLevel { get; private set; }
        public long HistoryLevel { get; set; } = historyExperience;
        public long HistoryExperience { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"Level:{Level} Penalty:{Penalty}";

        /// <inheritdoc/>
        public int CompareTo(EnchantItem other)
        {
            long leftValue = Level + Extensions.PenaltyToExperience(Penalty);
            long rightValue = other.Level + Extensions.PenaltyToExperience(other.Penalty);
            return leftValue.CompareTo(rightValue);
        }

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
        public static EnchantItem operator +(EnchantItem left, EnchantItem right)
        {
            EnchantItem level = new()
            {
                Level = left.Level + right.Level,
                Penalty = Math.Max(left.Penalty, right.Penalty) + 1,
                StepLevel = right.Level + Extensions.PenaltyToExperience(left.Penalty) + Extensions.PenaltyToExperience(right.Penalty)
            };
            level.HistoryLevel = level.StepLevel + left.HistoryLevel + right.HistoryLevel;
            level.HistoryExperience = Convert.ToInt64(Extensions.LevelToExperience(level.StepLevel)) + left.HistoryExperience + right.HistoryExperience;
            return level;
        }

        /// <inheritdoc/>
        public static EnchantItem operator *(EnchantItem left, long right)
        {
            long PenaltyLevel = 0;
            for (long i = 2; i < right; i++)
            {
                PenaltyLevel += Extensions.PenaltyToExperience(left.Penalty + i - 2) + Extensions.PenaltyToExperience(left.Penalty);
            }
            EnchantItem level = new()
            {
                Level = left.Level * right,
                Penalty = Math.Max(left.Penalty + right - 1, 0),
                StepLevel = (left.Level * Math.Max(right - 1, 0)) + PenaltyLevel
            };
            level.HistoryLevel = level.StepLevel + (left.HistoryLevel * right);
            level.HistoryExperience = Convert.ToInt64(Extensions.LevelToExperience(level.StepLevel)) + (left.HistoryExperience * right);
            return level;
        }

        /// <inheritdoc/>
        public static EnchantItem operator *(long right, EnchantItem left) => left * right;
    }
}
