using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace OrderEnchant.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string text = string.Empty;
            List<(string, int)> enchantment_list = new List<(string, int)>();
            string jsonfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output.json");//JSON文件路径

            using (StreamReader file = File.OpenText(jsonfile))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o = (JObject)JToken.ReadFrom(reader);
                    while (text.ToUpper() != "Q")
                    {
                        Console.WriteLine("Input the name of enchantment(type q to order): ");
                        text = Console.ReadLine();
                        if (text.ToUpper() == "Q") { break; }
                        if (o.TryGetValue(text, out JToken v))
                        {
                            JObject value = (JObject)v;
                            if (value.TryGetValue("levelMax", out JToken levelMax) && value.TryGetValue("weight", out JToken weight))
                            {
                                (string text, int) enchantment = (text, levelMax.ToObject<int>() * weight.ToObject<int>());
                                enchantment_list.Add(enchantment);
                                Console.WriteLine($"{text} Added");
                            }
                            else
                            {
                                Console.WriteLine("The source of this enchantment is broken");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No such that enchantment");
                        }
                    }
                }
            }

            Console.WriteLine("Start ordering...");
            Console.WriteLine("*****************");
            (List<(List<(string name, int ex)> enchantments, int step)> ordering, double xp_max, int penalty, double xp_sum) = Instance.Ordering(enchantment_list);
            foreach ((List<(string name, int ex)> enchantments, int step) in ordering)
            {
                string str = $"step {step}:";
                foreach ((string name, int ex) in enchantments)
                {
                    str += $" {name} xp:{ex}";
                }
                Console.WriteLine(str);
            }
            Console.WriteLine($"max xp:{xp_max}");
            Console.WriteLine($"final penalty:{penalty}");
            Console.WriteLine($"total xp level:{xp_sum}");

            Console.Write("Press any key to exit...");
            Console.ReadKey(true);
        }
    }
}
