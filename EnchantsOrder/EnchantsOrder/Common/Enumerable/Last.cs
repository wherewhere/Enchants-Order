// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET20 || SILVERLIGHT || WINDOWSPHONE
using System.Collections.Generic;

namespace System.Linq
{
    internal static partial class Enumerable
    {
        /// <summary>
        /// Returns the last element of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> to return the last element of.</param>
        /// <returns>The value at the last position in the source sequence.</returns>
        public static TSource Last<TSource>(this IEnumerable<TSource> source)
        {
            TSource? last = source.TryGetLast(out bool found);
            return !found ? throw new Exception("NoElementsException") : last!;
        }

        private static TSource? TryGetLast<TSource>(this IEnumerable<TSource> source, out bool found)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is IPartition<TSource> partition)
            {
                return partition.TryGetLast(out found);
            }

            if (source is IList<TSource> list)
            {
                int count = list.Count;
                if (count > 0)
                {
                    found = true;
                    return list[count - 1];
                }
            }
            else
            {
                using IEnumerator<TSource> e = source.GetEnumerator();
                if (e.MoveNext())
                {
                    TSource result;
                    do
                    {
                        result = e.Current;
                    }
                    while (e.MoveNext());

                    found = true;
                    return result;
                }
            }

            found = false;
            return default;
        }
    }
}
#endif