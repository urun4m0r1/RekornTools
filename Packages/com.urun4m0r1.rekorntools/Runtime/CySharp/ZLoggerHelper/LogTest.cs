#nullable enable

using Microsoft.Extensions.Logging;
using UnityEngine;
using Urun4m0r1.RekornTools.ZLoggerHelper;
using ZLogger;

namespace Rekorn
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public sealed class LogTest : MonoBehaviour
    {
        private static ILogger<LogTest> Logger => Log.Create<LogTest>();

        private void OnEnable()
        {
            Logger.ZLogError(nameof(OnEnable));
        }
    }
}
