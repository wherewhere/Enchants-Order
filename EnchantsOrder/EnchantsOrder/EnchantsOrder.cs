using EnchantsOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#if WINRT
using Windows.Foundation.Metadata;
#else
using System.ComponentModel;
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
        [Overload(nameof(Ordering))]
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
            if (wantedList?.Any() != true)
            {
                throw new ArgumentNullException(nameof(wantedList), "Cannot enchant: No enchantment given.");
            }

            // get the xp required and into be enchanted it
            List<T> sortedList = [.. wantedList];
            sortedList.Sort((x, y) => y.CompareTo(x));
            List<long> numList = [.. sortedList.Select(x => x.Experience)];
            numList.Reverse();

            // generate base enchantment level list
            // i.e. add the sum of penalty level by item and merged books
            // also, count the max step
            int totalEnchantment = numList.Count;
            List<int> maxSteps = EnchantLayer(totalEnchantment, initialPenalty);

            int penalty = initialPenalty + maxSteps.Count;

            List<long>[] orderingSteps = OrderEnchants(numList, maxSteps, initialPenalty);
            EnchantUnit[] levelList = orderingSteps.ComputeExperience();
            long[] xpList = levelList.GetLevelList(initialPenalty);
            long xpSum = levelList.GetHistoryLevel(initialPenalty);

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
                    for (int i = 0; i < levelList.Length; i++)
                    {
                        T enchantment = sortedList[i];
                        if (enchantment.Experience == xp)
                        {
                            enchantments.Add(enchantment);
                            sortedList.RemoveAt(i);
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
            List<int> maxSteps = [];
            int step = initialPenalty;
            // Distribute the remaining enchantments into layers whose capacities double each step.
            // Each layer can hold up to 2^step enchantments; stop when all enchantments are allocated.
            while (totalEnchantment > 0)
            {
                // Determine how many enchantments this layer will take (capped by remaining total)
                int enchantmentCount = Math.Min(totalEnchantment, Convert.ToInt32(Math.Pow(2, step++)));
                maxSteps.Add(enchantmentCount);
                totalEnchantment -= enchantmentCount;
            }
            return maxSteps;
        }

        /// <summary>
        /// Distributes the elements of the specified list into groups based on the provided step limits, ordering the
        /// groups to balance their sums.
        /// </summary>
        /// <param name="numList">The sequence of numbers to be distributed into groups.</param>
        /// <param name="maxSteps">A list specifying the maximum number of elements that each group can contain. Each value represents the
        /// capacity for the corresponding group and must be non-negative.</param>
        /// <param name="initialPenalty">The initial penalty value used in the sum calculation for ordering the steps. This value is added to the
        /// step index during computation.</param>
        /// <returns>An array of lists, where each list contains the numbers assigned to a group. The order of the array
        /// corresponds to the order of the step limits provided in <paramref name="maxSteps"/>.</returns>
        private static List<long>[] OrderWithGreedy(List<long> numList, List<int> maxSteps, int initialPenalty)
        {
            // Create temporary copies to avoid modifying the input collections
            List<int> tempSteps = [.. maxSteps];
            List<long> tempList = [.. numList];
            List<long>[] orderingSteps = new List<long>[tempSteps.Count];

            // Sort the list in ascending order for greedy selection algorithm
            // The algorithm processes from highest to lowest value
            tempList.Sort();

            // Greedy algorithm: iterate from highest to lowest value
            // Assign each enchantment to the step with the most available capacity
            for (int i = tempList.Count; i > 0;)
            {
                // Find the step with the minimum remaining capacity (greedy choice)
                // This ensures we fill steps evenly and minimize experience cost
                int step = tempSteps.IndexOf(tempSteps.Min());
                orderingSteps[step] ??= [];
                long toBeEnchanted = tempList[--i];

                // Add the enchantment to the selected step and decrease its capacity
                orderingSteps[step].Add(toBeEnchanted);
                tempSteps[step]--;

                if (tempSteps[step] <= 0)
                {
                    tempSteps[step] = int.MaxValue;
                }
            }

            // Optimize the final ordering by sorting adjacent steps with equal counts
            return orderingSteps.OrderBySum(initialPenalty);
        }

        /// <summary>
        /// Reorders the specified array of steps based on a custom sum calculation that incorporates an initial penalty
        /// value.
        /// </summary>
        /// <param name="orderingSteps">An array of lists, where each list represents a step containing long values to be ordered.</param>
        /// <param name="initialPenalty">The initial penalty value used in the sum calculation for ordering the steps. This value is added to the
        /// step index during computation.</param>
        /// <returns>The reordered array of steps, sorted according to the custom sum logic. The returned array is the same
        /// instance as the input array, with elements potentially rearranged.</returns>
        private static List<long>[] OrderBySum(this List<long>[] orderingSteps, int initialPenalty)
        {
            // Iterate backward through the ordered steps to compare each step with its predecessor.
            // The goal is to swap adjacent steps when the later step has fewer elements but incurs
            // a higher marginal experience cost, improving overall efficiency.
            for (int i = orderingSteps.Length - 1; i > 0; i--)
            {
                List<long> currentStep = orderingSteps[i];
                List<long> previousStep = orderingSteps[i - 1];

                // Only evaluate swaps when the later step has fewer enchantments than the previous one.
                if (currentStep.Count < previousStep.Count)
                {
                    int index = initialPenalty + i;

                    // Compute the experience level if current/previous steps are applied at their positions.
                    long currentSum = currentStep.ComputeExperience().EnchantItem(index).StepLevel;
                    long previousSum = previousStep.ComputeExperience().EnchantItem(index - 1).StepLevel;

                    // Swap when moving the larger-cost group later would reduce the total cost more than its size.
                    if (currentSum - previousSum > previousStep.Count)
                    {
                        orderingSteps[i] = previousStep;
                        orderingSteps[i - 1] = currentStep;
                    }
                }
            }
            return orderingSteps;
        }

        /// <summary>
        /// Distributes the elements of the input sequence into multiple steps using an alternating high-low selection
        /// pattern, based on the specified maximum counts for each step.
        /// </summary>
        /// <param name="numList">The sequence of long values to be distributed across steps. The order of elements in the input is not
        /// preserved.</param>
        /// <param name="maxSteps">A list of integers specifying the number of elements to assign to each step. Each value determines how many
        /// items are selected for the corresponding step.</param>
        /// <returns>An array of lists, where each list contains the long values assigned to a step. The array length matches the
        /// number of steps specified in <paramref name="maxSteps"/>.</returns>
        private static List<long>[] OrderWithAlternating(List<long> numList, List<int> maxSteps)
        {
            // Create a sorted copy of the input list and initialize the output array
            // Each element in orderingSteps will contain enchantments for that step
            List<long> tempList = [.. numList];
            List<long>[] orderingSteps = new List<long>[maxSteps.Count];

            // Sort the list in ascending order for alternating selection
            tempList.Sort();
            
            // Distribute enchantments across steps using an alternating pattern
            // This method alternates between selecting the highest and lowest values
            for (int i = 0; i < maxSteps.Count; i++)
            {
                long step = maxSteps[i];
                List<long> list = [];
                
                // For each enchantment slot in this step
                for (int j = 1; j <= step; j++)
                {
                    // Odd positions: take from end (highest), Even positions: take from start (lowest)
                    int index = (j & 1) == 1 ? tempList.Count - 1 : 0;
                    long toBeEnchanted = tempList[index];
                    list.Add(toBeEnchanted);
                    tempList.RemoveAt(index);
                }
                orderingSteps[i] = list;
            }

            return orderingSteps.OrderBySum();
        }

        /// <summary>
        /// Distributes the sorted elements of the specified sequence into multiple lists according to the provided step
        /// counts, returning the lists ordered by the sum of their elements.
        /// </summary>
        /// <param name="numList">The sequence of long integers to be distributed. The elements are sorted in ascending order before
        /// distribution.</param>
        /// <param name="maxSteps">A list of integers specifying the maximum number of elements to assign to each resulting list. Each value
        /// determines the capacity for the corresponding list.</param>
        /// <returns>An array of lists of long integers, where each list contains up to the specified number of elements from the
        /// sorted input sequence. The resulting array is ordered by the sum of elements in each list.</returns>
        private static List<long>[] OrderWithSequential(List<long> numList, List<int> maxSteps)
        {
            List<int> tempSteps = [.. maxSteps];
            List<long> tempList = [.. numList];
            List<long>[] orderingSteps = new List<long>[tempSteps.Count];

            tempList.Sort();

            for (int i = tempList.Count; i > 0;)
            {
                for (int j = 0; j < tempSteps.Count; j++)
                {
                    if (tempSteps[j] > 0)
                    {
                        orderingSteps[j] ??= [];
                        orderingSteps[j].Add(tempList[--i]);
                        tempSteps[j]--;
                    }
                }
            }

            return orderingSteps;
        }

        /// <summary>
        /// Reorders the specified array of steps so that, among adjacent steps with equal counts, the step with the
        /// lower sum precedes the one with the higher sum.
        /// </summary>
        /// <param name="orderingSteps">An array of lists of long integers representing the steps to be ordered. Each list corresponds to a step,
        /// and the array defines their initial order.</param>
        /// <returns>The reordered array of steps, with adjacent steps of equal count arranged so that steps with lower sums come first.</returns>
        private static List<long>[] OrderBySum(this List<long>[] orderingSteps)
        {
            // Optimize by swapping adjacent steps if they have equal counts
            // and the later step has a higher sum (to minimize experience cost)
            for (int i = orderingSteps.Length - 1; i > 0; i--)
            {
                List<long> currentStep = orderingSteps[i];
                List<long> previousStep = orderingSteps[i - 1];

                // Only consider swapping steps with identical element counts
                // Different counts indicate different layers in the enchantment tree
                if (currentStep.Count == previousStep.Count)
                {
                    long currentSum = 0;
                    long previousSum = 0;

                    // Calculate the total experience value for each step
                    for (int j = 0; j < currentStep.Count; j++)
                    {
                        currentSum += currentStep[j];
                        previousSum += previousStep[j];
                    }

                    // Swap if the later step has a higher sum
                    // This places the cheaper operation earlier in the sequence
                    if (currentSum > previousSum)
                    {
                        orderingSteps[i] = previousStep;
                        orderingSteps[i - 1] = currentStep;
                    }
                }
            }
            return orderingSteps;
        }

        /// <summary>
        /// Determines the optimal ordering of enchantments based on the provided parameters.
        /// </summary>
        /// <param name="numList">A collection of numbers representing the items to be enchanted.</param>
        /// <param name="maxSteps">A list of integers specifying the maximum number of enchantments allowed at each step.</param>
        /// <param name="initialPenalty">The initial penalty value used to calculate the total experience cost.</param>
        /// <returns>An array of lists, where each list represents the items enchanted at a specific step.  The returned ordering
        /// minimizes the total experience cost based on the given parameters.</returns>
        private static List<long>[] OrderEnchants(List<long> numList, List<int> maxSteps, int initialPenalty)
        {
            List<long>[] list1 = OrderWithGreedy(numList, maxSteps, initialPenalty);
            List<long>[] list2 = OrderWithSequential(numList, maxSteps);
            List<long>[] list3 = OrderWithAlternating(numList, maxSteps);

            long value1 = GetExperience(list1, initialPenalty);
            long value2 = GetExperience(list2, initialPenalty);
            long value3 = GetExperience(list3, initialPenalty);

            return value1 <= value2
                ? (value1 <= value3 ? list1 : list3)
                : (value2 <= value3 ? list2 : list3);
        }

        /// <summary>
        /// Calculates the total experience based on a sequence of ordered values and an initial penalty.
        /// </summary>
        /// <param name="orderingNum">A collection of collections representing the ordered numeric values used to compute experience.</param>
        /// <param name="initialPenalty">The initial penalty value to be applied to the experience calculation.</param>
        /// <returns>The total calculated experience as a long integer.</returns>
        private static long GetExperience(List<long>[] orderingNum, int initialPenalty)
        {
            EnchantUnit[] xpList = orderingNum.ComputeExperience();
            EnchantUnit item = new(0, initialPenalty);
            foreach (EnchantUnit level in xpList)
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
        /// <returns>A list of <see cref="EnchantUnit"/> objects, where each object represents the final computed experience
        /// value for a corresponding sequence of steps.</returns>
        private static EnchantUnit[] ComputeExperience(this List<long>[] orderingSteps)
        {
            EnchantUnit[] xpList = new EnchantUnit[orderingSteps.Length];
            for (int i = 0; i < orderingSteps.Length; i++)
            {
                xpList[i] = orderingSteps[i].ComputeExperience();
            }
            return xpList;
        }

        /// <summary>
        /// Aggregates a single enchantment step into one <see cref="EnchantUnit"/> by iteratively
        /// pairing and merging adjacent units until a single result remains (balanced reduction).
        /// </summary>
        /// <param name="orderingStep">
        /// The experience values that compose a single step. Must contain at least one element.
        /// </param>
        /// <returns>
        /// The combined <see cref="EnchantUnit"/> representing the merged result for this step.
        /// </returns>
        private static EnchantUnit ComputeExperience(this List<long> orderingStep)
        {
            // Initialize leaf nodes from raw experience values
            EnchantUnit[] temp = new EnchantUnit[orderingStep.Count];
            for (int j = 0; j < orderingStep.Count; j++)
            {
                temp[j] = new EnchantUnit(orderingStep[j]);
            }

            // Iteratively reduce by pairwise merging (a + b), carrying the last forward if count is odd
            while (temp.Length > 1)
            {
                EnchantUnit[] result = new EnchantUnit[(temp.Length + 1) >> 1];
                for (int j = 0, k = 0; j < temp.Length; j++, k++)
                {
                    EnchantUnit first = temp[j];
                    result[k] = ++j < temp.Length ? first + temp[j] : first;
                }
                temp = result;
            }

            // Return the final aggregate unit
            return temp[0];
        }

        /// <summary>
        /// Calculates a list of experience levels based on the provided enchantment items and an initial penalty.
        /// </summary>
        /// <param name="xpList">A list of <see cref="EnchantUnit"/> objects representing the base experience levels.</param>
        /// <param name="initialPenalty">An integer value representing the initial penalty to apply to the calculation.</param>
        /// <returns>A list of long integers where each value represents the calculated experience level for the corresponding
        /// enchantment item.</returns>
        private static long[] GetLevelList(this EnchantUnit[] xpList, int initialPenalty)
        {
            long[] results = new long[xpList.Length];
            for (int index = 0; index < xpList.Length; index++)
            {
                EnchantUnit item = new(0, index + initialPenalty);
                item += xpList[index];
                results[index] = item.StepLevel;
            }
            return results;
        }

        /// <summary>
        /// Calculates the total experience level after applying a series of enchantment levels and an initial penalty.
        /// </summary>
        /// <param name="xpList">A collection of <see cref="EnchantUnit"/> objects representing the enchantment levels to be applied.</param>
        /// <param name="initialPenalty">The initial penalty to be applied to the experience level.</param>
        /// <returns>The total experience level after applying all enchantment levels and the initial penalty.</returns>
        private static long GetHistoryLevel(this EnchantUnit[] xpList, int initialPenalty)
        {
            EnchantUnit item = new(0, initialPenalty);
            foreach (EnchantUnit level in xpList)
            {
                item += level;
            }
            return item.HistoryLevel;
        }
    }
}
