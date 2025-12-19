// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET20
using System.Collections.Generic;

namespace System.Linq
{
    internal static partial class Enumerable
    {
        /// <summary>
        /// Returns the minimum value in a sequence of <see cref="int"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int"/> values to determine the minimum value of.</param>
        /// <returns>The minimum value in the sequence.</returns>
        public static int Min(this IEnumerable<int> source) => MinInteger(source);

        private static int MinInteger(this IEnumerable<int> source)
        {
            int value;

            using (IEnumerator<int> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new Exception("NoElementsException");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    int x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }
    }
}
#endif