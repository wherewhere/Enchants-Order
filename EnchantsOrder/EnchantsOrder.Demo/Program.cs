using EnchantsOrder.Demo.Common;
using EnchantsOrder.Demo.Properties.Resource;
using EnchantsOrder.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Enchantment = EnchantsOrder.Demo.Models.Enchantment;

namespace EnchantsOrder.Demo
{
    internal class Program
    {
        private static string[] Items = [];
        private static readonly List<Enchantment> Enchantments = [];
        private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder().AddWritableJsonFile(Path.Combine("Assets", "AppSettings.json"), true, true).Build();

        private static async Task<int> Main(string[] args)
        {
            InitializeSettings();
            InitializeLanguage();
            InitializeEnchantments();

            CliOption<IEnumerable<string>> enchantmentOption = new("--enchantments", "-e")
            {
                Arity = ArgumentArity.OneOrMore,
                AllowMultipleArgumentsPerToken = true,
                Description = Resource.EnchantmentOptionDescription,
                HelpName = Resource.EnchantmentOption
            };
            enchantmentOption.AcceptOnlyFromAmong(Enchantments.Select(x => x.Name).ToArray());

            CliOption<int> penaltyOption = new("--penalty", "-p")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Resource.PenaltyOptionDescription,
                DefaultValueFactory = _ => 0,
                HelpName = Resource.PenaltyOption
            };

            CliCommand orderCommand = new("order", Resource.OrderCommandDescription)
            {
                enchantmentOption,
                penaltyOption
            };
            orderCommand.SetAction(x => OrderCommandHandler(x.GetValue(enchantmentOption), x.GetValue(penaltyOption)));

            CliArgument<string> itemArgument = new("item")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Resource.ItemArgumentDescription,
                HelpName = Resource.ItemArgument
            };
            itemArgument.AcceptOnlyFromAmong(Items);

            CliCommand listCommand = new("list", Resource.ListCommandDescription)
            {
                itemArgument,
                penaltyOption
            };
            listCommand.SetAction(x => ListCommandHandler(x.GetValue(itemArgument), x.GetValue(penaltyOption)));

            CliArgument<string> langArgument = new("code")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Resource.LangArgumentDescription,
                HelpName = Resource.LangArgument
            };
            langArgument.CompletionSources.Add(
                x => from code in (IEnumerable<string>)["zh-CN", "en-US"]
                     where string.IsNullOrWhiteSpace(x.WordToComplete) || code.StartsWith(x.WordToComplete, StringComparison.OrdinalIgnoreCase)
                     select code);

            CliCommand langCommand = new("lang", Resource.LangCommandDescription)
            {
                langArgument,
            };
            langCommand.SetAction(x => LangCommandHandler(x.GetValue(langArgument)));

            CliRootCommand rootCommand = new(Resource.RootCommandDescription)
            {
                orderCommand,
                listCommand,
                langCommand
            };
            rootCommand.SetAction(_ => RootCommandHandler());

            return await new CliConfiguration(rootCommand).InvokeAsync(args);
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
                    case "?":
                    case "h":
                    case "helper":
                        Console.WriteLine("Description:");
                        Console.WriteLine("  {0}", Resource.RootCommandDescription);
                        Console.WriteLine();
                        Console.WriteLine("Usage:");
                        Console.WriteLine("  EnchantsOrder.Demo order [options]");
                        Console.WriteLine();
                        Console.WriteLine("Options:");
                        Console.WriteLine("  ?, h, help\tShow help and usage information");
                        Console.WriteLine("  version\tShow version information");
                        Console.WriteLine();
                        Console.WriteLine("Commands:");
                        Console.WriteLine("  order\t\t{0}", Resource.OrderCommandDescription);
                        Console.WriteLine("  list <{0}>\t{1}", Resource.ItemArgument, Resource.ListCommandDescription);
                        Console.WriteLine("  lang <{0}>\t{1}", Resource.LangArgument, Resource.LangCommandDescription);
                        break;
                    case "version":
                        Console.WriteLine(FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductVersion);
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

        private static void InitializeSettings()
        {
            if (string.IsNullOrEmpty(Configuration["Language"]))
            {
                Configuration["Language"] = "default";
            }
        }

        private static void InitializeLanguage()
        {
            string code = Configuration["Language"];
            if (!code.Equals("default"))
            {
                try
                {
                    CultureInfo culture = new(code);
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Configuration["Language"] = "default";
                }
            }
        }

        private static void InitializeEnchantments()
        {
            Enchantments.Clear();
            string json = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Enchants", Resource.EnchantsFileName);
            using StreamReader file = File.OpenText(json);
            JsonDocument document = JsonDocument.Parse(file.BaseStream);
            foreach (JsonProperty token in document.RootElement.EnumerateObject())
            {
                Enchantments.Add(new(token));
            }
            Items = Enchantments.OrderByDescending((x) => x.Items.Count()).FirstOrDefault().Items;
        }

        private static void LangCommandHandler(string code = "")
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

            try
            {
                if (text.Equals("null", StringComparison.OrdinalIgnoreCase) || text.Equals("default", StringComparison.OrdinalIgnoreCase))
                {
                    CultureInfo.DefaultThreadCurrentCulture = null;
                    CultureInfo.DefaultThreadCurrentUICulture = null;
                    Configuration["Language"] = "default";
                }
                else
                {
                    CultureInfo culture = new(text);
                    CultureInfo.DefaultThreadCurrentCulture = culture;
                    CultureInfo.DefaultThreadCurrentUICulture = culture;
                    Configuration["Language"] = text;
                }

                InitializeEnchantments();
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
            List<IEnchantment> enchantmentList = [];

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
                    try
                    {
                        Console.WriteLine(Resource.InputEnchantment);
                        Console.Write("> ");
                        text = Console.ReadLine();
                        if (text.ToUpper() == "Q") { break; }
                        else if (text.ToUpper() == "C")
                        {
                            Console.WriteLine(Resource.InputName);
                            Console.Write("> ");
                            string name = Console.ReadLine();
                            Console.WriteLine(Resource.InputLevel);
                            Console.Write("> ");
                            int.TryParse(Console.ReadLine(), out int level);
                            Console.WriteLine(Resource.InputWeight);
                            Console.Write("> ");
                            int.TryParse(Console.ReadLine(), out int weight);
                            global::EnchantsOrder.Models.Enchantment enchantment = new(name, level, weight);
                            enchantmentList.Add(enchantment);
                            Console.WriteLine(string.Format(Resource.AddedFormat, name));
                        }
                        else if (Enchantments.FirstOrDefault((x) => x.Name == text) is Enchantment enchantment)
                        {
                            enchantmentList.Add(enchantment);
                            Console.WriteLine(string.Format(Resource.AddedFormat, text));
                        }
                        else
                        {
                            Console.WriteLine(string.Format(Resource.NotFoundEnchantmentFormat, text));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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

            Console.WriteLine(Resource.StartOrdering);
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
