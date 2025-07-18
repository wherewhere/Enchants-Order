﻿#if SILVERLIGHT || WINDOWSPHONE7_0 || (NETFRAMEWORK && !NET45_OR_GREATER)
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace System.Collections.Generic
{
    /// <summary>
    /// Represents a read-only collection of elements that can be accessed by index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the read-only list.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NET40_OR_GREATER
    internal interface IReadOnlyList<out T> : IReadOnlyCollection<T>
#else
    internal interface IReadOnlyList<T> : IReadOnlyCollection<T>
#endif
    {
        /// <summary>
        /// Gets the element at the specified index in the read-only list.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the read-only list.</returns>
        T this[int index] { get; }
    }
}
#else
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Collections.Generic.IReadOnlyList<>))]
#endif