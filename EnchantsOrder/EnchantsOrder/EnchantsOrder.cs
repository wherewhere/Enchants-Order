using EnchantsOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace EnchantsOrder
{
    public static class Instance
    {
        internal static short max_penalty = 6;
        internal static short max_experience = 39;

        public static OrderingResults Ordering(this IEnumerable<Enchantment> wantedlist, int inital_penalty = 0)
        {
            if (wantedlist == null || !wantedlist.Any())
            {
                throw new ArgumentNullException(nameof(wantedlist), "Cannot enchant: no enchantment given");
            }

            //get the xp required and in（tobe_enchanted it
            List<Enchantment> sortedlist = wantedlist.ToList();
            sortedlist.Sort((x, y) => y.CompareTo(x));
            List<long> numlist = wantedlist.Select((x) => x.Experience).ToList();

            List<object> priority_list = new List<object>();

            // generate base enchantment level list
            // i.e. add the sum of penalty level by item and merged books
            // also, count the max_step
            int total_enchantment = numlist.Count;
            List<int> max_step = EnchantLayer(total_enchantment, inital_penalty);

            List<EnchantmentStep> ordering = new List<EnchantmentStep>();

            TooExpensiveException exception = null;
            int penalty = inital_penalty + max_step.Count;
            if (penalty > max_penalty)
            {
                exception = new TooExpensiveException(TooExpensiveReason.Penalty);
            }

            List<long>[] ordering_num = OrderEnchants(numlist, max_step);
            var level_list = ComputeExperience(ordering_num);
            var xp_list = GetExperienceList(level_list, inital_penalty);
            var xp_sum = GetExperience(level_list, inital_penalty);

            double xp_max = xp_list.Max();
            if (xp_max > max_experience)
            {
                exception = new TooExpensiveException(TooExpensiveReason.Experience);
            }

            //penalty of merged books
            int enchantment_step = 0;
            foreach (List<long> element in ordering_num)
            {
                if (element == null) { continue; }
                enchantment_step++;
                ordering.Add(new EnchantmentStep(enchantment_step));
                //penalty for merge book

                //list steps with name
                foreach (long j in element)
                {
                    foreach (Enchantment k in sortedlist)
                    {
                        if (k.Experience == j)
                        {
                            ordering[enchantment_step - 1].Add(k);
                            sortedlist.Remove(k);
                            break;
                        }
                    }
                }
            }

            return new OrderingResults(ordering, penalty, xp_max, xp_sum);
        }

        private static List<int> EnchantLayer(int total_enchantment, int inital_penalty)
        {
            List<int> max_step = new List<int>();
            int step = 0 + inital_penalty;
            while (total_enchantment > 0)
            {
                int num_of_enchantment = Math.Min(total_enchantment, Convert.ToInt32(Math.Pow(2, step++)));
                max_step.Add(num_of_enchantment);
                total_enchantment -= num_of_enchantment;
            }
            return max_step;
        }

        private static List<long>[] OrderEnchants(IEnumerable<long> numlist, IList<int> max_step)
        {
            List<long> nums = numlist.ToList();
            List<long>[] ordering_num = new List<long>[max_step.Count];

            while (nums.Any())
            {
                List<double> temp_xp_list = new List<double>();

                foreach (double i in max_step)
                {
                    temp_xp_list.Add(i == 0 ? 1000 : i);
                }

                int step = temp_xp_list.IndexOf(temp_xp_list.Min());
                ordering_num[step] ??= new List<long>();
                long tobe_enchanted = nums.Max();

                ordering_num[step].Add(tobe_enchanted);
                max_step[step] -= 1;

                nums.Remove(tobe_enchanted);
            }

            return ordering_num;
        }

        private static List<Level> ComputeExperience(IEnumerable<long>[] ordering_num)
        {
            List<Level> xplist = new List<Level>();
            List<Level>[] step_list = ordering_num.Select((x) => x.Select((x) => new Level(x)).ToList()).ToArray();

            foreach(List<Level> step in step_list)
            {
                List<Level> temp = step;
                while (temp.Count > 1)
                {
                    List<Level> result = new List<Level>();
                    Level item = new Level();
                    foreach (var num in temp)
                    {
                        int index = temp.IndexOf(num) + 1;
                        if (index % 2 == 1)
                        {
                            item = num;
                            if (index == step.Count)
                            {
                                result.Add(item);
                            }
                        }
                        else
                        {
                            item += num;
                            result.Add(item);
                        }
                    }
                    temp = result;
                }
                xplist.Add(temp.FirstOrDefault());
            }

            return xplist;
        }

        private static List<long> GetExperienceList(List<Level> xplist, int inital_penalty)
        {
            List<long> results = new List<long>();
            foreach (Level level in xplist)
            {
                int index = xplist.IndexOf(level);
                Level item = new Level(0, index + inital_penalty);
                item += level;
                results.Add(item.StepExperience);
            }
            return results;
        }

        private static long GetExperience(List<Level> xplist, int inital_penalty)
        {
            Level item = new Level(0, inital_penalty);
            foreach (Level level in xplist)
            {
                item += level;
            }
            return item.HistoryExperience;
        }
    }
}
