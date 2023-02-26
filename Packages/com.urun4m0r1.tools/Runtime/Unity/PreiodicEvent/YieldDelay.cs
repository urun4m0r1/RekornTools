#nullable enable

using System;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
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

        private readonly WaitUntil _waitUntil;

        public YieldDelay() => _waitUntil = new WaitUntil(() => Continue);

        //중간에 타입 바꾸면 null 리턴하게 바꾸기

        public object? WaitForDelay =>
            Type switch
            {
                Delay.Immediately     => null,
                Delay.Manually        => _waitUntil,
                Delay.Seconds         => YieldCache.Instance.WaitForSeconds.Request(Value),
                Delay.SecondsRealtime => YieldCache.Instance.WaitForSecondsRealtime.Request(Value),
                Delay.FixedUpdate     => YieldCache.Instance.WaitForFixedUpdate,
                Delay.EndOfFrame      => YieldCache.Instance.WaitForEndOfFrame,
                _                     => throw new ArgumentOutOfRangeException(),
            };
    }
}
