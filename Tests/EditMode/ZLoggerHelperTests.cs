#nullable enable

using Microsoft.Extensions.Logging;
using NUnit.Framework;
using ZLogger;
using static Rekorn.Tools.ZLoggerHelper.LogManager;

namespace Rekorn.Tools.Tests.EditMode
{
    public sealed class ZLoggerHelperTests
    {
        private static readonly ILogger<ZLoggerHelperTests> s_logger = GetLogger<ZLoggerHelperTests>();

        [Test]
        public void Static_Logger_Will_Log()
        {
            Logger.ZLogDebug(nameof(Static_Logger_Will_Log));
        }

        [Test]
        public void Class_Logger_Will_Log()
        {
            s_logger.ZLogDebug(nameof(Class_Logger_Will_Log));
        }
    }
}
