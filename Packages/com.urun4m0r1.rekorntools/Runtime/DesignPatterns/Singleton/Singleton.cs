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
        private static Lazy<T>? _lazyInstance;

#region InstanceAccess
        public static bool HasInstance => _lazyInstance?.IsValueCreated ?? false;

        public static T? InstanceOrNull => HasInstance ? _lazyInstance?.Value : null;

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
            _lazyInstance ??= GenerateLazyInstance();

            var instance = _lazyInstance.Value;
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
            _lazyInstance = null;
        }
#endregion // InstanceDestruction

#if UNITY_INCLUDE_TESTS
        private static Lazy<T>? _previousInstance;

        private static GenericValue<T, bool> _isTesting;

        public static bool IsTesting => _isTesting.Value;

        public static void SetupForTests()
        {
            _isTesting.Value = true;

            _previousInstance = _lazyInstance;
            _lazyInstance     = null;
        }

        public static void TearDownForTests()
        {
            DestroySingletonInstance();

            _lazyInstance     = _previousInstance;
            _previousInstance = null;

            _isTesting.Value = false;
        }
#endif // UNITY_INCLUDE_TESTS
    }
}
