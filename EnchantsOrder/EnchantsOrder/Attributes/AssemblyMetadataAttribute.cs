// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#if SILVERLIGHT || WINDOWSPHONE7_0
namespace System.Reflection
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    internal sealed class AssemblyMetadataAttribute(string key, string value) : Attribute
    {
        public string Key { get; } = key;
        public string Value { get; } = value;
    }
}
#endif