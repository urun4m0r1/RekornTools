#nullable enable

using System;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [Flags] public enum Limiter
    {
        None       = 0,
        Time       = 1 << 0,
        Iterations = 1 << 1,
    }

    [Serializable]
    public sealed class CoroutineLimiter
    {
        [field: SerializeField] public Limiter Limit      { get; set; }
        [field: SerializeField] public float   Time       { get; set; }
        [field: SerializeField] public int     Iterations { get; set; }

        public bool IsLimitReached(int iterations, float time)
        {
            if (Limit is Limiter.None) return false;

            var isIterationsLimited = false;
            var isTimeLimited       = false;

            if (Limit.HasFlag(Limiter.Iterations))
                isIterationsLimited = iterations >= Iterations;
            if (Limit.HasFlag(Limiter.Time))
                isTimeLimited = time >= Time;

            return isIterationsLimited || isTimeLimited;
        }
    }
}
