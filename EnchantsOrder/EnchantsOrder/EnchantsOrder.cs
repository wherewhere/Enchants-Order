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
    /// The tool to order the enchantments.
    /// </summary>
    public static class EnchantsOrder
    {
#if WINRT
        /// <summary>
        /// Ordering the enchantments.
        /// </summary>
        /// <param name="wantedList">Enchantments you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
        public static OrderingResults Ordering(this IEnumerable<Enchantment> wantedList) => wantedList.Ordering(0);
#endif

        /// <summary>
        /// Ordering the enchantments.
        /// </summary>
        /// <param name="wantedList">Enchantments you want to enchant.</param>
        /// <param name="initialPenalty">The penalty of your item which you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
        public static OrderingResults Ordering(this IEnumerable<Enchantment> wantedList, int initialPenalty
#if !WINRT
            = 0
#endif
            ) => wantedList.Cast<IEnchantment>().Ordering(initialPenalty);

#if WINRT
        /// <summary>
        /// Ordering the enchantments.
        /// </summary>
        /// <param name="wantedList">Enchantments you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
#if WINRT
        [DefaultOverload]
#endif
        public static OrderingResults Ordering(this IEnumerable<IEnchantment> wantedList) => wantedList.Ordering(0);
#endif

        /// <summary>
        /// Ordering the enchantments.
        /// </summary>
        /// <param name="wantedList">Enchantments you want to enchant.</param>
        /// <param name="initialPenalty">The penalty of your item which you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
#if WINRT
        [DefaultOverload]
#endif
        public static OrderingResults Ordering(this IEnumerable<IEnchantment> wantedList, int initialPenalty
#if !WINRT
            = 0
#endif
            )
        {
            if (wantedList == null || !wantedList.Any())
            {
                throw new ArgumentNullException(nameof(wantedList), "Cannot enchant: No enchantment given.");
            }

            // get the xp required and into be enchanted it
            List<IEnchantment> sortedList = wantedList.ToList();
            sortedList.Sort((x, y) => y.CompareTo(x));
            IEnumerable<long> numList = wantedList.Select((x) => x.Experience);

            List<object> priorityList = new();

            // generate base enchantment level list
            // i.e. add the sum of penalty level by item and merged books
            // also, count the max step
            int totalEnchantment = numList.Count();
            List<int> maxStep = EnchantLayer(totalEnchantment, initialPenalty);

            List<IEnchantmentStep> ordering = new();

            int penalty = initialPenalty + maxStep.Count;

            IList<List<long>> orderingSteps = OrderEnchants(numList, maxStep, initialPenalty);
            List<EnchantItem> levelList = ComputeExperience(orderingSteps);
            List<long> xpList = GetExperienceList(levelList, initialPenalty);
            long xpSum = GetExperience(levelList, initialPenalty);

            double xpMax = xpList.Max();

            // penalty of merged books
            int enchantmentStep = 0;
            foreach (List<long> step in orderingSteps)
            {
                if (step == null) { continue; }
                enchantmentStep++;
                ordering.Add(new EnchantmentStep(enchantmentStep));
                // penalty for merge book

                // list steps with name
                foreach (long xp in step)
                {
                    foreach (IEnchantment enchantment in sortedList)
                    {
                        if (enchantment.Experience == xp)
                        {
                            ordering[enchantmentStep - 1].Add(enchantment);
                            sortedList.Remove(enchantment);
                            break;
                        }
                    }
                }
            }

            return new OrderingResults(ordering, penalty, xpMax, xpSum);
        }

        private static List<int> EnchantLayer(int totalEnchantment, int initialPenalty)
        {
            List<int> maxStep = new();
            int step = 0 + initialPenalty;
            while (totalEnchantment > 0)
            {
                int enchantmentCount = Math.Min(totalEnchantment, Convert.ToInt32(Math.Pow(2, step++)));
                maxStep.Add(enchantmentCount);
                totalEnchantment -= enchantmentCount;
            }
            return maxStep;
        }

        private static IList<List<long>> OrderEnchants(IEnumerable<long> numList, IList<int> maxStep, int initialPenalty)
        {
            List<List<long>> FirstMethod()
            {
                List<long> tempList = numList.ToList();
                List<List<long>> orderingSteps = new(maxStep.Count);

                foreach (long step in maxStep)
                {
                    List<long> list = new();
                    for (int i = 1; i <= step; i++)
                    {
                        long ToBeEnchanted = i % 2 == 1 ? tempList.Max() : tempList.Min();
                        list.Add(ToBeEnchanted);
                        tempList.Remove(ToBeEnchanted);
                    }
                    orderingSteps.Add(list);
                }

                for (int j = orderingSteps.Count - 1; j > 0; j--)
                {
                    if (orderingSteps[j].Count == orderingSteps[j - 1].Count)
                    {
                        if (orderingSteps[j].Sum() > orderingSteps[j - 1].Sum())
                        {
#if !NET47_OR_GREATER && !NETCOREAPP
                            List<long> temp = orderingSteps[j];
                            orderingSteps[j] = orderingSteps[j - 1];
                            orderingSteps[j - 1] = temp;
#else
                            (orderingSteps[j - 1], orderingSteps[j]) = (orderingSteps[j], orderingSteps[j - 1]);
#endif
                        }
                    }
                }

                return orderingSteps;
            }

            List<long>[] SecondMethod()
            {
                List<long> tempList = numList.ToList();
                List<long>[] orderingSteps = new List<long>[maxStep.Count];
                while (tempList.Any())
                {
                    List<double> xpList = new();

                    foreach (double i in maxStep)
                    {
                        xpList.Add(i == 0 ? 1000 : i);
                    }

                    int step = xpList.IndexOf(xpList.Min());
                    orderingSteps[step] ??= new List<long>();
                    long ToBeEnchanted = tempList.Max();

                    orderingSteps[step].Add(ToBeEnchanted);
                    maxStep[step] -= 1;

                    tempList.Remove(ToBeEnchanted);
                }
                return orderingSteps;
            }

            List<List<long>> list1 = FirstMethod();
            List<long>[] list2 = SecondMethod();

            long value1 = GetExperience(list1, initialPenalty);
            long value2 = GetExperience(list2, initialPenalty);

            return value1 < value2 ? list1 : list2;
        }

        private static long GetExperience(IList<List<long>> orderingNum, int initialPenalty)
        {
            List<EnchantItem> xpList = ComputeExperience(orderingNum);
            EnchantItem item = new(0, initialPenalty);
            foreach (EnchantItem level in xpList)
            {
                item += level;
            }
            return item.HistoryExperience;
        }

        private static List<EnchantItem> ComputeExperience(IList<List<long>> orderingSteps)
        {
            List<EnchantItem> xpList = new();
            IEnumerable<List<EnchantItem>> stepList = orderingSteps.Select((x) => x.Select((x) => new EnchantItem(x)).ToList());

            foreach (List<EnchantItem> step in stepList)
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
                xpList.Add(temp.FirstOrDefault());
            }

            return xpList;
        }

        private static List<long> GetExperienceList(IList<EnchantItem> xpList, int initialPenalty)
        {
            List<long> results = new();
            for (int index = 0; index < xpList.Count; index++)
            {
                EnchantItem level = xpList[index];
                EnchantItem item = new(0, index + initialPenalty);
                item += level;
                results.Add(item.StepLevel);
            }
            return results;
        }

        private static long GetExperience(IEnumerable<EnchantItem> xpList, int initialPenalty)
        {
            EnchantItem item = new(0, initialPenalty);
            foreach (EnchantItem level in xpList)
            {
                item += level;
            }
            return item.HistoryLevel;
        }
    }
}
