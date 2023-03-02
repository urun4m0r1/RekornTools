#nullable enable

using System.Collections.Generic;
using System.Linq;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class KeyValuePairExtensions
    {
        // Target:   1 2 4 5
        // Preserve: 1 3 4
        // Result:   1 4
        public static void ExceptKeys<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue?>> target, IEnumerable<TKey> keysToPreserve, IEqualityComparer<TKey>? comparer = null)
        {
            comparer ??= EqualityComparer<TKey>.Default;

            var pairsToRemove = from pair in target
                                where !keysToPreserve.Contains(pair.Key, comparer)
                                select pair;

            if (target is IDictionary<TKey, TValue?> dictionary)
            {
                foreach (var pair in pairsToRemove)
                    dictionary.Remove(pair.Key);

                return;
            }

            foreach (var pair in pairsToRemove)
                target.Remove(pair);
        }

        public static void RemoveKeys<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue?>> target, IEnumerable<TKey> keysToRemove, IEqualityComparer<TKey>? comparer = null)
        {
            comparer ??= EqualityComparer<TKey>.Default;

            foreach (var keyToRemove in keysToRemove)
                target.Remove(keyToRemove, comparer);
        }

        public static bool Remove<TKey, TValue>(this ICollection<KeyValuePair<TKey, TValue?>> target, TKey key, IEqualityComparer<TKey>? comparer = null)
        {
            if (target is IDictionary<TKey, TValue?> dictionary)
                return dictionary.Remove(key);

            comparer ??= EqualityComparer<TKey>.Default;

            if (!target.TryGetValue(key, out var value, comparer))
                return false;

            var pair = new KeyValuePair<TKey, TValue?>(key, value);
            return target.Remove(pair);
        }

        public static bool ContainsKey<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue?>> target, TKey key, IEqualityComparer<TKey>? comparer = null)
        {
            if (target is IDictionary<TKey, TValue?> dictionary)
                return dictionary.ContainsKey(key);

            if (target is IReadOnlyDictionary<TKey, TValue?> readOnlyDictionary)
                return readOnlyDictionary.ContainsKey(key);

            comparer ??= EqualityComparer<TKey>.Default;

            foreach (var pair in target)
            {
                if (comparer.Equals(pair.Key, key))
                    return true;
            }

            return false;
        }

        public static bool ContainsValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue?>> target, TValue? value, IEqualityComparer<TValue?>? comparer = null)
        {
            if (target is Dictionary<TKey, TValue?> dictionary)
                return dictionary.ContainsValue(value);

            comparer ??= EqualityComparer<TValue?>.Default;

            foreach (var pair in target)
            {
                if (comparer.Equals(pair.Value, value))
                    return true;
            }

            return false;
        }

        public static bool TryGetValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue?>> target, TKey key, out TValue? value, IEqualityComparer<TKey>? comparer = null)
        {
            if (target is IDictionary<TKey, TValue?> dictionary)
                return dictionary.TryGetValue(key, out value);

            if (target is IReadOnlyDictionary<TKey, TValue?> readOnlyDictionary)
                return readOnlyDictionary.TryGetValue(key, out value);

            comparer ??= EqualityComparer<TKey>.Default;

            foreach (var pair in target)
            {
                if (comparer.Equals(pair.Key, key))
                {
                    value = pair.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}
