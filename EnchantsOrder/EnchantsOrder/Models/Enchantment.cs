using EnchantsOrder.Common;
using System.Diagnostics.CodeAnalysis;

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
#if WINRT
    sealed
#endif
    public class Enchantment(string name, int level, int weight) : IEnchantment
#if WINRT
        , IStringable
#endif
    {
        /// <summary>
        /// Gets or sets the name of this enchantment.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets or sets the level of this enchantment.
        /// </summary>
        public int Level => level;

        /// <summary>
        /// Gets or sets the weight for enchant of this enchantment.
        /// </summary>
        public int Weight => weight;

        /// <summary>
        /// Gets or sets the experience level when enchant request of this enchantment.
        /// </summary>
        public long Experience => (long)Level * Weight;

        /// <inheritdoc/>
        public override string ToString() => $"{Name} {Level.GetRomanNumber()}";

        /// <inheritdoc/>
        public int CompareTo(IEnchantment? other)
        {
            if (other == null) { return -1; }
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
        public bool Equals([NotNullWhen(true)] IEnchantment? other) => CompareTo(other) == 0;

        /// <summary>
        /// Deconstructs the object into its component properties.
        /// </summary>
        /// <param name="name">The name associated with the object.</param>
        /// <param name="level">The level value of the object.</param>
        /// <param name="weight">The weight value of the object.</param>
        public void Deconstruct(out string name, out int level, out int weight)
        {
            name = Name;
            level = Level;
            weight = Weight;
        }

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
