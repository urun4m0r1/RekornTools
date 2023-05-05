#nullable enable

using System;
using System.Runtime.CompilerServices;
using UnityEditor;

namespace Urun4m0r1.RekornTools.Editor
{
    /// <summary>
    /// Provides a static events that invoked when the application is initialized or destroyed.
    /// </summary>
    public static class EditorEventManager
    {
#region Events
        public static event Action? BeforeAssemblyReload;

        public static event Action? AfterAssemblyReload;

        public static event Action? ExitingEditMode;

        public static event Action? EnteredPlayMode;

        public static event Action? ExitingPlayMode;

        public static event Action? EnteredEditMode;

        public static event Action? EditorWantsToQuit;

        public static event Action? EditorQuitting;
#endregion // Events

#region Callback Registration
        static EditorEventManager()
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload  += OnAfterAssemblyReload;
            EditorApplication.playModeStateChanged    += OnPlayModeStateChanged;
            EditorApplication.wantsToQuit             += OnEditorWantsToQuit;
            EditorApplication.quitting                += OnEditorQuitting;
        }
#endregion // Callback Registration

#region Callbacks
        private static void OnBeforeAssemblyReload()
        {
            HandleCallbackEvent(ref BeforeAssemblyReload);
        }

        private static void OnAfterAssemblyReload()
        {
            HandleCallbackEvent(ref AfterAssemblyReload);
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    OnEditorExitingEditMode();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnEditorEnteredPlayMode();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    OnEditorExitingPlayMode();
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    OnEditorEnteredEditMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null!);
            }
        }

        private static void OnEditorExitingEditMode()
        {
            HandleCallbackEvent(ref ExitingEditMode);
        }

        private static void OnEditorEnteredPlayMode()
        {
            HandleCallbackEvent(ref EnteredPlayMode);
        }

        private static void OnEditorExitingPlayMode()
        {
            HandleCallbackEvent(ref ExitingPlayMode);
        }

        private static void OnEditorEnteredEditMode()
        {
            HandleCallbackEvent(ref EnteredEditMode);
        }

        private static bool OnEditorWantsToQuit()
        {
            HandleCallbackEvent(ref EditorWantsToQuit);
            return true;
        }

        private static void OnEditorQuitting()
        {
            HandleCallbackEvent(ref EditorQuitting);
        }
#endregion // Callbacks

        private static void HandleCallbackEvent(ref Action? action, [CallerMemberName] string callerName = "")
        {
            Debug.Log(nameof(EditorEventManager), callerName);
            action?.Invoke();
            action = null;
        }
    }
}
