// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET20 || SILVERLIGHT || WINDOWSPHONE
using System.Collections.Generic;

namespace System.Linq
{
    internal static partial class Enumerable
    {
        public static int Min(this IEnumerable<int> source) => MinInteger(source);

        public static int? Min(this IEnumerable<int?> source) => MinInteger(source);

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

        private static int? MinInteger(this IEnumerable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int? value = null;
            using (IEnumerator<int?> e = source.GetEnumerator())
            {
                // Start off knowing that we have a non-null value (or exit here, knowing we don't)
                // so we don't have to keep testing for nullity.
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                // Keep hold of the wrapped value, and do comparisons on that, rather than
                // using the lifted operation each time.
                int valueVal = value.GetValueOrDefault();
                while (e.MoveNext())
                {
                    int? cur = e.Current;
                    int x = cur.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.HasValue & x < valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        public static long Min(this IEnumerable<long> source) => MinLong(source);

        public static long? Min(this IEnumerable<long?> source) => MinLong(source);

        private static long MinLong(this IEnumerable<long> source)
        {
            long value;

            using (IEnumerator<long> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new Exception("NoElementsException");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    long x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        private static long? MinLong(this IEnumerable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            long? value = null;
            using (IEnumerator<long?> e = source.GetEnumerator())
            {
                // Start off knowing that we have a non-null value (or exit here, knowing we don't)
                // so we don't have to keep testing for nullity.
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                // Keep hold of the wrapped value, and do comparisons on that, rather than
                // using the lifted operation each time.
                long valueVal = value.GetValueOrDefault();
                while (e.MoveNext())
                {
                    long? cur = e.Current;
                    long x = cur.GetValueOrDefault();

                    // Do not replace & with &&. The branch prediction cost outweighs the extra operation
                    // unless nulls either never happen or always happen.
                    if (cur.HasValue & x < valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        public static float Min(this IEnumerable<float> source) => MinFloat(source);

        public static float? Min(this IEnumerable<float?> source) => MinFloat(source);

        private static float MinFloat(this IEnumerable<float> source)
        {
            float value;

            using (IEnumerator<float> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new Exception("NoElementsException");
                }

                value = e.Current;
                if (float.IsNaN(value))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    float x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }

                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    // Not testing for NaN therefore isn't an option, but since we
                    // can't find a smaller value, we can short-circuit.
                    else if (float.IsNaN(x))
                    {
                        return x;
                    }
                }
            }

            return value;
        }

        private static float? MinFloat(this IEnumerable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            float? value = null;
            using (IEnumerator<float?> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                float valueVal = value.GetValueOrDefault();
                if (float.IsNaN(valueVal))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    float? cur = e.Current;
                    if (cur.HasValue)
                    {
                        float x = cur.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                        else if (float.IsNaN(x))
                        {
                            return cur;
                        }
                    }
                }
            }

            return value;
        }

        public static double Min(this IEnumerable<double> source) => MinDouble(source);

        public static double? Min(this IEnumerable<double?> source) => MinDouble(source);

        private static double MinDouble(this IEnumerable<double> source)
        {
            double value;

            using (IEnumerator<double> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new Exception("NoElementsException");
                }

                value = e.Current;
                if (double.IsNaN(value))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    double x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }

                    // Normally NaN < anything is false, as is anything < NaN
                    // However, this leads to some irksome outcomes in Min and Max.
                    // If we use those semantics then Min(NaN, 5.0) is NaN, but
                    // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                    // ordering where NaN is smaller than every value, including
                    // negative infinity.
                    // Not testing for NaN therefore isn't an option, but since we
                    // can't find a smaller value, we can short-circuit.
                    else if (double.IsNaN(x))
                    {
                        return x;
                    }
                }
            }

            return value;
        }

        private static double? MinDouble(this IEnumerable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double? value = null;
            using (IEnumerator<double?> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                double valueVal = value.GetValueOrDefault();
                if (double.IsNaN(valueVal))
                {
                    return value;
                }

                while (e.MoveNext())
                {
                    double? cur = e.Current;
                    if (cur.HasValue)
                    {
                        double x = cur.GetValueOrDefault();
                        if (x < valueVal)
                        {
                            valueVal = x;
                            value = cur;
                        }
                        else if (double.IsNaN(x))
                        {
                            return cur;
                        }
                    }
                }
            }

            return value;
        }

        public static decimal Min(this IEnumerable<decimal> source)
        {
            decimal value;

            using (IEnumerator<decimal> e = source.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    throw new Exception("NoElementsException");
                }

                value = e.Current;
                while (e.MoveNext())
                {
                    decimal x = e.Current;
                    if (x < value)
                    {
                        value = x;
                    }
                }
            }

            return value;
        }

        public static decimal? Min(this IEnumerable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            decimal? value = null;
            using (IEnumerator<decimal?> e = source.GetEnumerator())
            {
                do
                {
                    if (!e.MoveNext())
                    {
                        return value;
                    }

                    value = e.Current;
                }
                while (!value.HasValue);

                decimal valueVal = value.GetValueOrDefault();
                while (e.MoveNext())
                {
                    decimal? cur = e.Current;
                    decimal x = cur.GetValueOrDefault();
                    if (cur.HasValue && x < valueVal)
                    {
                        valueVal = x;
                        value = cur;
                    }
                }
            }

            return value;
        }

        public static TSource Min<TSource>(this IEnumerable<TSource> source) => Min(source, comparer: null);

        /// <summary>Returns the minimum value in a generic sequence.</summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="comparer">The <see cref="IComparer{T}" /> to compare values.</param>
        /// <returns>The minimum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">No object in <paramref name="source" /> implements the <see cref="System.IComparable" /> or <see cref="System.IComparable{T}" /> interface.</exception>
        /// <remarks>
        /// <para>If type <typeparamref name="TSource" /> implements <see cref="System.IComparable{T}" />, the <see cref="Max{T}(IEnumerable{T})" /> method uses that implementation to compare values. Otherwise, if type <typeparamref name="TSource" /> implements <see cref="System.IComparable" />, that implementation is used to compare values.</para>
        /// <para>If <typeparamref name="TSource" /> is a reference type and the source sequence is empty or contains only values that are <see langword="null" />, this method returns <see langword="null" />.</para>
        /// <para>In Visual Basic query expression syntax, an `Aggregate Into Max()` clause translates to an invocation of <see cref="O:Enumerable.Max" />.</para>
        /// </remarks>
        public static TSource Min<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            comparer ??= Comparer<TSource>.Default;

            TSource value = default;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                if (value == null)
                {
                    do
                    {
                        if (!e.MoveNext())
                        {
                            return value;
                        }

                        value = e.Current;
                    }
                    while (value == null);

                    while (e.MoveNext())
                    {
                        TSource next = e.Current;
                        if (next != null && comparer.Compare(next, value) < 0)
                        {
                            value = next;
                        }
                    }
                }
                else
                {
                    if (!e.MoveNext())
                    {
                        throw new Exception("NoElementsException");
                    }

                    value = e.Current;
                    if (comparer == Comparer<TSource>.Default)
                    {
                        while (e.MoveNext())
                        {
                            TSource next = e.Current;
                            if (Comparer<TSource>.Default.Compare(next, value) < 0)
                            {
                                value = next;
                            }
                        }
                    }
                    else
                    {
                        while (e.MoveNext())
                        {
                            TSource next = e.Current;
                            if (comparer.Compare(next, value) < 0)
                            {
                                value = next;
                            }
                        }
                    }
                }
            }

            return value;
        }
    }
}
#endif