#if UNITY_EDITOR

#nullable enable

using UnityEditor;
using Urun4m0r1.RekornTools.Unity.Editor;

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    public static partial class LogManager
    {
        private static ZLogger? s_loggerEditMode;


        private static ZLogger GetLoggerEditor()
        {
            return EditorApplication.isPlayingOrWillChangePlaymode
                ? GetLoggerPlayMode()
                : GetLoggerEditMode();
        }

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
    }
}
#endif // UNITY_EDITOR
