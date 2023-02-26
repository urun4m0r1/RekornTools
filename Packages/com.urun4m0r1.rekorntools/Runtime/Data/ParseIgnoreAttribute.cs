#nullable enable

using System;

namespace Urun4m0r1.RekornTools.Data
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ParseIgnoreAttribute : Attribute
    {
        public ParseIgnoreAttribute() { }
    }
}
