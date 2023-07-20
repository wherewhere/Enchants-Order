using EnchantsOrder.Demo.Properties;
using EnchantsOrder.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Enchantment = EnchantsOrder.Demo.Models.Enchantment;

namespace EnchantsOrder.Demo
{
    internal class Program
    {
        private static readonly List<Enchantment> Enchantments = new();

        private static async Task<int> Main(string[] args)
        {
            InitLanguage();
            InitEnchantments();

            Option<IEnumerable<string>> enchantmentArgument = new("--enchantments", Array.Empty<string>, "Set the enchantments which you want to enchant")
            {
                AllowMultipleArgumentsPerToken = true
            };
            enchantmentArgument.AddAlias("-e");

            Option<int> penaltyOption = new("--penalty", () => 0, "Set the penalty of your item which you want to enchant");
            penaltyOption.AddAlias("-p");

            Command orderCommand = new("order", "Ordering the enchantments")
            {
                enchantmentArgument,
                penaltyOption
            };
            orderCommand.SetHandler(OrderCommandHandler, enchantmentArgument, penaltyOption);

            Argument<string> itemArgument = new("item", () => string.Empty, "Set the item which you want to list enchantments");

            Command listCommand = new("list", "List ordered enchantments of item")
            {
                itemArgument,
                penaltyOption
            };
            listCommand.SetHandler(ListCommandHandler, itemArgument, penaltyOption);

            Argument<string> langArgument = new("code", () => string.Empty, "Set the language code you want to change");

            Command langCommand = new("lang", "Change the language of this program")
            {
                langArgument,
            };
            langCommand.SetHandler(LangCommandHandler, langArgument);

            RootCommand rootCommand = new("A demo of EnchantsOrder which can order enchantments")
            {
                orderCommand,
                listCommand,
                langCommand
            };
            rootCommand.SetHandler(RootCommandHandler);

            return await rootCommand.InvokeAsync(args);
        }

        private static void RootCommandHandler()
        {
            string text = string.Empty;
            while (!(text?.Equals("exit", StringComparison.OrdinalIgnoreCase) == true))
            {
                Console.WriteLine("Input the command (type exit to quit): ");
                Console.Write("> ");
                text = Console.ReadLine();
                Console.WriteLine();
                switch (text.ToLowerInvariant())
                {
                    case "order":
                        OrderCommandHandler();
                        break;
                    case "list":
                        ListCommandHandler();
                        break;
                    case "lang":
                        LangCommandHandler();
                        break;
                    case "helper":
                        Console.WriteLine("Description:");
                        Console.WriteLine("  Ordering the enchantments");
                        Console.WriteLine();
                        Console.WriteLine("Usage:");
                        Console.WriteLine("  EnchantsOrder.Demo order [options]");
                        Console.WriteLine();
                        Console.WriteLine("Options:");
                        Console.WriteLine("  --version       Show version information");
                        Console.WriteLine("  -?, -h, --help  Show help and usage information");
                        Console.WriteLine();
                        Console.WriteLine("Commands:");
                        Console.WriteLine("  order        Ordering the enchantments");
                        Console.WriteLine("  list <item>  List ordered enchantments of item");
                        Console.WriteLine("  lang <code>  Change the language of this program");
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

        private static void InitLanguage()
        {
            string code = Settings.Default.Language;
            if(!code.Equals("default"))
            {
                try
                {
                    CultureInfo culture = new(code);
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Settings.Default.Language = "default";
                    Settings.Default.Save();
                }
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

        private static void LangCommandHandler(string code = "")
        {
            try
            {
                string text = code;
                if (string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine($"Current language is {CultureInfo.CurrentCulture.DisplayName}");
                    Console.WriteLine("Input the language code to change language (type exit to quit): ");
                    Console.Write("> ");
                    text = Console.ReadLine();
                }

                if (text.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Function cancelled.");
                    return;
                }

                if (text.Equals("null", StringComparison.OrdinalIgnoreCase) || text.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    CultureInfo.DefaultThreadCurrentCulture = null;
                    Settings.Default.Language = "default";
                    Settings.Default.Save();
                }
                else
                {
                    CultureInfo culture = new(text);
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    Settings.Default.Language = text;
                    Settings.Default.Save();
                }

                InitEnchantments();
                Console.WriteLine($"Current language is changed to {CultureInfo.CurrentCulture.DisplayName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void OrderCommandHandler(IEnumerable<string> enchantments = null, int initialPenalty = 0)
        {
            string text = string.Empty;
            List<IEnchantment> enchantmentList = new();

            if (enchantments?.Any() == true)
            {
                enchantmentList.AddRange(enchantments.Select((item) =>
                {
                    if (Enchantments.FirstOrDefault((x) => x.Name == item) is Enchantment enchantment)
                    {
                        return enchantment;
                    }
                    else
                    {
                        Console.WriteLine($"Not found enchantment named {item}.");
                        return null;
                    }
                }));
            }
            else
            {
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
                        Console.WriteLine($"Not found enchantment named {text}.");
                    }
                }
            }

            Console.WriteLine("Start ordering...");
            Console.WriteLine("*****************");

            try
            {
                OrderingResults results = enchantmentList.Ordering(initialPenalty);
                Console.WriteLine(results.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ListCommandHandler(string item = "", int initialPenalty = 0)
        {
            string text = item;
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Input the name of item (type exit to quit): ");
                Console.Write("> ");
                text = Console.ReadLine();
                if (text.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Function cancelled.");
                    return;
                }
            }

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
                            OrderingResults results = lists.Append(enchantment).Ordering(initialPenalty);
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
                        OrderingResults results = enchantments.Ordering(initialPenalty);
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
                Console.WriteLine($"Not found item named {text}.");
            }
        }
    }
}
