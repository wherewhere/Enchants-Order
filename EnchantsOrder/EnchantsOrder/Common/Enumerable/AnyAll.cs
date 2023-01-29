﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NET20 || SILVERLIGHT || WINDOWSPHONE
using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    internal static partial class Enumerable
    {
        public static bool Any<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ICollection<TSource> collectionoft)
            {
                return collectionoft.Count != 0;
            }
            else if (source is IIListProvider<TSource> listProv)
            {
                // Note that this check differs from the corresponding check in
                // Count (whereas otherwise this method parallels it).  If the count
                // can't be retrieved cheaply, that likely means we'd need to iterate
                // through the entire sequence in order to get the count, and in that
                // case, we'll generally be better off falling through to the logic
                // below that only enumerates at most a single element.
                int count = listProv.GetCount(onlyIfCheap: true);
                if (count >= 0)
                {
                    return count != 0;
                }
            }
            else if (source is ICollection collection)
            {
                return collection.Count != 0;
            }

            using IEnumerator<TSource> e = source.GetEnumerator();
            return e.MoveNext();
        }
    }
}
#endif