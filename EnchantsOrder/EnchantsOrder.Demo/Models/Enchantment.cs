using EnchantsOrder.Common;
using EnchantsOrder.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace EnchantsOrder.Demo.Models
{
    internal class Enchantment : IEnchantment
    {
        public int Level { get; set; }
        public int Weight { get; set; }

        public bool Hidden { get; set; } = false;

        public string Name { get; set; }

        public IEnumerable<string> Items { get; set; }
        public IEnumerable<string> Incompatible { get; set; }

        public long Experience => (long)Level * Weight;

        public Enchantment(JToken token)
        {
            if (token != null)
            {
                if (token is JProperty property)
                {
                    Name = property.Name;

                    JObject v = (JObject)property.Value;
                    if (v.TryGetValue("levelMax", out JToken levelMax))
                    {
                        Level = levelMax.ToObject<int>();
                    }

                    if (v.TryGetValue("weight", out JToken weight))
                    {
                        Weight = weight.ToObject<int>();
                    }

                    if (v.TryGetValue("hidden", out JToken hidden))
                    {
                        Hidden = hidden.ToObject<bool>();
                    }

                    if (v.TryGetValue("items", out JToken items))
                    {
                        Items = items.Select((x) => x.ToString());
                    }

                    if (v.TryGetValue("incompatible", out JToken incompatible))
                    {
                        Incompatible = incompatible.Select((x) => x.ToString());
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Name} {Level.GetRomanNumber()}";

        /// <inheritdoc/>
        public int CompareTo(IEnchantment other)
        {
            if (other is null) { return -1; }
            int value = Experience.CompareTo(other.Experience);
            if (value == 0)
            {
                value = Level.CompareTo(other.Level);
                if (value == 0)
                {
                    value = Name.CompareTo(other.Name);
                }
            }
            return value;
        }

        /// <inheritdoc/>
        public bool Equals(IEnchantment other) => CompareTo(other) == 0;
    }
}
