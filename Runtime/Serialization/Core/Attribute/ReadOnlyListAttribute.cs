#nullable enable

using System;
using JetBrains.Annotations;

namespace Rekorn.Tools.Serialization
{
    /// <inheritdoc />
    [UsedImplicitly]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class ReadOnlyListAttribute : Attribute { }
}
