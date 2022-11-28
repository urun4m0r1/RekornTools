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

        private static Action<ZLoggerOptions> ConfigureLog() => static x =>
        {
            x.PrefixFormatter = static (writer, info) =>
            {
                var category = info.CategoryName;
                ZString.Utf8Format(writer, s_preset.PrefixFormat, category);
            };

            x.SuffixFormatter = static (writer, info) =>
            {
                var level    = info.LogLevel.ToString();
                var eventId  = info.EventId.ToString();
                var dateTime = info.Timestamp.ToLocalTime().DateTime;
                ZString.Utf8Format(writer, s_preset.SuffixFormat, level, eventId, dateTime);
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
                builder.SetMinimumLevel(s_preset.MinimumLevel);

                // AddZLoggerUnityDebug is only available for Unity, it send log to UnityEngine.Debug.Log.
                if (s_preset.UseUnityLogging)
                    builder.AddZLoggerUnityDebug(s_configureLog);

                if (s_preset.UseFileLogging)
                    builder.AddZLoggerFile(s_preset.FileUrl, s_configureLog);

                if (s_preset.UseRollingFileLogging)
                    builder.AddZLoggerRollingFile(fileNameSelector: static (dt, i) => s_preset.GetRollingFileUrl(dt, i),
                                                  timestampPattern: static t => t.ToLocalTime().Date,
                                                  rollSizeKB: s_preset.RollingFileSizeKB,
                                                  configure: s_configureLog);
            })!;

            s_globalLogger = s_loggerFactory.CreateLogger(s_preset.GlobalCategory);

            UnityEngine.Application.quitting += static () => s_loggerFactory.Dispose();
        }

        public static ILogger Logger => s_globalLogger;

        public static ILogger<T> GetLogger<T>() where T : class => s_loggerFactory.CreateLogger<T>();
        public static ILogger    GetLogger(string categoryName) => s_loggerFactory.CreateLogger(categoryName);
    }
}
