﻿#if NET20 || SILVERLIGHT || WINDOWSPHONE
using System.Collections.Generic;

namespace System.Linq
{
    internal static partial class Enumerable
    {
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            List<TSource> list = source.ToList();
            TSource[] builder = new TSource[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                TSource item = list[i];
                builder[i] = item;
            }

            return builder;
        }

        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            return source == null ? throw new ArgumentNullException(nameof(source)) : new List<TSource>(source);
        }
    }
}
#endif