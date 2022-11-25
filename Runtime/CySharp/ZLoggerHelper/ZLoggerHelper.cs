#nullable enable

using System;
using Cysharp.Text;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Rekorn.Tools.ZLoggerHelper
{
    public static class LogManager
    {
        private static readonly string s_logFilePath       = $"{UnityEngine.Application.persistentDataPath}/Logs";
        private static readonly string s_logFileName       = "application";
        private static readonly string s_logFileExtension  = "log";
        private static readonly int    s_logFileRollSizeKB = 1024;

        private static readonly LogLevel s_logMinimumLevel = LogLevel.Trace;

        private static readonly string s_globalLogCategory = "Global";
        private static readonly string s_logPrefixFormat   = "<b>[{0}]</b>";
        private static readonly string s_logSuffixFormat   = "\n\n[{0}] ({1}) {2}";

        private static readonly bool s_isFileLogEnabled        = true;
        private static readonly bool s_isRollingFileLogEnabled = true;

        private static readonly ILogger        s_globalLogger;
        private static readonly ILoggerFactory s_loggerFactory;

        private static string GetFileUrl(string fileName)
        {
            return ZString.Format("{0}/{1}.{2}", s_logFilePath, fileName, s_logFileExtension);
        }

        private static string GetFileName(DateTimeOffset dateTimeOffset, int index)
        {
            var yyyy = dateTimeOffset.Year;
            var mm   = dateTimeOffset.Month;
            var dd   = dateTimeOffset.Day;
            return ZString.Format("{0:D4}-{1:D2}-{2:D2}_{3:D3}", yyyy, mm, dd, index);
        }

        private static Action<ZLoggerOptions> ConfigureLog() => static x =>
        {
            x.PrefixFormatter = static (writer, info) =>
            {
                var category = info.CategoryName;
                ZString.Utf8Format(writer, s_logPrefixFormat, category);
            };

            x.SuffixFormatter = static (writer, info) =>
            {
                var level   = info.LogLevel.ToString();
                var eventId = info.EventId.ToString();
                var dateTime = info.Timestamp.ToLocalTime().DateTime;
                ZString.Utf8Format(writer, s_logSuffixFormat, level, eventId, dateTime);
            };
        };

        private static readonly Action<ZLoggerOptions> s_configureLog = ConfigureLog();

        static LogManager()
        {
            // Standard LoggerFactory does not work on IL2CPP,
            // But you can use ZLogger's UnityLoggerFactory instead,
            // it works on IL2CPP, all platforms(includes mobile).
            s_loggerFactory = UnityLoggerFactory.Create(static builder =>
            {
                // LogLevels are translate to
                // * Trace/Debug/Information -> LogType.Log
                // * Warning/Critical        -> LogType.Warning
                // * Error without Exception -> LogType.Error
                // * Error with Exception    -> LogException

                // For more configuration, you can use builder.AddFilter
                // builder.AddFilter(static (category, level) => true);
                builder.SetMinimumLevel(s_logMinimumLevel);

                // AddZLoggerUnityDebug is only available for Unity, it send log to UnityEngine.Debug.Log.
                builder.AddZLoggerUnityDebug(s_configureLog);

                if (s_isFileLogEnabled)
                    builder.AddZLoggerFile(GetFileUrl(s_logFileName), s_configureLog);

                if (s_isRollingFileLogEnabled)
                    builder.AddZLoggerRollingFile(fileNameSelector: static (dt, i) => GetFileUrl(GetFileName(dt, i)),
                                                  timestampPattern: static t => t.ToLocalTime().Date,
                                                  rollSizeKB: s_logFileRollSizeKB,
                                                  configure: s_configureLog);
            })!;

            s_globalLogger = s_loggerFactory.CreateLogger(s_globalLogCategory);

            UnityEngine.Application.quitting += static () => s_loggerFactory.Dispose();
        }

        public static ILogger Logger => s_globalLogger;

        public static ILogger<T> GetLogger<T>() where T : class => s_loggerFactory.CreateLogger<T>();
        public static ILogger    GetLogger(string categoryName) => s_loggerFactory.CreateLogger(categoryName);
    }
}
