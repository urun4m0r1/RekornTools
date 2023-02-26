#nullable enable

using System;

namespace Urun4m0r1.RekornTools.DesignPatterns
{
    /// <inheritdoc />
    public class PersistantSingleton<T> : ISingleton where T : class
    {
        private static readonly Lazy<T> s_lazyInstance = SingletonHelper<T>.GenerateThreadSafeLazyInstance();

#region InstanceAccess
        public static bool HasInstance => s_lazyInstance.IsValueCreated;

        public static T? InstanceOrNull => HasInstance ? s_lazyInstance.Value : null;

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
        private static T GetOrCreateInstance() => s_lazyInstance.Value ?? throw SingletonHelper<T>.InstanceNullException;
#endregion // InstanceGeneration
    }
}
