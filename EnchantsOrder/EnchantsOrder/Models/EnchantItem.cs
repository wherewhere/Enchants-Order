using System;
using EnchantsOrder.Common;

namespace EnchantsOrder.Models
{
    internal class EnchantItem
    {
        public long Level { get; set; }
        public long Penalty { get; set; }
        public long StepLevel { get; private set; }
        public long HistoryLevel { get; set; }
        public long HistoryExperience { get; set; }

        public EnchantItem()
        {
        }

        public EnchantItem(long num, long penalty = 0, long historyExperience = 0)
        {
            Level = num;
            Penalty = penalty;
            HistoryLevel = historyExperience;
        }

        /// <inheritdoc/>
        public static EnchantItem operator +(EnchantItem left, EnchantItem right)
        {
            EnchantItem level = new EnchantItem
            {
                Level = left.Level + right.Level,
                Penalty = Math.Max(left.Penalty, right.Penalty) + 1,
                StepLevel = right.Level + Extensions.PenaltyToExperience(left.Penalty) + Extensions.PenaltyToExperience(right.Penalty)
            };
            level.HistoryLevel = level.StepLevel + left.HistoryLevel + right.HistoryLevel;
            level.HistoryExperience = Convert.ToInt64(Extensions.LevelToExperience(level.StepLevel)) + left.HistoryExperience + right.HistoryExperience;
            return level;
        }
    }
}
