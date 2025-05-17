using EnchantsOrder.Demo.Common;
using EnchantsOrder.Demo.Models;
using EnchantsOrder.Demo.Properties.Resource;
using EnchantsOrder.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
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

        private static Task<int> Main(string[] args)
        {
            InitializeSettings();
            InitializeLanguage();
            InitializeEnchantments();

            Option<int[]> levelOption = new("--level", "-l")
            {
                Arity = ArgumentArity.ZeroOrMore,
                AllowMultipleArgumentsPerToken = true,
                Description = Resource.LevelOptionDescription,
                HelpName = Resource.LevelOption
            };

            Option<int[]> weightOption = new("--weight", "-w")
            {
                Arity = ArgumentArity.ZeroOrMore,
                AllowMultipleArgumentsPerToken = true,
                Description = Resource.WeightOptionDescription,
                HelpName = Resource.WeightOption
            };

            Argument<IEnumerable<IEnchantment>> enchantmentArgument = new("enchantments")
            {
                Arity = ArgumentArity.OneOrMore,
                Description = Resource.EnchantmentArgumentDescription,
                HelpName = Resource.EnchantmentArgument,
                CustomParser = x =>
                {
                    int[] levels = x.GetValue(levelOption);
                    int[] weights = x.GetValue(weightOption);
                    return levels == null
                        ? x.Tokens.Select(token =>
                        {
                            if (Enchantments.FirstOrDefault(item => item.Name == token.Value) is Enchantment enchantment)
                            {
                                return enchantment;
                            }
                            else
                            {
                                x.AddError(string.Format(Resource.NotFoundEnchantmentFormat, token.Value));
                                return null;
                            }
                        })
                        : weights == null
                            ? x.Tokens.Select((token, index) =>
                            {
                                if (Enchantments.FirstOrDefault(item => item.Name == token.Value) is IEnchantment enchantment)
                                {
                                    if (levels.Length > index)
                                    {
                                        enchantment = new global::EnchantsOrder.Models.Enchantment(enchantment.Name, levels[index], enchantment.Weight);
                                    }
                                    return enchantment;
                                }
                                else
                                {
                                    x.AddError(string.Format(Resource.NotFoundEnchantmentFormat, token.Value));
                                    return null;
                                }
                            })
                            : x.Tokens.Select((token, index) =>
                            {
                                if (weights.Length > index)
                                {
                                    return new global::EnchantsOrder.Models.Enchantment(token.Value, levels.Length > index ? levels[index] : 1, weights[index]);
                                }
                                else if (Enchantments.FirstOrDefault(item => item.Name == token.Value) is IEnchantment enchantment)
                                {
                                    if (levels.Length > index)
                                    {
                                        enchantment = new global::EnchantsOrder.Models.Enchantment(enchantment.Name, levels[index], enchantment.Weight);
                                    }
                                    return enchantment;
                                }
                                else
                                {
                                    x.AddError(string.Format(Resource.NotFoundEnchantmentFormat, token.Value));
                                    return null;
                                }
                            });
                }
            };
            enchantmentArgument.Validators.Add(x =>
            {
                int[] levels = x.GetValue(levelOption);
                int[] weights = x.GetValue(weightOption);
                if (levels != null && levels.Length > x.Tokens.Count)
                {
                    x.AddError(string.Format(Resource.TooManyLevelsFormat, levels.Length, x.Tokens.Count));
                    return;
                }
                if (weights != null && weights.Length > x.Tokens.Count)
                {
                    x.AddError(string.Format(Resource.TooManyWeightsFormat, weights.Length, x.Tokens.Count));
                    return;
                }
                IEnumerable<Token> tokens = weights == null ? x.Tokens : x.Tokens.Skip(weights.Length);
                foreach (Token token in tokens)
                {
                    if (!Enchantments.Select(x => x.Name).Any(x => x.Equals(token.Value, StringComparison.OrdinalIgnoreCase)))
                    {
                        x.AddError(string.Format(Resource.NotFoundEnchantmentFormat, token.Value));
                    }
                }
            });
            enchantmentArgument.CompletionSources.Add(
                x => from enchantment in Enchantments
                     where string.IsNullOrWhiteSpace(x.WordToComplete) || enchantment.Name.StartsWith(x.WordToComplete.Trim('\'', '\"', ' '), StringComparison.OrdinalIgnoreCase)
                     select enchantment.Name.Contains(' ') ? $"'{enchantment.Name}'" : enchantment.Name);

            Option<int> penaltyOption = new("--penalty", "-p")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = Resource.PenaltyOptionDescription,
                DefaultValueFactory = _ => 0,
                HelpName = Resource.PenaltyOption
            };

            Command orderCommand = new("order", Resource.OrderCommandDescription)
            {
                enchantmentArgument,
                levelOption,
                weightOption,
                penaltyOption
            };
            orderCommand.SetAction(x => OrderCommandHandler(x.GetValue(enchantmentArgument), x.GetValue(penaltyOption)));

            Argument<string> itemArgument = new("item")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Resource.ItemArgumentDescription,
                HelpName = Resource.ItemArgument
            };
            itemArgument.Validators.Add(x =>
            {
                string value = x.GetValue(itemArgument);
                if (!Array.Exists(Items, item => item.Equals(value, StringComparison.OrdinalIgnoreCase)))
                {
                    x.AddError(string.Format(Resource.NotFoundItemFormat, value));
                }
            });
            itemArgument.CompletionSources.Add(
                x => from item in Items
                     where string.IsNullOrWhiteSpace(x.WordToComplete) || item.StartsWith(x.WordToComplete.Trim('\'', '\"', ' '), StringComparison.OrdinalIgnoreCase)
                     select item.Contains(' ') ? $"'{item}'" : item);

            Command listCommand = new("list", Resource.ListCommandDescription)
            {
                itemArgument,
                penaltyOption
            };
            listCommand.SetAction(x => ListCommandHandler(x.GetValue(itemArgument), x.GetValue(penaltyOption)));

            Argument<string> langArgument = new("code")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = Resource.LangArgumentDescription,
                HelpName = Resource.LangArgument
            };
            langArgument.CompletionSources.Add(
                x => from code in (IEnumerable<string>)["default", "zh-CN", "en-US"]
                     where string.IsNullOrWhiteSpace(x.WordToComplete) || code.StartsWith(x.WordToComplete.Trim('\'', '\"', ' '), StringComparison.OrdinalIgnoreCase)
                     select code);

            Command langCommand = new("lang", Resource.LangCommandDescription)
            {
                langArgument,
            };
            langCommand.SetAction(x => LangCommandHandler(x.GetValue(langArgument)));

            RootCommand rootCommand = new(Resource.RootCommandDescription)
            {
                orderCommand,
                listCommand,
                langCommand
            };
            rootCommand.SetAction(_ => RootCommandHandler());

            return new CommandLineConfiguration(rootCommand).InvokeAsync(args);
        }

        private static void RootCommandHandler()
        {
            string text = string.Empty;
            while (text?.Equals("exit", StringComparison.OrdinalIgnoreCase) != true)
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
                        Console.WriteLine("  order <{0}>\t{1}", Resource.EnchantmentArgument, Resource.OrderCommandDescription);
                        Console.WriteLine("  list <{0}>\t{1}", Resource.ItemArgument, Resource.ListCommandDescription);
                        Console.WriteLine("  lang <{0}>\t{1}", Resource.LangArgument, Resource.LangCommandDescription);
                        break;
                    case "version":
                        Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
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
            Items = Enchantments.OrderByDescending(x => x.Items.Length).FirstOrDefault().Items;
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
                enchantmentList.AddRange(enchantments.Select(item =>
                {
                    if (Enchantments.FirstOrDefault(x => x.Name == item) is Enchantment enchantment)
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
                        else if (Enchantments.FirstOrDefault(x => x.Name == text) is Enchantment enchantment)
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

            OrderCommandHandler(enchantmentList, initialPenalty);
        }

        private static void OrderCommandHandler(IEnumerable<IEnchantment> enchantments, int initialPenalty = 0)
        {
            Console.WriteLine(Resource.StartOrdering);
            Console.WriteLine("*****************");
            try
            {
                OrderingResults results = enchantments.Ordering(initialPenalty);
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
            List<Enchantment> enchantments = [.. Enchantments.Where(x => !x.Hidden && x.Items.Any(x => x.Equals(text, StringComparison.OrdinalIgnoreCase)))];
            if (enchantments.Count > 0)
            {
                List<List<IEnchantment>> group = [];
                while (enchantments.Count > 0)
                {
                    Enchantment enchantment = enchantments[0];
                    List<IEnchantment> list = [enchantment];
                    enchantments.RemoveAt(0);
                    if (enchantment.Incompatible.Length > 0)
                    {
                        for (int i = enchantments.Count; --i >= 0;)
                        {
                            Enchantment temp = enchantments[i];
                            if (temp.Incompatible.Any(x => x.Equals(enchantment.Name, StringComparison.OrdinalIgnoreCase)))
                            {
                                list.Add(temp);
                                enchantments.RemoveAt(i);
                            }
                        }
                        if (list.Count > 0)
                        {
                            List<IEnchantment> tempList = [];
                            while (list.Count > 0)
                            {
                                IEnchantment temp = list[0];
                                list.RemoveAt(0);
                                IEnumerable<IEnchantment> temps = list.Where(x => x.Level == temp.Level && x.Weight == temp.Weight);
                                if (temps.Any())
                                {
                                    IEnchantment[] array = [.. temps.Append(temp).OrderBy(x => x.Name)];
                                    tempList.Add(new EnchantmentGroup(array));
                                    foreach (IEnchantment enchantmentTemp in array)
                                    {
                                        list.Remove(enchantmentTemp);
                                    }
                                }
                                else
                                {
                                    tempList.Add(temp);
                                }
                            }
                            list = tempList;
                        }
                    }
                    group.Add(list);
                }

                static List<List<IEnchantment>> GetAllEnchantmentPaths(List<List<IEnchantment>> group)
                {
                    List<List<IEnchantment>> result = [];
                    void Growing(int depth = 0, params List<IEnchantment> path)
                    {
                        if (depth == group.Count)
                        {
                            result.Add([.. path]);
                            return;
                        }
                        int next = depth + 1;
                        foreach (IEnchantment enchantment in group[depth])
                        {
                            path.Add(enchantment);
                            Growing(next, path);
                            path.RemoveAt(path.Count - 1);
                        }
                    }
                    Growing();
                    return result;
                }

                Console.WriteLine(text);
                foreach (List<IEnchantment> list in GetAllEnchantmentPaths(group))
                {
                    try
                    {
                        Console.WriteLine("*****************");
                        OrderingResults results = list.Ordering(initialPenalty);
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
