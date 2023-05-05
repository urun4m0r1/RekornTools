#nullable enable

using Microsoft.Extensions.Logging;
using UnityEngine;
using ILogger = Microsoft.Extensions.Logging.ILogger;

#if UNITY_EDITOR
using Urun4m0r1.RekornTools.Unity.Editor;
using UnityEditor;

#else // !UNITY_EDITOR
using Urun4m0r1.RekornTools.Unity;

#endif // UNITY_EDITOR

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    public static class LogManager
    {
        public static ILogger    Global                      => GetLogger().Global;
        public static ILogger<T> Create<T>() where T : class => GetLogger().Create<T>();
        public static ILogger    Create(string categoryName) => GetLogger().Create(categoryName);
        public static bool       IsDisposed                  => GetLogger().IsDisposed;

        private static ZLogger? s_logger;

        private static ZLogger GetLogger()
        {
#if UNITY_EDITOR
            return EditorApplication.isPlayingOrWillChangePlaymode
                ? GetLoggerPlayMode()
                : GetLoggerEditMode();

#else // !UNITY_EDITOR
            return GetLoggerStandalone();

#endif // UNITY_EDITOR
        }

#if UNITY_EDITOR
        private static ZLogger? s_loggerEditMode;

        private static ZLogger GetLoggerPlayMode()
        {
            if (s_logger is not null)
                return s_logger;

            Info(nameof(GetLoggerPlayMode));
            s_logger = new ZLogger();
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

        private static ZLogger GetLoggerEditMode()
        {
            if (s_loggerEditMode is not null)
                return s_loggerEditMode;

            Info(nameof(GetLoggerEditMode));
            s_loggerEditMode = new ZLogger();
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

#else // !UNITY_EDITOR
        private static ZLogger GetLoggerStandalone()
        {
            if (s_logger is not null)
                return s_logger;

            Info(nameof(GetLoggerStandalone));
            s_logger = new ZLogger();
            RegisterEvent();
            return s_logger;

            static void RegisterEvent()
            {
                UnityEventManager.ApplicationQuit += OnApplicationQuit;
            }

            static void UnregisterEvent()
            {
                UnityEventManager.ApplicationQuit -= OnApplicationQuit;
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
            Debug.Log($"<color=cyan><b>[{nameof(LogManager)}]</b></color> {message}");
        }
    }
}
