#nullable enable

using System;
using UnityEngine;
using Urun4m0r1.RekornTools.DesignPatterns;
using Urun4m0r1.RekornTools.Utils;

namespace Urun4m0r1.RekornTools.Unity
{
    public sealed class YieldCache : Singleton<YieldCache>, IDisposable
    {
        private YieldCache() { }

        public readonly Cache<float, WaitForSeconds>         WaitForSeconds         = new(f => new WaitForSeconds(f));
        public readonly Cache<float, WaitForSecondsRealtime> WaitForSecondsRealtime = new(f => new WaitForSecondsRealtime(f));
        public readonly WaitForFixedUpdate                   WaitForFixedUpdate     = new();
        public readonly WaitForEndOfFrame                    WaitForEndOfFrame      = new();

        public void Dispose()
        {
            WaitForSeconds.Dispose();
            WaitForSecondsRealtime.Dispose();
        }
    }
}
