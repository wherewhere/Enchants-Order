using EnchantsOrder.Common;

#if WINRT
using Windows.Foundation;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// A value of enchantment.
    /// </summary>
    /// <param name="name">The name of this enchantment.</param>
    /// <param name="level">The level of this enchantment.</param>
    /// <param name="weight">The weight for enchant of this enchantment.</param>
    public
#if WINRT
        sealed
#endif
        class Enchantment(string name, int level, int weight) : IEnchantment
#if WINRT
        , IStringable
#endif
    {
        /// <summary>
        /// Gets or sets the name of this enchantment.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// Gets or sets the level of this enchantment.
        /// </summary>
        public int Level { get; set; } = level;

        /// <summary>
        /// Gets or sets the weight for enchant of this enchantment.
        /// </summary>
        public int Weight { get; set; } = weight;

        /// <summary>
        /// Gets or sets the experience level when enchant request of this enchantment.
        /// </summary>
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

#if !WINRT
        /// <inheritdoc/>
        public static bool operator >(Enchantment left, Enchantment right) => left.CompareTo(right) == 1;

        /// <inheritdoc/>
        public static bool operator >=(Enchantment left, Enchantment right) => left.CompareTo(right) != -1;

        /// <inheritdoc/>
        public static bool operator <(Enchantment left, Enchantment right) => left.CompareTo(right) == -1;

        /// <inheritdoc/>
        public static bool operator <=(Enchantment left, Enchantment right) => left.CompareTo(right) != 1;
#endif
    }
}
