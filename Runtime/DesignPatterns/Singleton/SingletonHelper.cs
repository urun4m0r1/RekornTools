﻿#nullable enable

using System;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace Rekorn.Tools.DesignPatterns
{
    internal static class SingletonHelper<T> where T : class
    {
        public static string BaseTypeName => typeof(T).BaseType.Name.Replace("`1", "");
        public static string TypeName     => typeof(T).Name;

        public static string GenericTypeName => $"{BaseTypeName}<T>";
        public static string ClassName       => $"{BaseTypeName}<{TypeName}>";

#region Exceptions
        public static InvalidOperationException InstanceNullException            => new($"{ClassName} instance is null.");
        public static InvalidOperationException InstanceNullOrDestroyedException => new($"{ClassName} instance is null or destroyed.");
        public static InvalidOperationException InstanceAlreadyCreatedException  => new($"{ClassName} instance already created.");
#endregion // Exceptions

#region InstanceGeneration
        public static Lazy<T> GenerateThreadSafeLazyInstance() => GenerateThreadSafeLazyInstance(GenerateInstance);

        public static Lazy<T> GenerateThreadSafeLazyInstance(Func<T> valueFactory) => new(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);

        public static T GenerateInstance()
        {
            if (SingletonHelper.GetConstructor(typeof(T)).Invoke(null!) is not T instance)
            {
                throw new MissingMethodException($"{ClassName} has a non-public constructor that does not return an instance of {TypeName}.");
            }

            LogInstanceCreated();
            return instance;
        }
#endregion // InstanceGeneration

#region Logging
        public static void LogInstanceCreated()
        {
            Debug.Log(GenericTypeName, $"<{TypeName}> instance created.");
        }

        public static void LogInstanceDestroyed()
        {
            Debug.Log(GenericTypeName, $"<{TypeName}> instance destroyed.");
        }
#endregion // Logging

#region Validation
        public static void EnsureApplicationIsPlaying()
        {
            if (!Application.isPlaying)
            {
                throw new InvalidOperationException($"{ClassName} cannot be created while the application is not playing or quitting.");
            }
        }
#endregion // Validation
    }

    internal static class SingletonHelper
    {
        /// <summary>
        /// Get unique non-public parameterless constructor of the specified type.
        /// </summary>
        public static ConstructorInfo GetConstructor(Type type)
        {
            var publicConstructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (publicConstructors.Length > 0)
            {
                throw new NotSupportedException($"{type.Name} has {publicConstructors.Length} public constructors (or default). No public constructors are allowed.");
            }

            var nonPublicConstructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            if (nonPublicConstructors.Length != 1)
            {
                throw new NotSupportedException($"{type.Name} has {nonPublicConstructors.Length} non-public constructors. Only one non-public constructor is allowed.");
            }

            var parameters = nonPublicConstructors[0].GetParameters();
            if (parameters.Length > 0)
            {
                throw new NotSupportedException($"{type.Name} has a non-public constructor with {parameters.Length} parameters. No parameters are allowed.");
            }

            return nonPublicConstructors[0];
        }
    }
}
