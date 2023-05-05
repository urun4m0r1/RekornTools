#nullable enable

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Urun4m0r1.RekornTools
{
    /// <summary>
    /// Provides a static events that invoked when the application is initialized or destroyed.
    /// </summary>
    public static class AppInitializer
    {
#region Events
#if UNITY_EDITOR
        public static event Action? EditorBeforeAssemblyReload;

        public static event Action? EditorAfterAssemblyReload;

        public static event Action? EditorExitingEditMode;

        public static event Action? EditorEnteredPlayMode;

        public static event Action? EditorExitingPlayMode;

        public static event Action? EditorEnteredEditMode;

        public static event Action? EditorWantsToQuit;

        public static event Action? EditorQuitting;
#endif

        /// <summary>
        /// Initialization Order = 1<br/><br/>
        ///
        /// <b>Standalone Player</b><br/>
        /// Invoked directly after application launch, before any subsystems are initialized.<br/><br/>
        ///
        /// <b>Unity Editor</b><br/>
        /// Domain Reloading ON: Invoked every time entering Play Mode.<br/>
        /// Domain Reloading OFF: Invoked only once when entering Play Mode after last script compilation.<br/><br/>
        /// </summary>
        public static event Action? StaticConstructorInitialized;

        /// <summary>
        /// Initialization Order = 2
        /// </summary>
        public static event Action? SubsystemRegistration;

        /// <summary>
        /// Initialization Order = 3
        /// </summary>
        public static event Action? AfterAssembliesLoaded;

        /// <summary>
        /// Initialization Order = 4
        /// </summary>
        public static event Action? BeforeSplashScreen;

        /// <summary>
        /// Initialization Order = 5
        /// </summary>
        public static event Action? BeforeSceneLoad;

        /// <summary>
        /// Initialization Order = 6
        /// </summary>
        public static event Action? AfterSceneLoad;

        /// <summary>
        /// Termination Order = 1
        /// </summary>
        public static event Action? ApplicationWantsToQuit;

        /// <summary>
        /// Termination Order = 2
        /// </summary>
        public static event Action? ApplicationQuit;
#endregion // Events

#region Callback Registration
        static AppInitializer()
        {
            OnStaticConstructorInitialized();

#if UNITY_EDITOR
            AssemblyReloadEvents.beforeAssemblyReload += OnEditorBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload  += OnEditorAfterAssemblyReload;
            EditorApplication.playModeStateChanged    += OnEditorPlayModeStateChanged;
            EditorApplication.wantsToQuit             += OnEditorWantsToQuit;
            EditorApplication.quitting                += OnEditorQuitting;
#endif

            Application.wantsToQuit += OnApplicationWantsToQuit;
            Application.quitting    += OnApplicationQuit;
        }
#endregion // Callback Registration

#region Callbacks
#if UNITY_EDITOR
        private static void OnEditorBeforeAssemblyReload()
        {
            HandleCallbackEvent(ref EditorBeforeAssemblyReload);
        }

        private static void OnEditorAfterAssemblyReload()
        {
            HandleCallbackEvent(ref EditorAfterAssemblyReload);
        }

        private static void OnEditorPlayModeStateChanged(PlayModeStateChange state)
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
            HandleCallbackEvent(ref EditorExitingEditMode);
        }

        private static void OnEditorEnteredPlayMode()
        {
            HandleCallbackEvent(ref EditorEnteredPlayMode);
        }

        private static void OnEditorExitingPlayMode()
        {
            HandleCallbackEvent(ref EditorExitingPlayMode);
        }

        private static void OnEditorEnteredEditMode()
        {
            HandleCallbackEvent(ref EditorEnteredEditMode);
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
#endif

        private static void OnStaticConstructorInitialized()
        {
            HandleCallbackEvent(ref StaticConstructorInitialized);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnSubsystemRegistration()
        {
            HandleCallbackEvent(ref SubsystemRegistration);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
        {
            HandleCallbackEvent(ref AfterAssembliesLoaded);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void OnBeforeSplashScreen()
        {
            HandleCallbackEvent(ref BeforeSplashScreen);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad()
        {
            HandleCallbackEvent(ref BeforeSceneLoad);
            MonoInitializer.CreateInstance();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad()
        {
            HandleCallbackEvent(ref AfterSceneLoad);
        }

        private static bool OnApplicationWantsToQuit()
        {
            HandleCallbackEvent(ref ApplicationWantsToQuit);
            return true;
        }

        private static void OnApplicationQuit()
        {
            HandleCallbackEvent(ref ApplicationQuit);
        }
#endregion // Callbacks

        private static void HandleCallbackEvent(ref Action? action, [CallerMemberName] string callerName = "")
        {
            Debug.Log(nameof(AppInitializer), callerName);
            action?.Invoke();
            action = null;
        }
    }
}
