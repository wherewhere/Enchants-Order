using System.Collections.Generic;

#if !WINRT
using System;
#endif

namespace EnchantsOrder.Models
{
    /// <summary>
    /// The result of ordering.
    /// </summary>
    public interface IOrderingResults
#if !WINRT
        : IComparable<IOrderingResults>
#endif
    {
        /// <summary>
        /// The penalty of item.
        /// </summary>
        int Penalty { get; }

        /// <summary>
        /// The max experience level request during enchant.
        /// </summary>
        double MaxExperience { get; }

        /// <summary>
        /// The total experience level request during enchant.
        /// </summary>
        double TotalExperience { get; }

        /// <summary>
        /// The steps of enchant.
        /// </summary>
        IList<IEnchantmentStep> Steps { get; }

        /// <summary>
        /// Too expensive because max experience level max than 39.
        /// </summary>
        bool IsTooExpensive { get; }

        /// <summary>
        /// Max penalty max than 6 so that you cannot enchant any more.
        /// </summary>
        bool IsTooManyPenalty { get; }

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
        int CompareTo(IOrderingResults other);
#endif
    }
}
