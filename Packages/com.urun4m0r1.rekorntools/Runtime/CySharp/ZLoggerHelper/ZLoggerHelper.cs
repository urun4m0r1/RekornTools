#nullable enable

using System;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

#if UNITY_EDITOR
using Urun4m0r1.RekornTools.Unity.Editor;
using UnityEditor;
#endif

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    public static class Log
    {
        public static ILogger    Global                      => GetLogger().Global;
        public static ILogger<T> Create<T>() where T : class => GetLogger().Create<T>();
        public static ILogger    Create(string categoryName) => GetLogger().Create(categoryName);
        public static bool       IsDisposed                  => GetLogger().IsDisposed;

        private static Logger? s_logger;

        private static Logger GetLogger()
        {
#if UNITY_EDITOR
            return EditorApplication.isPlayingOrWillChangePlaymode
                ? GetLoggerPlayMode()
                : GetLoggerEditMode();
#else
            return GetLoggerStandalone();
#endif
        }

#if UNITY_EDITOR
        private static Logger? s_loggerEditMode;

        private static Logger GetLoggerPlayMode()
        {
            if (s_logger is not null)
                return s_logger;

            Info(nameof(GetLoggerPlayMode));
            s_logger = new Logger();
            RegisterEvents();
            return s_logger;

            static void RegisterEvents()
            {
                EditorEventManager.BeforeAssemblyReload += OnBeforeAssemblyReload;
                EditorEventManager.ExitingPlayMode      += OnExitingPlayMode;
                EditorEventManager.EditorQuitting       += OnEditorQuitting;
            }

            static void UnregisterEvents()
            {
                EditorEventManager.BeforeAssemblyReload -= OnBeforeAssemblyReload;
                EditorEventManager.ExitingPlayMode      -= OnExitingPlayMode;
                EditorEventManager.EditorQuitting       -= OnEditorQuitting;
            }

            static void OnBeforeAssemblyReload() => Dispose(nameof(OnBeforeAssemblyReload));
            static void OnExitingPlayMode()      => Dispose(nameof(OnExitingPlayMode));
            static void OnEditorQuitting()       => Dispose(nameof(OnEditorQuitting));

            static void Dispose(string message)
            {
                UnregisterEvents();

                if (s_logger is null)
                    return;

                Info(message);
                s_logger?.Dispose();
                s_logger = null;
            }
        }

        private static Logger GetLoggerEditMode()
        {
            if (s_loggerEditMode is not null)
                return s_loggerEditMode;

            Info(nameof(GetLoggerEditMode));
            s_loggerEditMode = new Logger();
            RegisterEvents();
            return s_loggerEditMode;

            static void RegisterEvents()
            {
                EditorEventManager.BeforeAssemblyReload += OnBeforeAssemblyReload;
                EditorEventManager.ExitingEditMode      += OnExitingEditMode;
                EditorEventManager.EditorQuitting       += OnEditorQuitting;
            }

            static void UnregisterEvents()
            {
                EditorEventManager.BeforeAssemblyReload -= OnBeforeAssemblyReload;
                EditorEventManager.ExitingEditMode      -= OnExitingEditMode;
                EditorEventManager.EditorQuitting       -= OnEditorQuitting;
            }

            static void OnBeforeAssemblyReload() => Dispose(nameof(OnBeforeAssemblyReload));
            static void OnExitingEditMode()      => Dispose(nameof(OnExitingEditMode));
            static void OnEditorQuitting()       => Dispose(nameof(OnEditorQuitting));

            static void Dispose(string message)
            {
                UnregisterEvents();

                if (s_loggerEditMode is null)
                    return;

                Info(message);
                s_loggerEditMode?.Dispose();
                s_loggerEditMode = null;
            }
        }
#else // UNITY_EDITOR
        private static Logger GetLoggerStandalone()
        {
            if (s_logger is not null)
                return s_logger;

            Info(nameof(GetLoggerStandalone));
            s_logger = new Logger();
            RegisterEvent();
            return s_logger;

            static void RegisterEvent()
            {
                EventManager.ApplicationQuit += OnApplicationQuit;
            }

            static void UnregisterEvent()
            {
                EventManager.ApplicationQuit -= OnApplicationQuit;
            }

            static void OnApplicationQuit() => Dispose(nameof(OnApplicationQuit));

            static void Dispose(string message)
            {
                UnregisterEvent();

                if (s_logger is null)
                    return;

                Info(message);
                s_logger?.Dispose();
                s_logger = null;
            }
        }
#endif // UNITY_EDITOR

        private static void Info(string message)
        {
            Debug.Log($"<color=cyan><b>[{nameof(Log)}]</b></color> {message}");
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
            Debug.Log($"<color=cyan><b>[{nameof(Logger)}]</b></color> {message}");
        }
    }
}
