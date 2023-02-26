#nullable enable

using System;
using JetBrains.Annotations;

namespace Urun4m0r1.RekornTools.Serialization
{
    /// <inheritdoc />
    [UsedImplicitly]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ReadOnlyListAttribute : Attribute { }
}
