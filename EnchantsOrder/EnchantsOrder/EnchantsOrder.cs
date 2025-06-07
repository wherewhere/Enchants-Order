using EnchantsOrder.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

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
        /// <summary>
        /// Ordering the enchantments.
        /// </summary>
        /// <param name="wantedList">Enchantments you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
#if WINRT
        [Overload("Ordering")]
#else
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        [MethodImpl((MethodImplOptions)0x100)]
        public static OrderingResults Ordering(this IEnumerable<IEnchantment> wantedList) => wantedList.Ordering(0);

        /// <summary>
        /// Ordering the enchantments with initial penalty.
        /// </summary>
        /// <param name="wantedList">Enchantments you want to enchant.</param>
        /// <param name="initialPenalty">The penalty of your item which you want to enchant.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
#if WINRT
        [DefaultOverload]
        [Overload("OrderingWithPenalty")]
#else
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        [MethodImpl((MethodImplOptions)0x100)]
        public static OrderingResults Ordering(this IEnumerable<IEnchantment> wantedList, int initialPenalty) => wantedList.Ordering(initialPenalty);

        /// <summary>
        /// Ordering the enchantments with initial penalty.
        /// </summary>
        /// <typeparam name="T">The type of enchantment.</typeparam>
        /// <param name="wantedList">Enchantments you want to enchant.</param>
        /// <param name="initialPenalty">The penalty of your item which you want to enchant, default is <see langword="0"/>.</param>
        /// <returns>The step list and some eigenvalue of this result.</returns>
        /// <exception cref="ArgumentNullException">The list of enchantments you want to enchant is empty or null.</exception>
        [OverloadResolutionPriority(1)]
#if WINRT
        internal
#else
        public
#endif
            static OrderingResults Ordering<T>(this IEnumerable<T> wantedList, int initialPenalty = 0) where T : IEnchantment
        {
            if (wantedList == null || !wantedList.Any())
            {
                throw new ArgumentNullException(nameof(wantedList), "Cannot enchant: No enchantment given.");
            }

            // get the xp required and into be enchanted it
            List<T> sortedList = [.. wantedList];
            sortedList.Sort((x, y) => y.CompareTo(x));
            List<long> numList = [.. wantedList.Select(x => x.Experience)];

            // generate base enchantment level list
            // i.e. add the sum of penalty level by item and merged books
            // also, count the max step
            int totalEnchantment = numList.Count;
            List<int> maxStep = EnchantLayer(totalEnchantment, initialPenalty);

            int penalty = initialPenalty + maxStep.Count;

            List<long>[] orderingSteps = OrderEnchants(numList, maxStep, initialPenalty);
            List<EnchantItem> levelList = ComputeExperience(orderingSteps);
            List<long> xpList = GetExperienceList(levelList, initialPenalty);
            long xpSum = GetExperience(levelList, initialPenalty);

            double xpMax = xpList.Max();
            List<EnchantmentStep> ordering = new(orderingSteps.Length);

            // penalty of merged books
            int index = 0;
            foreach (List<long> step in orderingSteps)
            {
                if (step == null) { continue; }
                List<IEnchantment> enchantments = new(step.Count);
                ordering.Add(new EnchantmentStep(++index, enchantments));
                // penalty for merge book

                // list steps with name
                foreach (long xp in step)
                {
                    foreach (T enchantment in sortedList)
                    {
                        if (enchantment.Experience == xp)
                        {
                            enchantments.Add(enchantment);
                            sortedList.Remove(enchantment);
                            break;
                        }
                    }
                }
            }
            ordering.TrimExcess();

            return new OrderingResults(penalty, xpMax, xpSum, ordering);
        }

        /// <summary>
        /// Distributes a total enchantment value into layers based on an incremental step penalty.
        /// </summary>
        /// <param name="totalEnchantment">The total amount of enchantment to be distributed across layers. Must be a non-negative integer.</param>
        /// <param name="initialPenalty">The initial penalty applied to the step calculation. Must be a non-negative integer.</param>
        /// <returns>A list of integers where each element represents the enchantment value for a specific layer. The sum of all
        /// elements equals <paramref name="totalEnchantment"/>.</returns>
        private static List<int> EnchantLayer(int totalEnchantment, int initialPenalty)
        {
            List<int> maxStep = [];
            int step = 0 + initialPenalty;
            while (totalEnchantment > 0)
            {
                int enchantmentCount = Math.Min(totalEnchantment, Convert.ToInt32(Math.Pow(2, step++)));
                maxStep.Add(enchantmentCount);
                totalEnchantment -= enchantmentCount;
            }
            return maxStep;
        }

        /// <summary>
        /// Determines the optimal ordering of enchantments based on the provided parameters.
        /// </summary>
        /// <param name="numList">A collection of numbers representing the items to be enchanted.</param>
        /// <param name="maxStep">A list of integers specifying the maximum number of enchantments allowed at each step.</param>
        /// <param name="initialPenalty">The initial penalty value used to calculate the total experience cost.</param>
        /// <returns>An array of lists, where each list represents the items enchanted at a specific step.  The returned ordering
        /// minimizes the total experience cost based on the given parameters.</returns>
        private static List<long>[] OrderEnchants(IEnumerable<long> numList, List<int> maxStep, int initialPenalty)
        {
            List<long>[] FirstMethod()
            {
                List<long> tempList = [.. numList];
                List<long>[] orderingSteps = new List<long>[maxStep.Count];

                for (int i = 0; i < maxStep.Count; i++)
                {
                    long step = maxStep[i];
                    List<long> list = [];
                    for (int j = 1; j <= step; j++)
                    {
                        long ToBeEnchanted = j % 2 == 1 ? tempList.Max() : tempList.Min();
                        list.Add(ToBeEnchanted);
                        tempList.Remove(ToBeEnchanted);
                    }
                    orderingSteps[i] = list;
                }

                for (int j = orderingSteps.Length - 1; j > 0; j--)
                {
                    if (orderingSteps[j].Count == orderingSteps[j - 1].Count
                        && orderingSteps[j].Sum() > orderingSteps[j - 1].Sum())
                    {
                        (orderingSteps[j - 1], orderingSteps[j]) = (orderingSteps[j], orderingSteps[j - 1]);
                    }
                }

                return orderingSteps;
            }

            List<long>[] SecondMethod()
            {
                List<long> tempList = [.. numList];
                List<long>[] orderingSteps = new List<long>[maxStep.Count];
                while (tempList.Count > 0)
                {
                    List<int> xpList = [];

                    foreach (int i in maxStep)
                    {
                        xpList.Add(i == 0 ? int.MaxValue : i);
                    }

                    int step = xpList.IndexOf(xpList.Min());
                    orderingSteps[step] ??= [];
                    long ToBeEnchanted = tempList.Max();

                    orderingSteps[step].Add(ToBeEnchanted);
                    maxStep[step] -= 1;

                    tempList.Remove(ToBeEnchanted);
                }
                return orderingSteps;
            }

            List<long>[] list1 = FirstMethod();
            List<long>[] list2 = SecondMethod();

            long value1 = GetExperience(list1, initialPenalty);
            long value2 = GetExperience(list2, initialPenalty);

            return value1 < value2 ? list1 : list2;
        }

        /// <summary>
        /// Calculates the total experience based on a sequence of ordered values and an initial penalty.
        /// </summary>
        /// <param name="orderingNum">A collection of collections representing the ordered numeric values used to compute experience.</param>
        /// <param name="initialPenalty">The initial penalty value to be applied to the experience calculation.</param>
        /// <returns>The total calculated experience as a long integer.</returns>
        private static long GetExperience(List<long>[] orderingNum, int initialPenalty)
        {
            List<EnchantItem> xpList = ComputeExperience(orderingNum);
            EnchantItem item = new(0, initialPenalty);
            foreach (EnchantItem level in xpList)
            {
                item += level;
            }
            return item.HistoryExperience;
        }

        /// <summary>
        /// Computes a list of experience values by processing a series of ordered steps.
        /// </summary>
        /// <param name="orderingSteps">A collection of collections, where each inner collection represents a sequence of numeric values to be
        /// processed into experience items.</param>
        /// <returns>A list of <see cref="EnchantItem"/> objects, where each object represents the final computed experience
        /// value for a corresponding sequence of steps.</returns>
        private static List<EnchantItem> ComputeExperience(List<long>[] orderingSteps)
        {
            List<EnchantItem> xpList = [];
            IEnumerable<List<EnchantItem>> stepList = orderingSteps.Select<IEnumerable<long>, List<EnchantItem>>(x => [.. x.Select(x => new EnchantItem(x))]);

            foreach (List<EnchantItem> step in stepList)
            {
                List<EnchantItem> temp = step;
                while (temp.Count > 1)
                {
                    List<EnchantItem> result = [];
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
                xpList.Add(temp[0]);
            }

            return xpList;
        }

        /// <summary>
        /// Calculates a list of experience levels based on the provided enchantment items and an initial penalty.
        /// </summary>
        /// <param name="xpList">A list of <see cref="EnchantItem"/> objects representing the base experience levels.</param>
        /// <param name="initialPenalty">An integer value representing the initial penalty to apply to the calculation.</param>
        /// <returns>A list of long integers where each value represents the calculated experience level for the corresponding
        /// enchantment item.</returns>
        private static List<long> GetExperienceList(List<EnchantItem> xpList, int initialPenalty)
        {
            List<long> results = new(xpList.Count);
            for (int index = 0; index < xpList.Count; index++)
            {
                EnchantItem level = xpList[index];
                EnchantItem item = new(0, index + initialPenalty);
                item += level;
                results.Add(item.StepLevel);
            }
            return results;
        }

        /// <summary>
        /// Calculates the total experience level after applying a series of enchantment levels and an initial penalty.
        /// </summary>
        /// <param name="xpList">A collection of <see cref="EnchantItem"/> objects representing the enchantment levels to be applied.</param>
        /// <param name="initialPenalty">The initial penalty to be applied to the experience level.</param>
        /// <returns>The total experience level after applying all enchantment levels and the initial penalty.</returns>
        private static long GetExperience(List<EnchantItem> xpList, int initialPenalty)
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
