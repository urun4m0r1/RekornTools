#nullable enable

using System;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    public enum Delay
    {
        Immediately,
        Manually,
        Seconds,
        SecondsRealtime,
        FixedUpdate,
        EndOfFrame,
    }

    [Serializable]
    public sealed class YieldDelay
    {
        [field: SerializeField] public Delay Type     { get; set; } = Delay.Manually;
        [field: SerializeField] public bool  Continue { get; set; } = true;
        [field: SerializeField] public float Value    { get; set; }

        readonly WaitUntil  _waitUntil;
        readonly YieldCache _cache = YieldCache.Instance;

        public YieldDelay() => _waitUntil = new WaitUntil(() => Continue);

        //중간에 타입 바꾸면 null 리턴하게 바꾸기

        public object? WaitForDelay =>
            Type switch
            {
                Delay.Immediately     => null,
                Delay.Manually        => _waitUntil,
                Delay.Seconds         => _cache.WaitForSeconds.Request(Value),
                Delay.SecondsRealtime => _cache.WaitForSecondsRealtime.Request(Value),
                Delay.FixedUpdate     => _cache.WaitForFixedUpdate,
                Delay.EndOfFrame      => _cache.WaitForEndOfFrame,
                _                     => throw new ArgumentOutOfRangeException(),
            };
    }
}
