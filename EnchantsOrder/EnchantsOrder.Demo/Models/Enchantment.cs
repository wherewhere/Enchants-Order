using EnchantsOrder.Common;
using EnchantsOrder.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EnchantsOrder.Demo.Models
{
    internal sealed partial class Enchantment : IEnchantment
    {
        /// <inheritdoc/>
        [JsonPropertyName("levelMax")]
        public int Level { get; set; }

        /// <inheritdoc/>
        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        /// <summary>
        /// Get or set is hidden.
        /// </summary>
        [JsonPropertyName("hidden")]
        public bool Hidden { get; set; } = false;

        /// <inheritdoc/>
        [JsonIgnore]
        public string Name { get; set; }

        /// <summary>
        /// The items this <see cref="Enchantment"/> can enchant.
        /// </summary>
        [JsonPropertyName("items")]
        public string[] Items { get; set; }

        /// <summary>
        /// The enchantments this <see cref="Enchantment"/> can't enchant with.
        /// </summary>
        [JsonPropertyName("incompatible")]
        public string[] Incompatible { get; set; }

        /// <inheritdoc/>
        [JsonIgnore]
        public long Experience => (long)Level * Weight;

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

        public static Enchantment Deserialize(JsonProperty token)
        {
            Enchantment enchantment = token.Value.Deserialize(SourceGenerationContext.Default.Enchantment) ?? new Enchantment();
            enchantment.Name = token.Name;
            return enchantment;
        }

        [JsonSerializable(typeof(Enchantment))]
        private sealed partial class SourceGenerationContext : JsonSerializerContext;
    }
}
