// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET47_OR_GREATER && !NETCOREAPP && !NETSTANDARD2_0_OR_GREATER
using System.Runtime.InteropServices;

namespace System
{
    /// <summary>
    /// Represents a 2-tuple, or pair, as a value type.
    /// </summary>
    /// <typeparam name="T1">The type of the tuple's first component.</typeparam>
    /// <typeparam name="T2">The type of the tuple's second component.</typeparam>
    [StructLayout(LayoutKind.Auto)]
    internal struct ValueTuple<T1, T2>;
}
#endif