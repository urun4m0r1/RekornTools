#nullable enable

using System;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    public static class LogManager
    {
        public static ILogger Logger => s_globalLogger;

        public static ILogger<T> GetLogger<T>() where T : class => s_loggerFactory.CreateLogger<T>();
        public static ILogger    GetLogger(string categoryName) => s_loggerFactory.CreateLogger(categoryName);

        public static bool IsDisposed => s_logScope.IsLoggerDisposed;

        private static readonly ZLoggerHelperPreset s_preset;
        private static readonly ILogger             s_globalLogger;
        private static readonly ILoggerFactory      s_loggerFactory;

        // ReSharper disable once NotAccessedField.Local
        private static readonly LogManagerScope s_logScope;

        /// <summary>
        /// 외부 static 클래스의 생명 주기 관리를 위한 클래스.
        /// </summary>
        private sealed class LogManagerScope : IDisposable
        {
            public bool IsLoggerDisposed { get; private set; }

            public LogManagerScope()
            {
                UnityEngine.Application.quitting += OnApplicationQuit;
            }

            private void OnApplicationQuit()
            {
                UnityEngine.Application.quitting -= OnApplicationQuit;
                Dispose();
            }

            public void Dispose()
            {
                DisposeLogger();
                GC.SuppressFinalize(this);
            }

            ~LogManagerScope()
            {
                DisposeLogger();
            }

            private void DisposeLogger()
            {
                if (IsLoggerDisposed)
                    return;

                s_loggerFactory.Dispose();
                IsLoggerDisposed = true;

                Log("LogManager disposed.");
            }
        }

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

                if (s_preset.UseUnityLogging)
                    builder.AddZLoggerUnityDebug(ConfigureUnityLog());

                if (s_preset.UseFileLogging)
                    builder.AddZLoggerFile(s_preset.FileUrl, ConfigureFileLog());

                if (s_preset.UseRollingFileLogging)
                    builder.AddZLoggerRollingFile(
                        fileNameSelector: static (dt, i) => s_preset.GetRollingFileUrl(dt, i)
                      , timestampPattern: static t => t.ToLocalTime().Date
                      , rollSizeKB: s_preset.RollingFileSizeKB
                      , configure: ConfigureFileLog()
                    );
            })!;

            s_globalLogger = s_loggerFactory.CreateLogger(s_preset.GlobalCategory);

            s_logScope = new LogManagerScope();

            Log("LogManager initialized.");
        }

        private static Action<ZLoggerOptions> ConfigureUnityLog() => static x =>
        {
            x.PrefixFormatter = static (writer, info) => s_preset.FormatUnityPrefix(info, writer);
            x.SuffixFormatter = static (writer, info) => s_preset.FormatUnitySuffix(info, writer);
        };

        private static Action<ZLoggerOptions> ConfigureFileLog() => static x =>
        {
            x.PrefixFormatter = static (writer, info) => s_preset.FormatFilePrefix(info, writer);
            x.SuffixFormatter = static (writer, info) => s_preset.FormatFileSuffix(info, writer);
        };

        private static void Log(string message)
        {
            UnityEngine.Debug.Log($"<color=cyan><b>[Debug]</b></color> {message}");
        }
    }
}
