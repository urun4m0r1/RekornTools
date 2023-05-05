﻿#nullable enable

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Urun4m0r1.RekornTools
{
    /// <summary>
    /// Provides a static events that invoked when the application is initialized or destroyed.
    /// </summary>
    public static class AppInitializer
    {
#region Events
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
        public static event Action? ApplicationQuit;
#endregion // Events

#region Callback Registration
        private static readonly Action OnQuittingCache = OnQuitting;

        static AppInitializer()
        {
            OnStaticConstructorInitialized();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnApplicationInitialized()
        {
            if (Application.isPlaying)
            {
                Application.quitting -= OnQuittingCache;
                Application.quitting += OnQuittingCache;
            }
        }

        private static void OnQuitting()
        {
            Application.quitting -= OnQuittingCache;
            OnApplicationQuit();
        }
#endregion // Callback Registration

#region Callbacks
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
