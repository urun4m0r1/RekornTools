#nullable enable

using System;

namespace Urun4m0r1.RekornTools.DesignPatterns
{
    /// <inheritdoc />
    public class PersistantSingleton<T> : ISingleton where T : class
    {
        private static readonly Lazy<T> LazyInstance = SingletonHelper<T>.GenerateThreadSafeLazyInstance();

#region InstanceAccess
        public static bool HasInstance => LazyInstance.IsValueCreated;

        public static T? InstanceOrNull => HasInstance ? LazyInstance.Value : null;

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
        private static T GetOrCreateInstance() => LazyInstance.Value ?? throw SingletonHelper<T>.InstanceNullException;
#endregion // InstanceGeneration
    }
}
