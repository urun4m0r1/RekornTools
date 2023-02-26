#nullable enable

using Rekorn.Tools.DesignPatterns;
using Rekorn.Tools.Utils;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    public sealed class YieldCache : Singleton<YieldCache>
    {
        YieldCache() { }

        public readonly Cache<float, WaitForSeconds>         WaitForSeconds         = new(f => new WaitForSeconds(f));
        public readonly Cache<float, WaitForSecondsRealtime> WaitForSecondsRealtime = new(f => new WaitForSecondsRealtime(f));
        public readonly WaitForFixedUpdate                   WaitForFixedUpdate     = new();
        public readonly WaitForEndOfFrame                    WaitForEndOfFrame      = new();

        public void Clear()
        {
            WaitForSeconds.Clear();
            WaitForSecondsRealtime.Clear();
        }
    }
}
