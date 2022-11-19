using System;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ReadOnlyAttribute : PropertyAttribute
    {
        public readonly bool RuntimeOnly;
        public ReadOnlyAttribute(bool runtimeOnly = false) => RuntimeOnly = runtimeOnly;
    }
}
