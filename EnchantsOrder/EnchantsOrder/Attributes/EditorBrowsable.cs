// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#if SILVERLIGHT || WINDOWSPHONE
using System.Diagnostics.CodeAnalysis;

namespace System.ComponentModel
{
    /// <summary>
    /// Specifies the browsable state of a property or method from within an editor.
    /// </summary>
    internal enum EditorBrowsableState
    {
        /// <summary>
        /// The property or method is always browsable from within an editor.
        /// </summary>
        Always,

        /// <summary>
        /// The property or method is never browsable from within an editor.
        /// </summary>
        Never,

        /// <summary>
        /// The property or method is a feature that only advanced users should see. An editor can either show or hide such properties.
        /// </summary>
        Advanced
    }

    /// <summary>
    /// Specifies that a class or member is viewable in an editor. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Delegate | AttributeTargets.Interface)]
    internal sealed class EditorBrowsableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorBrowsableAttribute"/> class with an <see cref="EditorBrowsableState"/>.
        /// </summary>
        /// <param name="state">The <see cref="EditorBrowsableAttribute"/> to set <see cref="State"/> to.</param>
        public EditorBrowsableAttribute(EditorBrowsableState state)
        {
            State = state;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorBrowsableAttribute"/> class with <see cref="State"/> set to the default state.
        /// </summary>
        public EditorBrowsableAttribute() : this(EditorBrowsableState.Always)
        {
        }

        /// <summary>
        /// Gets the browsable state of the property or method.
        /// </summary>
        public EditorBrowsableState State { get; }

        /// <summary>
        /// Returns whether the value of the given object is equal to the current <see cref="EditorBrowsableAttribute"/>.
        /// </summary>
        /// <param name="obj">The object to test the value equality of.</param>
        /// <returns><see langword="true"/> if the value of the given object is equal to that of the current; otherwise, <see langword="false"/>.</returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj == this)
            {
                return true;
            }

            return (obj is EditorBrowsableAttribute other) && other.State == State;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => base.GetHashCode();
    }
}
#endif