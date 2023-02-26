using System;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Math
{
    [Serializable]
    public sealed class FloatRange
    {
        [field: SerializeField] public float Min { get; set; }
        [field: SerializeField] public float Max { get; set; }

        public FloatRange() { }

        public FloatRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public static implicit operator FloatRange(Vector2 v)
        {
            return new FloatRange(v.x, v.y);
        }

        public float Random()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }

    [Serializable]
    public sealed class IntRange
    {
        [field: SerializeField] public int Min { get; set; }
        [field: SerializeField] public int Max { get; set; }

        public IntRange() { }

        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public static implicit operator IntRange(Vector2Int v)
        {
            return new IntRange(v.x, v.y);
        }

        public int Random()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}
