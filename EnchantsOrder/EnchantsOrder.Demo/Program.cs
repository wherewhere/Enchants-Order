using EnchantsOrder.Demo.Common;
using EnchantsOrder.Demo.Properties.Resource;
using EnchantsOrder.Models;
using Microsoft.Extensions.Configuration;
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
        private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder().AddWritableJsonFile(Path.Combine("Assets", "AppSettings.json"), true, true).Build();

        private static async Task<int> Main(string[] args)
        {
            InitSettings();
            InitLanguage();
            InitEnchantments();

            Option<IEnumerable<string>> enchantmentArgument = new("--enchantments", Array.Empty<string>, Resource.EnchantmentArgument)
            {
                AllowMultipleArgumentsPerToken = true
            };
            enchantmentArgument.AddAlias("-e");
            enchantmentArgument.AddCompletions(Enchantments.Select((x) => x.Name).ToArray());

            Option<int> penaltyOption = new("--penalty", () => 0, Resource.PenaltyOption);
            penaltyOption.AddAlias("-p");

            Command orderCommand = new("order", Resource.OrderCommand)
            {
                enchantmentArgument,
                penaltyOption
            };
            orderCommand.SetHandler(OrderCommandHandler, enchantmentArgument, penaltyOption);

            Argument<string> itemArgument = new("item", () => string.Empty, Resource.ItemArgument);
            itemArgument.AddCompletions(Enchantments.OrderByDescending((x) => x.Items.Count()).FirstOrDefault().Items.ToArray());

            Command listCommand = new("list", Resource.ListCommand)
            {
                itemArgument,
                penaltyOption
            };
            listCommand.SetHandler(ListCommandHandler, itemArgument, penaltyOption);

            Argument<string> langArgument = new("code", () => string.Empty, Resource.LangArgument);
            langArgument.AddCompletions("zh-CN", "en-US");

            Command langCommand = new("lang", Resource.LangCommand)
            {
                langArgument,
            };
            langCommand.SetHandler(LangCommandHandler, langArgument);

            RootCommand rootCommand = new(Resource.RootCommand)
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
                Console.WriteLine(Resource.InputCommand);
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
                        Console.WriteLine($"  {Resource.RootCommand}");
                        Console.WriteLine();
                        Console.WriteLine("Usage:");
                        Console.WriteLine("  EnchantsOrder.Demo order [options]");
                        Console.WriteLine();
                        Console.WriteLine("Options:");
                        Console.WriteLine("  --version       Show version information");
                        Console.WriteLine("  -?, -h, --help  Show help and usage information");
                        Console.WriteLine();
                        Console.WriteLine("Commands:");
                        Console.WriteLine($"  order        {Resource.OrderCommand}");
                        Console.WriteLine($"  list <item>  {Resource.ListCommand}");
                        Console.WriteLine($"  lang <code>  {Resource.LangCommand}");
                        break;
                    case "exit":
                        break;
                    default:
                        Console.WriteLine(Resource.UnknownCommand);
                        break;
                }
                Console.WriteLine();
            }
        }

        private static void InitSettings()
        {
            if (string.IsNullOrEmpty(Configuration["Language"]))
            {
                Configuration["Language"] = "default";
            }
        }

        private static void InitLanguage()
        {
            string code = Configuration["Language"];
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
                    Configuration["Language"] = "default";
                }
            }
        }

        private static void InitEnchantments()
        {
            Enchantments.Clear();
            string json = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Enchants", Resource.EnchantsFileName);
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
                    Console.WriteLine(string.Format(Resource.CurrentLanguageFormat, CultureInfo.CurrentCulture.DisplayName));
                    Console.WriteLine(Resource.InputLanguageCode);
                    Console.Write("> ");
                    text = Console.ReadLine();
                }

                if (text.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(Resource.FunctionCancelled);
                    return;
                }

                if (text.Equals("null", StringComparison.OrdinalIgnoreCase) || text.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    CultureInfo.DefaultThreadCurrentCulture = null;
                    Configuration["Language"] = "default";
                }
                else
                {
                    CultureInfo culture = new(text);
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    Configuration["Language"] = text;
                }

                InitEnchantments();
                Console.WriteLine(string.Format(Resource.CurrentLanguageChangedFormat, CultureInfo.CurrentCulture.DisplayName));
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
                        Console.WriteLine(string.Format(Resource.NotFoundEnchantmentFormat, item));
                        return null;
                    }
                }));
            }
            else
            {
                while (text.ToUpper() != "Q")
                {
                    Console.WriteLine(Resource.InputEnchantment);
                    Console.Write("> ");
                    text = Console.ReadLine();
                    if (text.ToUpper() == "Q") { break; }
                    if (Enchantments.FirstOrDefault((x) => x.Name == text) is Enchantment enchantment)
                    {
                        enchantmentList.Add(enchantment);
                        Console.WriteLine(string.Format(Resource.AddedFormat, text));
                    }
                    else
                    {
                        Console.WriteLine(string.Format(Resource.NotFoundEnchantmentFormat, text));
                    }
                }
            }

            Console.WriteLine(Resource.StartOrdering);
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
                Console.WriteLine(Resource.InputItem);
                Console.Write("> ");
                text = Console.ReadLine();
                if (text.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(Resource.FunctionCancelled);
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
                Console.WriteLine(string.Format(Resource.NotFoundItemFormat, text));
            }
        }
    }
}
