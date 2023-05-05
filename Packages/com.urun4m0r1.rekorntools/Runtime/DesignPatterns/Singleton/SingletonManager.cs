#if UNITY_EDITOR

#nullable enable

using System;
using System.Reflection;
using UnityEngine;

namespace Urun4m0r1.RekornTools.DesignPatterns
{
    internal static class SingletonManager
    {
        [RuntimeInitializeOnLoadMethod(SingletonDefine.SingletonDestroyLoadType)]
        private static void OnRuntimeInitialize()
        {
            if (!SingletonDefine.DestroySingletonInstancesOnLoad)
                return;

            DestroySingletonInstances();
        }

        private static void DestroySingletonInstances()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var isConcreteClass = type is { IsClass: true, IsAbstract: false, IsGenericType: false };
                    if (!isConcreteClass)
                        continue;

                    if (TryInvokeDestroySingletonInstanceMethod(type, typeof(MonoSingleton<>))) continue;
                    if (TryInvokeDestroySingletonInstanceMethod(type, typeof(Singleton<>))) continue;
                }
            }
        }

        private static bool TryInvokeDestroySingletonInstanceMethod(Type? targetType, Type? genericType)
        {
            if (targetType == null || genericType == null)
                return false;

            if (!IsSubclassOfRawGeneric(targetType, genericType))
                return false;

            var type = genericType.MakeGenericType(targetType);
            InvokeDestroySingletonInstanceMethod(type);
            return true;
        }

        private static bool IsSubclassOfRawGeneric(Type? targetType, Type? genericType)
        {
            while (targetType != null && targetType != typeof(object))
            {
                var currentType = targetType.IsGenericType ? targetType.GetGenericTypeDefinition() : targetType;
                if (currentType == genericType)
                    return true;

                targetType = targetType.BaseType;
            }

            return false;
        }

        private static void InvokeDestroySingletonInstanceMethod(Type? type)
        {
            type?.GetMethod("DestroySingletonInstance", BindingFlags.Public | BindingFlags.Static)?.Invoke(null!, null!);
        }
    }
}
#endif // UNITY_EDITOR
