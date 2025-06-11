// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if SILVERLIGHT3_0 || WINDOWSPHONE7_0
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Specifies a destination <see cref="Type"/> in another assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class TypeForwardedToAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeForwardedToAttribute"/> class specifying a destination <see cref="Type"/>.
        /// </summary>
        /// <param name="destination">The destination <see cref="Type"/> in another assembly.</param>
        public TypeForwardedToAttribute(Type destination)
        {
            Destination = destination;
        }

        /// <summary>
        /// Gets the destination <see cref="Type"/> in another assembly.
        /// </summary>
        public Type Destination { get; }
    }
}
#endif