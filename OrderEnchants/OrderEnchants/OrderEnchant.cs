using OrderEnchants.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderEnchant
{
    public static class Instance
    {
        public static OrderingResults Ordering(List<Enchantment> wantedlist, int inital_penalty = 0)
        {
            if (!wantedlist.Any())
            {
                throw new Exception("Cannot enchant: no enchantment given");
            }

            //get the xp required and in（tobe_enchanted it
            List<Enchantment> sortedlist = wantedlist;
            sortedlist.Sort((x, y) => y.CompareTo(x));
            List<int> numlist = sortedlist.Select((x) => x.Experience).ToList();
#if NETCOREAPP
            int total_step = Convert.ToInt32(Math.Log2(sortedlist.Count)) + 1;
#else
            int total_step = Convert.ToInt32(Math.Log(sortedlist.Count) / Math.Log(2)) + 1;
#endif
            int penalty = inital_penalty + total_step;
            if (penalty > 6)
            {
                throw new Exception("Cannot enchant: final penalty larger than 6");
            }
            // the factor of enchantment (first 16 books, should be enough though), no idea to form an equation or function
            List<int> multiplyfactor = new List<int> { 0, 1, 1, 2, 1, 2, 2, 3, 1, 2, 2, 3, 2, 3, 3, 4 };
            List<int>[] ordering_num = new List<int>[total_step];
            List<EnchantmentStep> ordering = new List<EnchantmentStep>();
            // multiplied enchatment factor
            int xp_extra_level = 0;
            List<object> priority_list = new List<object>();
            // generate base enchantment level list
            // i.e. add the sum of penalty level by item and merged books
            // also, count the max_step
            int total_enchantment = numlist.Count;
            EnchantLayerResults enchantlayer_results = EnchantLayer(total_step, total_enchantment, inital_penalty);
            List<double> xp_list = enchantlayer_results.XpList;
            List<double> max_step = enchantlayer_results.MaxStep;
            while (numlist.Any())
            {
                List<double> temp_xp_list = new List<double>();
                foreach (double i in max_step)
                {
                    if (i == 0)
                    {
                        temp_xp_list.Add(1000);
                    }
                    else
                    {
                        temp_xp_list.Add(i);
                    }
                }
                int step = temp_xp_list.IndexOf(temp_xp_list.Min());
                ordering_num[step] ??= new List<int>();
                int existed_num = ordering_num[step].Count;
                int tobe_enchanted = numlist.Max();
                ordering_num[step].Add(tobe_enchanted);
                xp_list[step] += tobe_enchanted;
                max_step[step] -= 1;
                // combining enchantments cause the level counted more than one times
                if (existed_num != 0)
                {
                    xp_extra_level += tobe_enchanted * multiplyfactor[existed_num];
                }
                numlist.Remove(tobe_enchanted);
            }
            double xp_max = xp_list.Max();
            if (xp_max > 39)
            {
                throw new Exception("Cannot enchant! max xp larger than 39");
            }
            //penalty of merged books
            double xp_penalty_book = 0;
            int enchantment_step = 0;
            foreach (List<int> element in ordering_num)
            {
                if (element == null) { continue; }
                enchantment_step++;
                ordering.Add(new EnchantmentStep(enchantment_step));
                //penalty for merge book
                xp_penalty_book += MergedPenaltyBook(element.Count);
                //list steps with name
                foreach (int j in element)
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
            double xp_sum = xp_list.Sum() + xp_extra_level + xp_penalty_book;
            return new OrderingResults(ordering, penalty, xp_max, xp_sum);
        }

        private static EnchantLayerResults EnchantLayer(int total_step, double total_enchantment, int inital_penalty)
        {
            List<double> xp_list = new List<double>();
            List<double> max_step = new List<double>();
            foreach (int i in Enumerable.Range(0, total_step))
            {
                // add the penalty level by item
                xp_list.Add(Math.Pow(2, i) + inital_penalty - 1);
                double num_of_enchantment = Math.Min(Math.Pow(2, i), total_enchantment);
                max_step.Add(num_of_enchantment);
                total_enchantment -= num_of_enchantment;
#if NETCOREAPP
                double merged_books_penalty = Math.Pow(2, Math.Ceiling(Math.Log2(num_of_enchantment))) - 1;
#else
                double merged_books_penalty = Math.Pow(2, Math.Ceiling(Math.Log(num_of_enchantment) / Math.Log(2))) - 1;
#endif
                // add the penalty level by merged books
                xp_list[i] += merged_books_penalty;
            }
            return new EnchantLayerResults(xp_list, max_step);
        }

        private static double MergedPenaltyBook(int num)
        {
            if (num == 0)
            {
                return 0;
            }
            double xp = 0;
            List<int> p_list = Enumerable.Range(0, num).Select((x) => 0).ToList();
            while (p_list.Count != 1)
            {
                List<int> new_list = new List<int>();
                foreach (int i in Enumerable.Range(0, num / 2))
                {
                    xp += p_list.GetRange(i * 2, 2).Select((x) => Math.Pow(2, x) - 1).ToList().Sum();
                    new_list.Add(p_list.GetRange(i * 2, 2).Max() + 1);
                }
                if (num % 2 != 0)
                {
                    new_list.Add(p_list.Last());
                }
                p_list = new_list;
                num = p_list.Count;
            }
            return xp;
        }
    }
}
