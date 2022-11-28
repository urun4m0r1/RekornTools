#nullable enable

using System;
using Cysharp.Text;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Rekorn.Tools.ZLoggerHelper
{
    public static class LogManager
    {
        private static readonly ZLoggerHelperPreset s_preset;
        private static readonly ILogger             s_globalLogger;
        private static readonly ILoggerFactory      s_loggerFactory;

        private static string GetFileUrl(string? fileName)
        {
            return ZString.Concat(s_preset.LogFilePath, fileName, s_preset.LogFileExtension);
        }

        private static string GetFileName(DateTimeOffset dateTimeOffset, int index)
        {
            var yyyy = dateTimeOffset.Year;
            var mm   = dateTimeOffset.Month;
            var dd   = dateTimeOffset.Day;
            return ZString.Format(s_preset.LogRollingFileNameFormat, yyyy, mm, dd, index);
        }

        private static Action<ZLoggerOptions> ConfigureLog() => static x =>
        {
            x.PrefixFormatter = static (writer, info) =>
            {
                var category = info.CategoryName;
                ZString.Utf8Format(writer, s_preset.LogPrefixFormat, category);
            };

            x.SuffixFormatter = static (writer, info) =>
            {
                var level    = info.LogLevel.ToString();
                var eventId  = info.EventId.ToString();
                var dateTime = info.Timestamp.ToLocalTime().DateTime;
                ZString.Utf8Format(writer, s_preset.LogSuffixFormat, level, eventId, dateTime);
            };
        };

        private static readonly Action<ZLoggerOptions> s_configureLog = ConfigureLog();

        static LogManager()
        {
            s_preset = ZLoggerHelperSettings.GetPreset();

            // Standard LoggerFactory does not work on IL2CPP,
            // But you can use ZLogger's UnityLoggerFactory instead,
            // it works on IL2CPP, all platforms(includes mobile).
            s_loggerFactory = UnityLoggerFactory.Create(static builder =>
            {
                // For more configuration, you can use builder.AddFilter
                // builder.AddFilter(static (category, level) => true);
                builder.SetMinimumLevel(s_preset.LogMinimumLevel);

                // AddZLoggerUnityDebug is only available for Unity, it send log to UnityEngine.Debug.Log.
                builder.AddZLoggerUnityDebug(s_configureLog);

                if (s_preset.IsFileLogEnabled)
                    builder.AddZLoggerFile(GetFileUrl(s_preset.LogFileName), s_configureLog);

                if (s_preset.IsRollingFileLogEnabled)
                    builder.AddZLoggerRollingFile(fileNameSelector: static (dt, i) => GetFileUrl(GetFileName(dt, i)),
                                                  timestampPattern: static t => t.ToLocalTime().Date,
                                                  rollSizeKB: s_preset.LogFileRollSizeKB,
                                                  configure: s_configureLog);
            })!;

            s_globalLogger = s_loggerFactory.CreateLogger(s_preset.GlobalLogCategory);

            UnityEngine.Application.quitting += static () => s_loggerFactory.Dispose();
        }

        public static ILogger Logger => s_globalLogger;

        public static ILogger<T> GetLogger<T>() where T : class => s_loggerFactory.CreateLogger<T>();
        public static ILogger    GetLogger(string categoryName) => s_loggerFactory.CreateLogger(categoryName);
    }
}
