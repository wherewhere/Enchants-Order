using EnchantsOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#if WINRT
using Windows.Foundation.Metadata;
#endif

namespace EnchantsOrder
{
    /// <summary>
    /// Root of EnchantsOrder.
    /// </summary>
    public static class Instance
    {
#if WINRT
        /// <summary>
        /// Ordering enchantments.
        /// </summary>
        /// <param name="wantedlist">Enchantments you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
        public static OrderingResults Ordering(this IEnumerable<Enchantment> wantedlist) => wantedlist.Ordering(0);
#endif

        /// <summary>
        /// Ordering enchantments.
        /// </summary>
        /// <param name="wantedlist">Enchantments you want to enchant.</param>
        /// <param name="inital_penalty">The penalty of your item which you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
        public static OrderingResults Ordering(this IEnumerable<Enchantment> wantedlist, int inital_penalty
#if !WINRT
            = 0
#endif
            ) => wantedlist.Cast<IEnchantment>().Ordering(inital_penalty);

#if WINRT
        /// <summary>
        /// Ordering enchantments.
        /// </summary>
        /// <param name="wantedlist">Enchantments you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
#if WINRT
        [DefaultOverload]
#endif
        public static OrderingResults Ordering(this IEnumerable<IEnchantment> wantedlist) => wantedlist.Ordering(0);
#endif

        /// <summary>
        /// Ordering enchantments.
        /// </summary>
        /// <param name="wantedlist">Enchantments you want to enchant.</param>
        /// <param name="inital_penalty">The penalty of your item which you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
#if WINRT
        [DefaultOverload]
#endif
        public static OrderingResults Ordering(this IEnumerable<IEnchantment> wantedlist, int inital_penalty
#if !WINRT
            = 0
#endif
            )
        {
            if (wantedlist == null || !wantedlist.Any())
            {
                throw new ArgumentNullException(nameof(wantedlist), "Cannot enchant: No enchantment given.");
            }

            // get the xp required and into be enchanted it
            List<IEnchantment> sortedlist = wantedlist.ToList();
            sortedlist.Sort((x, y) => y.CompareTo(x));
            List<long> numlist = wantedlist.Select((x) => x.Experience).ToList();

            List<object> priority_list = new();

            // generate base enchantment level list
            // i.e. add the sum of penalty level by item and merged books
            // also, count the max_step
            int total_enchantment = numlist.Count;
            List<int> max_step = EnchantLayer(total_enchantment, inital_penalty);

            List<IEnchantmentStep> ordering = new();

            int penalty = inital_penalty + max_step.Count;

            List<long>[] ordering_num = OrderEnchants(numlist, max_step, inital_penalty);
            List<EnchantItem> level_list = ComputeExperience(ordering_num);
            List<long> xp_list = GetExperienceList(level_list, inital_penalty);
            long xp_sum = GetExperience(level_list, inital_penalty);

            double xp_max = xp_list.Max();

            // penalty of merged books
            int enchantment_step = 0;
            foreach (List<long> element in ordering_num)
            {
                if (element == null) { continue; }
                enchantment_step++;
                ordering.Add(new EnchantmentStep(enchantment_step));
                // penalty for merge book

                // list steps with name
                foreach (long j in element)
                {
                    foreach (IEnchantment k in sortedlist)
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
            List<int> max_step = new();
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
                List<List<long>> ordering_num = new();

                foreach (long step in max_step)
                {
                    List<long> list = new();
                    for (int i = 1; i <= step; i++)
                    {
                        long tobe_enchanted = i % 2 == 1 ? nums.Max() : nums.Min();
                        list.Add(tobe_enchanted);
                        nums.Remove(tobe_enchanted);
                    }
                    ordering_num.Add(list);
                }

                for (int j = ordering_num.Count - 1; j > 0; j--)
                {
                    if (ordering_num[j].Count == ordering_num[j - 1].Count)
                    {
                        if (ordering_num[j].Sum() > ordering_num[j - 1].Sum())
                        {
#if !NET47_OR_GREATER && !NETCOREAPP
#pragma warning disable IDE0180 // 使用元组交换值
                            List<long> temp = ordering_num[j];
#pragma warning restore IDE0180 // 使用元组交换值
                            ordering_num[j] = ordering_num[j - 1];
                            ordering_num[j - 1] = temp;
#else
                            (ordering_num[j - 1], ordering_num[j]) = (ordering_num[j], ordering_num[j - 1]);
#endif
                        }
                    }
                }

                return ordering_num.ToArray();
            }

            List<long>[] MethodTwo()
            {
                List<long> nums = numlist.ToList();
                List<long>[] ordering_num = new List<long>[max_step.Count];
                while (nums.Any())
                {
                    List<double> temp_xp_list = new();

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
            EnchantItem item = new(0, inital_penalty);
            foreach (EnchantItem level in xplist)
            {
                item += level;
            }
            return item.HistoryExperience;
        }

        private static List<EnchantItem> ComputeExperience(IEnumerable<long>[] ordering_num)
        {
            List<EnchantItem> xplist = new();
            List<EnchantItem>[] step_list = ordering_num.Select((x) => x.Select((x) => new EnchantItem(x)).ToList()).ToArray();

            foreach (List<EnchantItem> step in step_list)
            {
                List<EnchantItem> temp = step;
                while (temp.Count > 1)
                {
                    List<EnchantItem> result = new();
                    EnchantItem item = new();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        EnchantItem num = temp[i];
                        int index = i + 1;
                        if (index % 2 == 1)
                        {
                            item = num;
                            if (index == temp.Count)
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

        private static List<long> GetExperienceList(IList<EnchantItem> xplist, int inital_penalty)
        {
            List<long> results = new();
            for (int index = 0; index < xplist.Count; index++)
            {
                EnchantItem level = xplist[index];
                EnchantItem item = new(0, index + inital_penalty);
                item += level;
                results.Add(item.StepLevel);
            }
            return results;
        }

        private static long GetExperience(IEnumerable<EnchantItem> xplist, int inital_penalty)
        {
            EnchantItem item = new(0, inital_penalty);
            foreach (EnchantItem level in xplist)
            {
                item += level;
            }
            return item.HistoryLevel;
        }
    }
}
