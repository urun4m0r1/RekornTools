#nullable enable

using UnityEngine;

namespace Urun4m0r1.RekornTools.Math
{
    public static class IntExtensions
    {
        public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);
        public static int ClampMax(this int value, int max) => Mathf.Min(value, max);
        public static int ClampMin(this int value, int min) => Mathf.Max(value, min);
    }
}
