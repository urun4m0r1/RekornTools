#nullable enable

using System;

namespace Urun4m0r1.RekornTools.Data
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ParseAsAttribute : Attribute
    {
        public string Name { get; }

        public ParseAsAttribute(string? name) => Name = name;
    }
}
