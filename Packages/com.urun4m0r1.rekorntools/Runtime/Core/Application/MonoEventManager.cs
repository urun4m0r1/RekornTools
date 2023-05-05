#nullable enable

using System;
using System.Runtime.CompilerServices;
using Urun4m0r1.RekornTools.DesignPatterns;
using UnityEngine;

namespace Urun4m0r1.RekornTools
{
    [DisallowMultipleComponent]
    [AddComponentMenu("RekornTools/Mono Event Manager")]
    public sealed class MonoEventManager : MonoSingleton<MonoEventManager>
    {
#region Events
        /// <summary>
        /// Initialization Order = 1
        /// </summary>
        public static event Action? Awaken;

        /// <summary>
        /// Initialization Order = 1
        /// </summary>
        public static event Action? Enabled;

        /// <summary>
        /// Initialization Order = 3
        /// </summary>
        public static event Action? Started;

        /// <summary>
        /// Termination Order = 1
        /// </summary>
        public static event Action? ApplicationQuit;

        /// <summary>
        /// Termination Order = 2
        /// </summary>
        public static event Action? Disabled;

        /// <summary>
        /// Termination Order = 3
        /// </summary>
        public static event Action? Destroyed;

        public static event Action? ApplicationFocused;
        public static event Action? ApplicationUnfocused;

        public static event Action? ApplicationPause;
        public static event Action? ApplicationResume;
#endregion // Events

#region Callbacks
        protected override void AwakeInvoked()
        {
            HandleCallbackEvent(ref Awaken);
        }

        private void OnEnable()
        {
            HandleCallbackEvent(ref Enabled);
        }

        private void Start()
        {
            HandleCallbackEvent(ref Started);
        }

        protected override void OnApplicationQuitInvoked()
        {
            HandleCallbackEvent(ref ApplicationQuit);
        }

        private void OnDisable()
        {
            HandleCallbackEvent(ref Disabled);
        }

        protected override void OnDestroyInvoked()
        {
            HandleCallbackEvent(ref Destroyed);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                HandleCallbackEvent(ref ApplicationPause);
            else
                HandleCallbackEvent(ref ApplicationResume);
        }

        private void OnApplicationFocus(bool focusStatus)
        {
            if (focusStatus)
                HandleCallbackEvent(ref ApplicationFocused);
            else
                HandleCallbackEvent(ref ApplicationUnfocused);
        }
#endregion // Callbacks

        private static void HandleCallbackEvent(ref Action? action, [CallerMemberName] string callerName = "")
        {
            Debug.Log(nameof(MonoEventManager), callerName);
            action?.Invoke();
            action = null;
        }
    }
}
