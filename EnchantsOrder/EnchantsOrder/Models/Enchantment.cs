using EnchantsOrder.Common;

#if WINRT
using Windows.Foundation;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// A value of enchantment.
    /// </summary>
    public
#if WINRT
        sealed
#endif
        class Enchantment : IEnchantment
#if WINRT
        , IStringable
#endif
    {
        /// <summary>
        /// The name of this enchantment.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The level of this enchantment.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The weight for enchant of this enchantment.
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// The experience level when enchant request of this enchantment.
        /// </summary>
        public long Experience => (long)Level * Weight;

        /// <summary>
        /// Initializes a new instance of <see cref="Enchantment" />.
        /// </summary>
        public Enchantment(string name, int level, int weight)
        {
            Name = name;
            Level = level;
            Weight = weight;
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Name} {Level.GetLoumaNumber()}";

        /// <inheritdoc/>
        public int CompareTo(IEnchantment other)
        {
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
