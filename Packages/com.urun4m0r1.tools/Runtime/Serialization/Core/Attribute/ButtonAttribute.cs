#nullable enable

using System;
using JetBrains.Annotations;

namespace Rekorn.Tools.Serialization
{
    /// <inheritdoc />
    [UsedImplicitly]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ButtonAttribute : Attribute { }
}
