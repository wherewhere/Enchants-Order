using EnchantsOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
                throw new ArgumentNullException(nameof(wantedlist), "Cannot enchant: No enchantment given.");
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

            List<long>[] ordering_num = OrderEnchants(numlist, max_step, inital_penalty);
            List<EnchantItem> level_list = ComputeExperience(ordering_num);
            List<long> xp_list = GetExperienceList(level_list, inital_penalty);
            long xp_sum = GetExperience(level_list, inital_penalty);

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

        private static List<long>[] OrderEnchants(IEnumerable<long> numlist, IList<int> max_step, int inital_penalty)
        {
            List<long>[] MethodOne()
            {
                List<long> nums = numlist.ToList();
                List<List<long>> ordering_num = new List<List<long>>();
                foreach (long step in max_step)
                {
                    List<long> list = new List<long>();
                    for (int i = 1; i <= step; i++)
                    {
                        long tobe_enchanted = i % 2 == 1 ? nums.Max() : nums.Min();
                        list.Add(tobe_enchanted);
                        nums.Remove(tobe_enchanted);
                    }
                    ordering_num.Add(list);
                }
                return ordering_num.ToArray();
            }

            List<long>[] MethodTwo()
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

            List<long>[] list1 = MethodOne();
            List<long>[] list2 = MethodTwo();

            long value1 = GetExperience(list1, inital_penalty);
            long value2 = GetExperience(list2, inital_penalty);

            return value1 < value2 ? list1 : list2;
        }

        private static long GetExperience(IEnumerable<long>[] ordering_num, int inital_penalty)
        {
            List<EnchantItem> xplist = ComputeExperience(ordering_num);
            EnchantItem item = new EnchantItem(0, inital_penalty);
            foreach (EnchantItem level in xplist)
            {
                item += level;
            }
            return item.HistoryExperience;
        }

        private static List<EnchantItem> ComputeExperience(IEnumerable<long>[] ordering_num)
        {
            List<EnchantItem> xplist = new List<EnchantItem>();
            List<EnchantItem>[] step_list = ordering_num.Select((x) => x.Select((x) => new EnchantItem(x)).ToList()).ToArray();

            foreach (List<EnchantItem> step in step_list)
            {
                List<EnchantItem> temp = step;
                while (temp.Count > 1)
                {
                    List<EnchantItem> result = new List<EnchantItem>();
                    EnchantItem item = new EnchantItem();
                    foreach (EnchantItem num in temp)
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

        private static List<long> GetExperienceList(List<EnchantItem> xplist, int inital_penalty)
        {
            List<long> results = new List<long>();
            foreach (EnchantItem level in xplist)
            {
                int index = xplist.IndexOf(level);
                EnchantItem item = new EnchantItem(0, index + inital_penalty);
                item += level;
                results.Add(item.StepLevel);
            }
            return results;
        }

        private static long GetExperience(List<EnchantItem> xplist, int inital_penalty)
        {
            EnchantItem item = new EnchantItem(0, inital_penalty);
            foreach (EnchantItem level in xplist)
            {
                item += level;
            }
            return item.HistoryLevel;
        }
    }
}
