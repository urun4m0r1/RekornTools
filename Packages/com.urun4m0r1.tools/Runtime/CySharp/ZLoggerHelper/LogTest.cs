#nullable enable

using Microsoft.Extensions.Logging;
using Rekorn.Tools.ZLoggerHelper;
using UnityEngine;
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
