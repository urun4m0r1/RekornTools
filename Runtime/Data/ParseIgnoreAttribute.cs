using System;

namespace Rekorn.Tools.Data
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ParseIgnoreAttribute : Attribute
    {
        public ParseIgnoreAttribute() { }
    }
}
