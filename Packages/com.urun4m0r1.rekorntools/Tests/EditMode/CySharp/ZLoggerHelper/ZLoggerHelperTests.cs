#nullable enable

using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Urun4m0r1.RekornTools.ZLoggerHelper;
using ZLogger;

namespace Urun4m0r1.RekornTools.Tests.EditMode
{
    public sealed class ZLoggerHelperTests
    {
        private static readonly ILogger<ZLoggerHelperTests> s_logger = LogManager.Create<ZLoggerHelperTests>();

        [Test]
        public void Static_Logger_Will_Log()
        {
            LogManager.Global.ZLogDebug(nameof(Static_Logger_Will_Log));
        }

        [Test]
        public void Class_Logger_Will_Log()
        {
            s_logger.ZLogDebug(nameof(Class_Logger_Will_Log));
        }
    }
}
