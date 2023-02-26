// #define UNITY_INCLUDE_TESTS
// #undef UNITY_INCLUDE_TESTS

#nullable enable

using System;
using UnityEngine;
using Urun4m0r1.RekornTools.Utils;

namespace Urun4m0r1.RekornTools.DesignPatterns
{
    /// <inheritdoc />
    public class Singleton<T> : ISingleton where T : class
    {
        private static Lazy<T>? s_lazyInstance;

#region InstanceAccess
        public static bool HasInstance => s_lazyInstance?.IsValueCreated ?? false;

        public static T? InstanceOrNull => HasInstance ? s_lazyInstance?.Value : null;

        public static T Instance => GetOrCreateInstance();

        public static void CreateInstance()
        {
            if (HasInstance)
            {
                throw SingletonHelper<T>.InstanceAlreadyCreatedException;
            }

            GetOrCreateInstance();
        }

        public static bool TryCreateInstance()
        {
            if (HasInstance)
            {
                return false;
            }

            GetOrCreateInstance();
            return true;
        }

        public bool IsSingleton => ReferenceEquals(this, InstanceOrNull!);
#endregion // InstanceAccess

#region InstanceGeneration
        private static T GetOrCreateInstance()
        {
            s_lazyInstance ??= GenerateLazyInstance();

            var instance = s_lazyInstance.Value;
            if (instance == null)
            {
                ResetInstanceReferences();
                throw SingletonHelper<T>.InstanceNullException;
            }

            return instance;
        }

        private static Lazy<T> GenerateLazyInstance()
        {
#if UNITY_INCLUDE_TESTS
            if (IsTesting)
            {
                return SingletonHelper<T>.GenerateThreadSafeLazyInstance();
            }
#endif // UNITY_INCLUDE_TESTS

            SingletonHelper<T>.EnsureApplicationIsPlaying();
            var lazyInstance = SingletonHelper<T>.GenerateThreadSafeLazyInstance();
            RegisterApplicationQuittingCallback();

            return lazyInstance;
        }
#endregion // InstanceGeneration

#region InstanceDestruction
        private static void RegisterApplicationQuittingCallback()
        {
            Action onApplicationQuitting = OnApplicationQuitting;
            Application.quitting -= onApplicationQuitting;
            Application.quitting += onApplicationQuitting;
        }

        private static void OnApplicationQuitting()
        {
            Application.quitting -= OnApplicationQuitting;
            DestroySingletonInstance();
        }

        private static void DestroySingletonInstance()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            (InstanceOrNull as IDisposable)?.Dispose();
            ResetInstanceReferences();
            SingletonHelper<T>.LogInstanceDestroyed();
        }

        private static void ResetInstanceReferences()
        {
            s_lazyInstance = null;
        }
#endregion // InstanceDestruction

#if UNITY_INCLUDE_TESTS
        private static Lazy<T>? s_previousInstance;

        private static GenericValue<T, bool> s_isTesting;

        public static bool IsTesting => s_isTesting.Value;

        public static void SetupForTests()
        {
            s_isTesting.Value = true;

            s_previousInstance = s_lazyInstance;
            s_lazyInstance     = null;
        }

        public static void TearDownForTests()
        {
            DestroySingletonInstance();

            s_lazyInstance     = s_previousInstance;
            s_previousInstance = null;

            s_isTesting.Value = false;
        }
#endif // UNITY_INCLUDE_TESTS
    }
}
