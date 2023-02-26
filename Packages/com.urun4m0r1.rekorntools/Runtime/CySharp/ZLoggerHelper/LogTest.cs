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
        private static ILogger<LogTest> s_logger => LogManager.GetLogger<LogTest>();

        private void OnEnable()
        {
            s_logger.ZLogError(nameof(OnEnable));
        }
    }
}
