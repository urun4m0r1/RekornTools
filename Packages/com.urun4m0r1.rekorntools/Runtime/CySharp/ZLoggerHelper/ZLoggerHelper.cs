#nullable enable

using System;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    public static class Log
    {
        public static ILogger    Global                      => Logger.Global;
        public static ILogger<T> Create<T>() where T : class => Logger.Create<T>();
        public static ILogger    Create(string categoryName) => Logger.Create(categoryName);
        public static bool       IsDisposed                  => Logger.IsDisposed;

        private static Logger Logger { get; } = new();
    }

    public sealed class Logger : IDisposable
    {
        public ILogger Global { get; }

        public bool IsDisposed { get; private set; }

        public ILogger<T> Create<T>() where T : class => _loggerFactory.CreateLogger<T>();
        public ILogger    Create(string categoryName) => _loggerFactory.CreateLogger(categoryName);

        private readonly ZLoggerHelperPreset _preset;
        private readonly ILoggerFactory      _loggerFactory;

        public Logger()
        {
            Log("Logger initializing...");

            _preset = ZLoggerHelperSettings.GetPreset();

            // Standard LoggerFactory does not work on IL2CPP,
            // But you can use ZLogger's UnityLoggerFactory instead,
            // it works on IL2CPP, all platforms(includes mobile).
            _loggerFactory = UnityLoggerFactory.Create(builder =>
            {
                // For more configuration, you can use builder.AddFilter
                // builder.AddFilter(static (category, level) => true);
                builder.SetMinimumLevel(_preset.MinimumLevel);

                if (_preset.UseUnityLogging)
                    builder.AddZLoggerUnityDebug(ConfigureUnityLog());

                if (_preset.UseFileLogging)
                    builder.AddZLoggerFile(_preset.FileUrl, ConfigureFileLog());

                if (_preset.UseRollingFileLogging)
                    builder.AddZLoggerRollingFile(
                        fileNameSelector: (dt, i) => _preset.GetRollingFileUrl(dt, i)
                      , timestampPattern: static t => t.ToLocalTime().Date
                      , rollSizeKB: _preset.RollingFileSizeKB
                      , configure: ConfigureFileLog()
                    );
            })!;

            Global = _loggerFactory.CreateLogger(_preset.GlobalCategory);

            Log("Logger initialized.");

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

        ~Logger()
        {
            DisposeLogger();
        }

        private void DisposeLogger()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;
#endif

            Log("Logger disposing...");

            if (IsDisposed)
                return;

            _loggerFactory.Dispose();
            IsDisposed = true;

            Log("Logger disposed.");
        }

        private Action<ZLoggerOptions> ConfigureUnityLog() => x =>
        {
            x.PrefixFormatter = (writer, info) => _preset.FormatUnityPrefix(info, writer);
            x.SuffixFormatter = (writer, info) => _preset.FormatUnitySuffix(info, writer);
        };

        private Action<ZLoggerOptions> ConfigureFileLog() => x =>
        {
            x.PrefixFormatter = (writer, info) => _preset.FormatFilePrefix(info, writer);
            x.SuffixFormatter = (writer, info) => _preset.FormatFileSuffix(info, writer);
        };

        private static void Log(string message)
        {
            UnityEngine.Debug.Log($"<color=cyan><b>[Debug]</b></color> {message}");
        }
    }
}
