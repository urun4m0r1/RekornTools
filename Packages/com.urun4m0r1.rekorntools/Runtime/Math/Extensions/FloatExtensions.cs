using UnityEngine;

namespace Urun4m0r1.RekornTools.Math
{
    public static class FloatExtensions
    {
        public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);
        public static float ClampMax(this float value, float max) => Mathf.Min(value, max);
        public static float ClampMin(this float value, float min) => Mathf.Max(value, min);
    }
}
