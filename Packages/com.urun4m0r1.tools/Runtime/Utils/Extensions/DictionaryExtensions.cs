#nullable enable

using System;
using System.Collections.Generic;

namespace Rekorn.Tools.Utils
{
    public static class DictionaryExtensions
    {
#region EnumDictionaryKeyUpdater
        public static void UpdateKeysWithDefaultValues<TKey, TValue>(this IDictionary<TKey, TValue> target) where TKey : Enum where TValue : struct
        {
            var keys = EnumUtility.GetValuesList<TKey>();
            target.UpdateKeysWithDefaultValues(keys);
        }

        public static void UpdateKeysWithNullValues<TKey, TValue>(this IDictionary<TKey, TValue?> target) where TKey : Enum where TValue : class
        {
            var keys = EnumUtility.GetValuesList<TKey>();
            target.UpdateKeysWithNullValues(keys);
        }

        public static void UpdateKeysWithNewValues<TKey, TValue>(this IDictionary<TKey, TValue> target) where TKey : Enum where TValue : new()
        {
            var keys = EnumUtility.GetValuesList<TKey>();
            target.UpdateKeysWithNewValues(keys);
        }

        public static void UpdateKeysWithValueFactory<TKey, TValue>(this IDictionary<TKey, TValue> target, Func<TKey, TValue> valueFactory) where TKey : Enum
        {
            var keys = EnumUtility.GetValuesList<TKey>();
            target.UpdateKeysWithValueFactory(keys, valueFactory);
        }
#endregion // EnumDictionaryKeyUpdater

#region DictionaryKeyUpdater
        public static void UpdateKeysWithDefaultValues<TKey, TValue>(this IDictionary<TKey, TValue> target, IReadOnlyCollection<TKey> keysToUpdate) where TValue : struct
        {
            target.ExceptKeys(keysToUpdate);
            target.AppendKeysWithDefaultValues(keysToUpdate);
        }

        public static void UpdateKeysWithNullValues<TKey, TValue>(this IDictionary<TKey, TValue?> target, IReadOnlyCollection<TKey> keysToUpdate) where TValue : class
        {
            target.ExceptKeys(keysToUpdate);
            target.AppendKeysWithNullValues(keysToUpdate);
        }

        public static void UpdateKeysWithNewValues<TKey, TValue>(this IDictionary<TKey, TValue> target, IReadOnlyCollection<TKey> keysToUpdate) where TValue : new()
        {
            target.ExceptKeys(keysToUpdate);
            target.AppendKeysWithNewValues(keysToUpdate);
        }

        public static void UpdateKeysWithValueFactory<TKey, TValue>(this IDictionary<TKey, TValue> target, IReadOnlyCollection<TKey> keysToUpdate, Func<TKey, TValue> valueFactory)
        {
            target.ExceptKeys(keysToUpdate);
            target.AppendKeysWithValueFactory(keysToUpdate, valueFactory);
        }
#endregion // DictionaryKeyUpdater

        private static void AppendKeysWithDefaultValues<TKey, TValue>(this IDictionary<TKey, TValue> target, IEnumerable<TKey> keysToAppend) where TValue : struct
        {
            foreach (var keyToAppend in keysToAppend)
            {
                if (!target.ContainsKey(keyToAppend))
                {
                    target.Add(keyToAppend, default);
                }
            }
        }

        private static void AppendKeysWithNullValues<TKey, TValue>(this IDictionary<TKey, TValue?> target, IEnumerable<TKey> keysToAppend) where TValue : class
        {
            foreach (var keyToAppend in keysToAppend)
            {
                if (!target.ContainsKey(keyToAppend))
                {
                    target.Add(keyToAppend, null);
                }
            }
        }

        private static void AppendKeysWithNewValues<TKey, TValue>(this IDictionary<TKey, TValue> target, IEnumerable<TKey> keysToAppend) where TValue : new()
        {
            foreach (var keyToAppend in keysToAppend)
            {
                if (!target.ContainsKey(keyToAppend))
                {
                    target.Add(keyToAppend, new TValue());
                }
            }
        }

        private static void AppendKeysWithValueFactory<TKey, TValue>(this IDictionary<TKey, TValue> target, IEnumerable<TKey> keysToAppend, Func<TKey, TValue> valueFactory)
        {
            foreach (var keyToAppend in keysToAppend)
            {
                if (!target.ContainsKey(keyToAppend))
                {
                    target.Add(keyToAppend, valueFactory(keyToAppend));
                }
            }
        }

        private static void RemoveKeys<TKey, TValue>(this IDictionary<TKey, TValue> target, IEnumerable<TKey> keysToRemove)
        {
            foreach (var keyToRemove in keysToRemove)
            {
                if (target.ContainsKey(keyToRemove))
                {
                    target.Remove(keyToRemove);
                }
            }
        }

        private static void ExceptKeys<TKey, TValue>(this IDictionary<TKey, TValue> target, IEnumerable<TKey> keysToPreserve)
        {
            foreach (var keyToPreserve in keysToPreserve)
            {
                if (!target.ContainsKey(keyToPreserve))
                {
                    target.Remove(keyToPreserve);
                }
            }
        }
    }
}
