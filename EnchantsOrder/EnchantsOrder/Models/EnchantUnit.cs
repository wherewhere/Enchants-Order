using EnchantsOrder.Common;
using System;

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
    internal readonly struct EnchantUnit(long level = 0, int penalty = 0, long stepLevel = 0, long historyLevel = 0, long historyExperience = 0)
#if NET7_0_OR_GREATER
        : System.Numerics.IAdditionOperators<EnchantUnit, EnchantUnit, EnchantUnit>
#endif
    {
        private readonly long _level = level;
        private readonly int _penalty = penalty;
        private readonly long _stepLevel = stepLevel;
        private readonly long _historyLevel = historyLevel;
        private readonly long _historyExperience = historyExperience;

        /// <summary>
        /// Gets the current level of this enchantment item.
        /// </summary>
        public long Level => _level;

        /// <summary>
        /// Gets the current penalty of this enchantment item.
        /// </summary>
        public int Penalty => _penalty;

        /// <summary>
        /// Gets the level cost for this step.
        /// </summary>
        public long StepLevel => _stepLevel;

        /// <summary>
        /// Gets the history level cost of this enchantment item.
        /// </summary>
        public long HistoryLevel => _historyLevel;

        /// <summary>
        /// Gets the history experience cost of this enchantment item.
        /// </summary>
        public long HistoryExperience => _historyExperience;

        /// <summary>
        /// Applies an enchantment to the item, optionally adjusting the enchantment by a specified penalty.
        /// </summary>
        /// <param name="penalty">The penalty value to apply to the enchantment. The penalty reduces the effectiveness of the enchantment. The
        /// default is 0.</param>
        /// <returns>An <see cref="EnchantUnit"/> representing the result of the enchantment after applying the specified penalty.</returns>
        public EnchantUnit EnchantItem(int penalty = 0) => new EnchantUnit(0, penalty) + this;

        /// <inheritdoc/>
        public override string ToString() => $"Level:{_level} Penalty:{_penalty}";

        /// <summary>
        /// Adds two <see cref="EnchantUnit"/> instances, combining their levels and penalties according to enchantment rules.
        /// </summary>
        /// <param name="left">The first <see cref="EnchantUnit"/> to add.</param>
        /// <param name="right">The second <see cref="EnchantUnit"/> to add.</param>
        /// <returns>A new <see cref="EnchantUnit"/> representing the combined result of the two input units.</returns>
        public static EnchantUnit operator +(EnchantUnit left, EnchantUnit right)
        {
            long level = left._level + right._level;
            int penalty = Math.Max(left._penalty, right._penalty) + 1;
            long stepLevel = right._level + Extensions.PenaltyToLevel(left._penalty) + Extensions.PenaltyToLevel(right._penalty);
            long historyLevel = stepLevel + left._historyLevel + right._historyLevel;
            long historyExperience = Extensions.LevelToExperience(stepLevel) + left._historyExperience + right._historyExperience;
            return new EnchantUnit(level, penalty, stepLevel, historyLevel, historyExperience);
        }
    }
}
