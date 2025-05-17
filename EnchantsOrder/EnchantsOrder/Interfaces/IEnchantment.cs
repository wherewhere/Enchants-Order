#if !WINRT
using System;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// A value of enchantment.
    /// </summary>
    public interface IEnchantment
#if !WINRT
        : IComparable<IEnchantment>, IEquatable<IEnchantment>
#endif
    {
        /// <summary>
        /// The name of this enchantment.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The level of this enchantment.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// The weight for enchant of this enchantment.
        /// </summary>
        int Weight { get; }

        /// <summary>
        /// The experience level when enchant request of this enchantment.
        /// </summary>
        long Experience { get; }

#if WINRT
        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with this instance.</param>
        /// <returns>
        ///   <para>A value that indicates the relative order of the objects being compared. The return value has these meanings:</para>
        ///   <list type="table">
        ///     <listheader>
        ///       <term>Value</term>
        ///       <description>Meaning</description>
        ///     </listheader>
        ///     <item>
        ///       <term>Less than zero</term>
        ///       <description>This instance precedes <paramref name="other" /> in the sort order.</description>
        ///     </item>
        ///     <item>
        ///       <term>Zero</term>
        ///       <description>This instance occurs in the same position in the sort order as <paramref name="other" />.</description>
        ///     </item>
        ///     <item>
        ///       <term>Greater than zero</term>
        ///       <description>This instance follows <paramref name="other" /> in the sort order.</description>
        ///     </item>
        ///   </list>
        /// </returns>
        int CompareTo(IEnchantment other);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
        bool Equals(IEnchantment other);
#endif
    }
}
