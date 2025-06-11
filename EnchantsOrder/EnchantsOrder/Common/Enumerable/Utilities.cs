// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET20
namespace System.Linq
{
    /// <summary>
    /// Contains helper methods for System.Linq. Please put enumerable-related methods in EnumerableHelpers/>.
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Combines two selectors.
        /// </summary>
        /// <typeparam name="TSource">The type of the first selector's argument.</typeparam>
        /// <typeparam name="TMiddle">The type of the second selector's argument.</typeparam>
        /// <typeparam name="TResult">The type of the second selector's return value.</typeparam>
        /// <param name="selector1">The first selector to run.</param>
        /// <param name="selector2">The second selector to run.</param>
        /// <returns>
        /// A new selector that represents the composition of the first selector with the second selector.
        /// </returns>
        public static Func<TSource, TResult> CombineSelectors<TSource, TMiddle, TResult>(Func<TSource, TMiddle> selector1, Func<TMiddle, TResult> selector2) =>
            x => selector2(selector1(x));
    }
}
#else
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Linq.Enumerable))]
#endif