using EnchantsOrder.Common;
using EnchantsOrder.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EnchantsOrder.Demo.Models
{
    internal class Enchantment : IEnchantment
    {
        public int Level { get; set; }
        public int Weight { get; set; }

        public bool Hidden { get; set; } = false;

        public string Name { get; set; }

        public string[] Items { get; set; }
        public string[] Incompatible { get; set; }

        public long Experience => (long)Level * Weight;

        public Enchantment(JsonProperty token)
        {
            Name = token.Name;

            JsonElement value = token.Value;
            if (value.TryGetProperty("levelMax", out JsonElement levelMaxProperty) && levelMaxProperty.TryGetInt32(out int levelMax))
            {
                Level = levelMax;
            }

            if (value.TryGetProperty("weight", out JsonElement weightProperty) && weightProperty.TryGetInt32(out int weight))
            {
                Weight = weight;
            }

            if (value.TryGetProperty("hidden", out JsonElement hiddenProperty))
            {
                Hidden = hiddenProperty.GetBoolean();
            }

            if (value.TryGetProperty("items", out JsonElement itemsProperty))
            {
                Items = itemsProperty.EnumerateArray().Select((x) => x.GetString()).ToArray();
            }

            if (value.TryGetProperty("incompatible", out JsonElement incompatibleProperty))
            {
                Incompatible = incompatibleProperty.EnumerateArray().Select((x) => x.GetString()).ToArray();
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
