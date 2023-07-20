using EnchantsOrder.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Enchantment = EnchantsOrder.Demo.Models.Enchantment;

namespace EnchantsOrder.Demo
{
    internal class Program
    {
        private static readonly List<Enchantment> Enchantments = new();

        private static void Main(string[] args)
        {
            InitEnchantments();
            string text = args.FirstOrDefault();
            while (!(text?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true))
            {
                Console.WriteLine("Input the command (type exit to quit): ");
                Console.Write("> ");
                text = Console.ReadLine();
                Console.WriteLine();
                switch (text.ToLowerInvariant())
                {
                    case "order":
                        OrderEnchantments();
                        break;
                    case "list":
                        ListOrderedEnchantments();
                        break;
                    case "lang":
                        ChangeLanguage();
                        break;
                    case "helper":
                        Console.WriteLine("*****************");
                        Console.WriteLine("Helper");
                        Console.WriteLine("*****************");
                        Console.WriteLine("Commands:");
                        Console.WriteLine("order: Order enchantments");
                        Console.WriteLine("list: List ordered enchantments");
                        Console.WriteLine("lang: Change language");
                        Console.WriteLine("helper: Show this helper");
                        Console.WriteLine("exit: Exit");
                        break;
                    case "exit":
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
                Console.WriteLine();
            }
        }

        private static void InitEnchantments()
        {
            Enchantments.Clear();
            string json = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", CultureInfo.CurrentCulture.TwoLetterISOLanguageName.StartsWith("zh") ? "Enchants.zh-CN.json" : "Enchants.en-US.json");
            using StreamReader file = File.OpenText(json);
            using JsonTextReader reader = new(file);
            foreach (JToken token in JToken.ReadFrom(reader))
            {
                Enchantments.Add(new(token));
            }
        }

        private static void ChangeLanguage()
        {
            try
            {
                Console.WriteLine($"Current language is {CultureInfo.CurrentCulture.DisplayName}");
                Console.WriteLine("Input the language code to change language (type exit to quit): ");
                Console.Write("> ");
                string text = Console.ReadLine();
                if (text.Equals("exit", StringComparison.OrdinalIgnoreCase)) { return; }
                CultureInfo culture = new(text);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                InitEnchantments();
                Console.WriteLine($"Current language is changed to {CultureInfo.CurrentCulture.DisplayName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void OrderEnchantments()
        {
            string text = string.Empty;
            List<IEnchantment> enchantmentList = new();
            while (text.ToUpper() != "Q")
            {
                Console.WriteLine("Input the name of enchantment (type q to order): ");
                Console.Write("> ");
                text = Console.ReadLine();
                if (text.ToUpper() == "Q") { break; }
                if (Enchantments.FirstOrDefault((x) => x.Name == text) is Enchantment enchantment)
                {
                    enchantmentList.Add(enchantment);
                    Console.WriteLine($"{text} Added");
                }
                else
                {
                    Console.WriteLine("No such that enchantment");
                }
            }

            Console.WriteLine("Start ordering...");
            Console.WriteLine("*****************");

            try
            {
                OrderingResults results = enchantmentList.Ordering();
                Console.WriteLine(results.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ListOrderedEnchantments()
        {
            string text = string.Empty;
            while (!text.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Input the name of item (type exit to quit): ");
                Console.Write("> ");
                text = Console.ReadLine();
                if (text.Equals("exit", StringComparison.OrdinalIgnoreCase)) { break; }
                IEnumerable<Enchantment> enchantments = Enchantments.Where((x) => !x.Hidden && x.Items.Contains(text));
                if (enchantments.Any())
                {
                    IEnumerable<Enchantment> incompatibles = enchantments.Where((x) =>
                    {
                        foreach (Enchantment enchantment in enchantments)
                        {
                            if (x.Incompatible.Contains(enchantment.Name))
                            {
                                return true;
                            }
                        }
                        return false;
                    });
                    if (incompatibles.Any())
                    {
                        IEnumerable<Enchantment> lists = enchantments.Where((x) =>
                        {
                            foreach (Enchantment enchantment in incompatibles)
                            {
                                if (x.Name == enchantment.Name)
                                {
                                    return false;
                                }
                            }
                            return true;
                        });
                        foreach (Enchantment enchantment in incompatibles)
                        {
                            try
                            {
                                Console.WriteLine("*****************");
                                OrderingResults results = lists.Append(enchantment).Ordering();
                                Console.WriteLine(results.ToString());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            Console.WriteLine("*****************");
                            OrderingResults results = enchantments.Ordering();
                            Console.WriteLine(results.ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No such that item");
                }
                Console.WriteLine();
            }
        }
    }
}
