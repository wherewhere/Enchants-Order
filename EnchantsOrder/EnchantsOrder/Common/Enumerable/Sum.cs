// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET20 || SILVERLIGHT || WINDOWSPHONE
using System.Collections.Generic;

namespace System.Linq
{
    internal static partial class Enumerable
    {
        public static int Sum(this IEnumerable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int sum = 0;
            checked
            {
                foreach (int v in source)
                {
                    sum += v;
                }
            }
            return sum;
        }

        public static int? Sum(this IEnumerable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int sum = 0;
            checked
            {
                foreach (int? v in source)
                {
                    if (v != null)
                    {
                        sum += v.GetValueOrDefault();
                    }
                }
            }
            return sum;
        }

        public static long Sum(this IEnumerable<long> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            long sum = 0;
            checked
            {
                foreach (long v in source)
                {
                    sum += v;
                }
            }
            return sum;
        }

        public static long? Sum(this IEnumerable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            long sum = 0;
            checked
            {
                foreach (long? v in source)
                {
                    if (v != null)
                    {
                        sum += v.GetValueOrDefault();
                    }
                }
            }
            return sum;
        }

        public static float Sum(this IEnumerable<float> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double sum = 0;
            foreach (float v in source)
            {
                sum += v;
            }

            return (float)sum;
        }

        public static float? Sum(this IEnumerable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double sum = 0;
            foreach (float? v in source)
            {
                if (v != null)
                {
                    sum += v.GetValueOrDefault();
                }
            }
            return (float)sum;
        }

        public static double Sum(this IEnumerable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double sum = 0;
            foreach (double v in source)
            {
                sum += v;
            }

            return sum;
        }

        public static double? Sum(this IEnumerable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            double sum = 0;
            foreach (double? v in source)
            {
                if (v != null)
                {
                    sum += v.GetValueOrDefault();
                }
            }
            return sum;
        }

        public static decimal Sum(this IEnumerable<decimal> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            decimal sum = 0;
            foreach (decimal v in source)
            {
                sum += v;
            }

            return sum;
        }

        public static decimal? Sum(this IEnumerable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            decimal sum = 0;
            foreach (decimal? v in source)
            {
                if (v != null)
                {
                    sum += v.GetValueOrDefault();
                }
            }
            return sum;
        }

    }
}
#endif