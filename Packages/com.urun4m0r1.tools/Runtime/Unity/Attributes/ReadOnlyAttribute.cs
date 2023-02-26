using System;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ReadOnlyAttribute : PropertyAttribute
    {
        public readonly bool RuntimeOnly;
        public ReadOnlyAttribute(bool runtimeOnly = false) => RuntimeOnly = runtimeOnly;
    }
}
