using EnchantsOrder.Common;
using System;
using System.Diagnostics.CodeAnalysis;

#if WINRT
using Windows.Foundation;
using Windows.Foundation.Metadata;
#else
using System.Collections.Generic;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The information of enchantment, which is used to enchant item.
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
#else
        , IEquatable<IEnchantment>
#endif
#if NET7_0_OR_GREATER
        , System.Numerics.IComparisonOperators<Enchantment, Enchantment, bool>
#endif
    {
        /// <inheritdoc/>
        public string Name => name;

        /// <inheritdoc/>
        public int Level => level;

        /// <inheritdoc/>
        public int Weight => weight;

        /// <inheritdoc/>
        public long Experience => (long)level * weight;

        /// <inheritdoc/>
        public override string ToString() => $"{name} {level.GetRomanNumber()}";

        /// <inheritdoc/>
        public int CompareTo(IEnchantment? other)
        {
            if (other == null) { return -1; }
            int value = Experience.CompareTo(other.Experience);
            if (value == 0)
            {
                value = level.CompareTo(other.Level);
                if (value == 0)
                {
                    value = name.CompareTo(other.Name);
                }
            }
            return value;
        }

#if WINRT
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
        [DefaultOverload]
        [Overload(nameof(Equals))]
#else
        /// <inheritdoc/>
#endif
        public bool Equals([NotNullWhen(true)] IEnchantment? other) => CompareTo(other) == 0;

        /// <inheritdoc/>
#if WINRT
        [Overload("EqualsToObject")]
#endif
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is Enchantment enchantment && Equals(enchantment);

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(Name, Level, Weight);

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
        /// <summary>
        /// Determines whether one <see cref="Enchantment"/> instance is greater than another instance.
        /// </summary>
        /// <param name="left">The first <see cref="Enchantment"/> to compare.</param>
        /// <param name="right">The second <see cref="Enchantment"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >(Enchantment left, Enchantment right) => left.CompareTo(right) == 1;

        /// <summary>
        /// Determines whether one <see cref="Enchantment"/> instance is greater than or equal to another instance.
        /// </summary>
        /// <param name="left">The first <see cref="Enchantment"/> to compare.</param>
        /// <param name="right">The second <see cref="Enchantment"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator >=(Enchantment left, Enchantment right) => left.CompareTo(right) != -1;

        /// <summary>
        /// Determines whether two specified <see cref="Enchantment"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="Enchantment"/> to compare.</param>
        /// <param name="right">The second <see cref="Enchantment"/> to compare.</param>
        /// <returns><see langword="true"/> if the two <see cref="Enchantment"/> instances are equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Enchantment? left, Enchantment? right) => EqualityComparer<Enchantment?>.Default.Equals(left, right);

        /// <summary>
        /// Determines whether two <see cref="Enchantment"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="Enchantment"/> to compare.</param>
        /// <param name="right">The second <see cref="Enchantment"/> to compare.</param>
        /// <returns><see langword="true"/> if the specified <see cref="Enchantment"/> instances are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Enchantment? left, Enchantment? right) => !(left == right);

        /// <summary>
        /// Determines whether one <see cref="Enchantment"/> instance is less than another instance.
        /// </summary>
        /// <param name="left">The first <see cref="Enchantment"/> to compare.</param>
        /// <param name="right">The second <see cref="Enchantment"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <(Enchantment left, Enchantment right) => left.CompareTo(right) == -1;

        /// <summary>
        /// Determines whether one <see cref="Enchantment"/> is less than or equal to another Enchantment.
        /// </summary>
        /// <param name="left">The first <see cref="Enchantment"/> to compare.</param>
        /// <param name="right">The second <see cref="Enchantment"/> to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator <=(Enchantment left, Enchantment right) => left.CompareTo(right) != 1;
#endif
    }
}
