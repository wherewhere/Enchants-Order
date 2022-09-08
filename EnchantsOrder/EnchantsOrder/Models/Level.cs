using System;

namespace EnchantsOrder.Models
{
    public class Level
    {
        public long Num { get; set; }
        public long Penalty { get; set; }
        public long StepExperience { get; private set; }
        public long HistoryExperience { get; set; }

        public Level()
        {
        }

        public Level(long num, long penalty = 0, long historyExperience = 0)
        {
            Num = num;
            Penalty = penalty;
            HistoryExperience = historyExperience;
        }

        public static Level operator +(Level left, Level right)
        {
            var level = new Level();
            level.Num = left.Num + right.Num;
            level.Penalty = Math.Max(left.Penalty, right.Penalty) + 1;
            level.StepExperience = right.Num + PenaltyToExperience(left.Penalty) + PenaltyToExperience(right.Penalty);
            level.HistoryExperience = right.Num + PenaltyToExperience(left.Penalty) + PenaltyToExperience(right.Penalty) + left.HistoryExperience + right.HistoryExperience;
            return level;
        }

        private static long PenaltyToExperience(long penalty)
        {
            try
            {
                var a = Convert.ToInt64(Math.Pow(2, penalty));
            }
            catch
            {

            }
            return Math.Max(0, Convert.ToInt64(Math.Pow(2, penalty)) - 1);
        }
    }
}
