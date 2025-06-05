// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET20 || SILVERLIGHT || WINDOWSPHONE
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    internal static partial class Enumerable
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <returns>The number of elements in the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <see langword="null"/>.</exception>
        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ICollection<TSource> collectionoft)
            {
                return collectionoft.Count;
            }

            if (source is IIListProvider<TSource> listProv)
            {
                return listProv.GetCount(onlyIfCheap: false);
            }

            if (source is ICollection collection)
            {
                return collection.Count;
            }

            int count = 0;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                checked
                {
                    while (e.MoveNext())
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
#endif