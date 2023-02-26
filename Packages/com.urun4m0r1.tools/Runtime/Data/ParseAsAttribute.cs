using System;

namespace Rekorn.Tools.Data
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ParseAsAttribute : Attribute
    {
        public string Name { get; }

        public ParseAsAttribute(string name) => Name = name;
    }
}
