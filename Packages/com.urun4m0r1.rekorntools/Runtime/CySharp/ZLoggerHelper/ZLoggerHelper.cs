#nullable enable

using System;
using Microsoft.Extensions.Logging;
using UnityEngine;
using UnityEngine.Assertions;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    public static class Log
    {
        public static ILogger    Global                      => s_logger.Global;
        public static ILogger<T> Create<T>() where T : class => s_logger.Create<T>();
        public static ILogger    Create(string categoryName) => s_logger.Create(categoryName);
        public static bool       IsDisposed                  => s_logger.IsDisposed;

        private static Logger s_logger;

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnRuntimeInitializeOnLoad()
        {
            Info("Log OnRuntimeInitializeOnLoadMethod");
        }

        private static bool s_initializedInPlayMode;

        static Log() // 2
        {
            Info("Log static constructor");

            s_logger = new Logger();

            s_initializedInPlayMode = EditorApplication.isPlayingOrWillChangePlaymode;

            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            EditorApplication.playModeStateChanged    += OnPlayModeStateChanged;

            static void OnBeforeAssemblyReload()
            {
                Info("Log on BeforeAssemblyReload");
                Assert.IsFalse(s_logger.IsDisposed, "Logger is already disposed.");

                s_logger.Dispose();
            }

            static void OnPlayModeStateChanged(PlayModeStateChange state)
            {
                switch (state)
                {
                    // When clicking Play button in Editor
                    case PlayModeStateChange.ExitingEditMode:
                    {
                        Info("Log on PlayModeStateChange: ExitingEditMode");
                        Assert.IsFalse(s_initializedInPlayMode, "Logger is initialized in play mode.");
                        Assert.IsFalse(s_logger.IsDisposed,     "Logger is already disposed.");

                        s_logger.Dispose();
                        break;
                    }
                    // Domain Reload: ON (Static constructor is called again)
                    case PlayModeStateChange.EnteredPlayMode when s_initializedInPlayMode:
                    {
                        Info("Log on PlayModeStateChange: EnteredPlayMode (Domain Reload: ON)");
                        Assert.IsFalse(s_logger.IsDisposed, "Logger is already disposed.");
                        break;
                    }
                    // Domain Reload: OFF (Static constructor is not called again)
                    case PlayModeStateChange.EnteredPlayMode when !s_initializedInPlayMode:
                    {
                        Info("Log on PlayModeStateChange: EnteredPlayMode (Domain Reload: OFF)");
                        Assert.IsTrue(s_logger.IsDisposed, "Logger is not disposed yet.");

                        s_logger = new Logger();

                        s_initializedInPlayMode = EditorApplication.isPlayingOrWillChangePlaymode;
                        break;
                    }
                    // When clicking Stop button in Editor
                    case PlayModeStateChange.ExitingPlayMode:
                    {
                        Info("Log on PlayModeStateChange: ExitingPlayMode");
                        Assert.IsTrue(s_initializedInPlayMode, "Logger is not initialized in play mode.");
                        Assert.IsFalse(s_logger.IsDisposed, "Logger is already disposed.");

                        s_logger.Dispose();
                        break;
                    }
                    // When ExitingPlayMode is finished
                    case PlayModeStateChange.EnteredEditMode: // 5
                    {
                        Info("Log on PlayModeStateChange: EnteredEditMode");
                        Assert.IsTrue(s_initializedInPlayMode, "Logger is not initialized in play mode.");
                        Assert.IsTrue(s_logger.IsDisposed,     "Logger is not disposed yet.");

                        s_logger = new Logger();

                        s_initializedInPlayMode = EditorApplication.isPlayingOrWillChangePlaymode;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state), state, null!);
                }
            }
        }
#else // UNITY_EDITOR
        static Log()
        {
            Info("Log static constructor");

            s_logger = new Logger();

            UnityEngine.Application.quitting += OnApplicationQuit;

            static void OnApplicationQuit()
            {
                Info("Log on ApplicationQuit");
                Assert.IsFalse(s_logger.IsDisposed, "Logger is already disposed.");

                s_logger.Dispose();
            }
        }
#endif // UNITY_EDITOR

        private static void Info(string message)
        {
            Debug.Log($"<color=cyan><b>[Debug]</b></color> {message}");
        }
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
            Info("Logger initializing...");
            {
                _preset = ZLoggerHelperSettings.GetPreset();

                // Standard LoggerFactory does not work on IL2CPP,
                // But you can use ZLogger's UnityLoggerFactory instead,
                // it works on IL2CPP, all platforms(includes mobile).
                _loggerFactory = UnityLoggerFactory.Create(ConfigureLogger)!;

                Global = _loggerFactory.CreateLogger(_preset.GlobalCategory);
            }
            Info("Logger initialized.");


            void ConfigureLogger(ILoggingBuilder builder)
            {
                // For more configuration, you can use builder.AddFilter
                // builder.AddFilter(static (category, level) => true);
                builder.SetMinimumLevel(_preset.MinimumLevel);

                if (_preset.UseUnityLogging)
                    builder.AddZLoggerUnityDebug(ConfigureUnityLog);

                if (_preset.UseFileLogging)
                    builder.AddZLoggerFile(_preset.FileUrl, ConfigureFileLog);

                if (_preset.UseRollingFileLogging)
                    builder.AddZLoggerRollingFile(
                        fileNameSelector: (dt, i) => _preset.GetRollingFileUrl(dt, i),
                        timestampPattern: static t => t.ToLocalTime().Date,
                        rollSizeKB: _preset.RollingFileSizeKB,
                        configure: ConfigureFileLog);

                void ConfigureUnityLog(ZLoggerOptions options)
                {
                    options.PrefixFormatter = (writer, info) => _preset.FormatUnityPrefix(info, writer);
                    options.SuffixFormatter = (writer, info) => _preset.FormatUnitySuffix(info, writer);
                }

                void ConfigureFileLog(ZLoggerOptions options)
                {
                    options.PrefixFormatter = (writer, info) => _preset.FormatFilePrefix(info, writer);
                    options.SuffixFormatter = (writer, info) => _preset.FormatFileSuffix(info, writer);
                }
            }
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
            Info("Logger disposing...");
            {
                if (IsDisposed)
                {
                    Info("Logger already disposed.");
                    return;
                }

                _loggerFactory.Dispose();
                IsDisposed = true;
            }
            Info("Logger disposed.");
        }

        private static void Info(string message)
        {
            Debug.Log($"<color=cyan><b>[Debug]</b></color> {message}");
        }
    }
}
