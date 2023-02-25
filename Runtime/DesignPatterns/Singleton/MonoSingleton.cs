#nullable enable

using System;
using UnityEngine;
using Rekorn.Tools.Utils;

namespace Rekorn.Tools.DesignPatterns
{
    /// <inheritdoc cref="Rekorn.Tools.DesignPatterns.IMonoSingleton" />
    public class MonoSingleton<T> : MonoBehaviour, IMonoSingleton where T : MonoBehaviour
    {
        private static Lazy<T>? _lazyInstance;

        private static GenericValue<T, GameObject> _instanceGameObject;

        private static string GenericTypeName => SingletonHelper<T>.GenericTypeName;
        private static string ClassName       => SingletonHelper<T>.ClassName;
        private static string TypeName        => SingletonHelper<T>.TypeName;

        private static string GameObjectName => $"[{GenericTypeName}] ({TypeName})";

        private static string GetGameObjectPath(GameObject gameObject) => $"\"{gameObject.name}\" in scene \"{gameObject.scene.name}\"";

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

        public bool IsSingleton => ReferenceEquals(this, InstanceOrNull!) || ReferenceEquals(gameObject, _instanceGameObject.Value!);
#endregion // InstanceAccess

#region InstanceGeneration
        private static T GetOrCreateInstance()
        {
            _lazyInstance ??= GenerateLazyInstance();

            var instance = _lazyInstance.Value;
            if (instance == null)
            {
                ResetInstanceReferences();
                throw SingletonHelper<T>.InstanceNullOrDestroyedException;
            }

            return instance;
        }

        private static Lazy<T> GenerateLazyInstance()
        {
#if UNITY_INCLUDE_TESTS
            if (IsTesting)
            {
                return SingletonHelper<T>.GenerateThreadSafeLazyInstance(GenerateInstance);
            }
#endif // UNITY_INCLUDE_TESTS

            SingletonHelper<T>.EnsureApplicationIsPlaying();
            return SingletonHelper<T>.GenerateThreadSafeLazyInstance(GenerateInstance);
        }

        private static T GenerateInstance()
        {
            var instances = FindObjectsOfType<T>(true);
            if (instances == null || instances.Length == 0)
            {
                return CreateNewInstance();
            }

            if (instances.Length == 1)
            {
                return GetExistingInstance(instances[0]);
            }

            ResetInstanceReferences();
            throw new InvalidOperationException($"{ClassName} cannot be created because there are already {instances.Length} instances exists.");
        }

        private static T CreateNewInstance()
        {
            var gameObject = new GameObject(GameObjectName)
            {
                hideFlags = HideFlags.DontSave,
            };

            _instanceGameObject.Value = gameObject;

            var instance = gameObject.AddComponent<T>();
            if (instance == null)
            {
                ResetInstanceReferences();
                DestroyTarget(gameObject);
                throw new MissingComponentException($"{ClassName} failed to add component to {GetGameObjectPath(gameObject)}.");
            }

            SetDontDestroyOnLoad(instance);

            SingletonHelper<T>.LogInstanceCreated();
            return instance;
        }

        private static T GetExistingInstance(T instance)
        {
            var gameObject = instance.gameObject;

            _instanceGameObject.Value = gameObject;

            SetDontDestroyOnLoad(gameObject);

            Debug.Log(GenericTypeName, $"<{TypeName}> instance assigned to existing {GetGameObjectPath(gameObject)}.");
            return instance;
        }
#endregion // InstanceGeneration

#region InstanceDestruction
        private static void DestroySingletonInstance()
        {
            var instance = InstanceOrNull;

            // ReSharper disable once SuspiciousTypeConversion.Global
            (instance as IDisposable)?.Dispose();
            ResetInstanceReferences();

            if (instance != null)
            {
                DestroyTarget(instance);
            }

            SingletonHelper<T>.LogInstanceDestroyed();
        }

        private void DestroyDuplicatedInstance()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            (this as IDisposable)?.Dispose();
            DestroyTarget(this);
            Debug.LogWarning(GenericTypeName, $"<{TypeName}> instance already exists. Destroying duplicated instance attached to {GetGameObjectPath(gameObject)}.");
        }

        private static void ResetInstanceReferences()
        {
            _lazyInstance             = null;
            _instanceGameObject.Value = null;
        }
#endregion // InstanceDestruction

#region MonoBehaviour
        private bool _isApplicationQuitting;

        private void Awake()
        {
            if (_instanceGameObject.Value == null)
            {
                CreateInstance();
            }
            else if (!IsSingleton)
            {
                DestroyDuplicatedInstance();
            }

            AwakeInvoked();
        }

        private void OnApplicationQuit()
        {
            _isApplicationQuitting = true;

            OnApplicationQuitInvoked();

            if (IsSingleton)
            {
                DestroySingletonInstance();
            }
        }

        private void OnDestroy()
        {
            OnDestroyInvoked();

            if (IsSingleton)
            {
                DestroySingletonInstance();

                if (!_isApplicationQuitting)
                {
                    Debug.LogError(GenericTypeName, $"<{TypeName}> instance should not be destroyed manually. Singleton integrity is compromised.");
                }
            }
        }

        protected virtual void AwakeInvoked()             { }
        protected virtual void OnApplicationQuitInvoked() { }
        protected virtual void OnDestroyInvoked()         { }
#endregion // MonoBehaviour

#region Utils
        private static void DestroyTarget(UnityEngine.Object target)
        {
            if (Application.isPlaying)
            {
                Destroy(target);
            }
            else
            {
                DestroyImmediate(target);
            }
        }

        private static void SetDontDestroyOnLoad(UnityEngine.Object target)
        {
            if (Application.isPlaying)
                DontDestroyOnLoad(target);
        }
#endregion // Utils

#if UNITY_INCLUDE_TESTS
        private static Lazy<T>? _previousInstance;

        private static GenericValue<T, GameObject> _previousGameObject;

        private static GenericValue<T, bool> _isTesting;

        public static bool IsTesting => _isTesting.Value;

        public static void SetupForTests()
        {
            _isTesting.Value = true;

            _previousInstance   = _lazyInstance;
            _previousGameObject = _instanceGameObject;
            _lazyInstance       = null;
            _instanceGameObject = default;
        }

        public static void TearDownForTests()
        {
            DestroySingletonInstance();

            _lazyInstance       = _previousInstance;
            _instanceGameObject = _previousGameObject;
            _previousInstance   = null;
            _previousGameObject = default;

            _isTesting.Value = false;
        }
#endif // UNITY_INCLUDE_TESTS
    }
}
