#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Urun4m0r1.RekornTools.Utils
{
    [Serializable]
    public class Cache<TKey, TValue> :
        IDictionary<TKey, TValue?>,
        IReadOnlyDictionary<TKey, TValue?>,
        IDictionary,
        IDeserializationCallback,
        ISerializable,
        IDisposable
        where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue?> _cache = new();
        private readonly Func<TKey, TValue?>?      _valueFactory;
        private readonly EqualityComparer<TValue?> _valueEqualityComparer;

        private static readonly bool s_isReferenceTypeValue = typeof(TValue).IsClass;

        public Cache(Func<TKey, TValue?>?       valueFactory          = null
                   , EqualityComparer<TValue?>? valueEqualityComparer = null)
        {
            _valueFactory          = valueFactory;
            _valueEqualityComparer = valueEqualityComparer ?? EqualityComparer<TValue?>.Default;
        }

#region Cache
        public bool IsCached(TKey key)
        {
            return ContainsKey(key);
        }

        public TValue? Request(TKey key)
        {
            if (TryGetValue(key, out var value))
                return value;

            value = GenerateValue(key);
            Add(key, value);
            return value;
        }

        private TValue? GenerateValue(TKey key)
        {
            return _valueFactory is null
                ? default
                : _valueFactory(key);
        }

        public bool Remove(TKey key)
        {
            if (!TryGetValue(key, out var value))
                return false;

            DisposeValue(value);
            return _cache.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue?> item)
        {
            var queryKey   = item.Key!;
            var queryValue = item.Value;

            if (!TryGetValue(queryKey, out var currentValue))
                return false;

            if (!IsValueEquals(currentValue, queryValue))
                return false;

            DisposeValue(currentValue);
            return _cache.Remove(queryKey);
        }

        void IDictionary.Remove(object key)
        {
            if (key is not TKey tKey)
                return;

            Remove(tKey);
        }

        public void Reset()
        {
            Clear();
            TrimExcess();
        }

        public void Clear()
        {
            DisposeValues();
            _cache.Clear();
        }

        public TValue? this[TKey key]
        {
            get => _cache[key];
            set
            {
                var currentValue = _cache[key];
                var newValue     = value;

                if (IsValueEquals(currentValue, newValue))
                    return;

                DisposeValue(currentValue);
                _cache[key] = newValue;
            }
        }

        public object? this[object key]
        {
            get => IDictionary[key];
            set => this[GetKey(key)] = GetValue(value);
        }

        private static TKey GetKey(object key) => key switch
        {
            TKey tKey => tKey,
            _         => throw new KeyNotFoundException($"The key must be of type {typeof(TKey)}. Actual type: {key.GetType()}"),
        };

        private static TValue? GetValue(object? value) => value switch
        {
            TValue tValue => tValue,
            null          => default,
            _             => throw new ArgumentException($"The value must be of type {typeof(TValue)} or null. Actual type: {value.GetType()}"),
        };

        private bool IsValueEquals(TValue? lhs, TValue? rhs)
        {
            return _valueEqualityComparer.Equals(lhs, rhs);
        }
#endregion // Cache

#region IDisposable
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposeManaged)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                if (disposeManaged)
                    DisposeManagedResources();

                DisposeUnmanagedResources();
            }
        }
#endregion // IDisposable

        private void DisposeManagedResources()
        {
            Clear();
        }

        private void DisposeUnmanagedResources()
        {
            // Nothing to do
        }

        private void DisposeValue(TValue? value)
        {
            if (!s_isReferenceTypeValue)
                return;

            (value as IDisposable)?.Dispose();
        }

        private void DisposeValues()
        {
            if (!s_isReferenceTypeValue)
                return;

            foreach (var value in Values)
                (value as IDisposable)?.Dispose();
        }

#region InternalInterfaces
        private IReadOnlyDictionary<TKey, TValue?>       ReadOnlyDictionary => _cache;
        private IDictionary<TKey, TValue?>               Dictionary         => _cache;
        private ICollection<KeyValuePair<TKey, TValue?>> Collection         => _cache;
        private IEnumerable<KeyValuePair<TKey, TValue?>> Enumerable         => _cache;

        // ReSharper disable InconsistentNaming
        private IDictionary              IDictionary              => _cache;
        private ICollection              ICollection              => _cache;
        private IEnumerable              IEnumerable              => _cache;
        private IDeserializationCallback IDeserializationCallback => _cache;
        private ISerializable            ISerializable            => _cache;
        // ReSharper restore InconsistentNaming
#endregion // InternalInterfaces

#region DictionaryImplements
        public Dictionary<TKey, TValue?>.KeyCollection   Keys   => _cache.Keys;
        public Dictionary<TKey, TValue?>.ValueCollection Values => _cache.Values;

        public void TrimExcess()                 => _cache.TrimExcess();
        public bool ContainsValue(TValue? value) => _cache.ContainsValue(value);
#endregion // DictionaryImplements

#region GenericImplements
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue?>.   Keys   => ReadOnlyDictionary.Keys;
        IEnumerable<TValue?> IReadOnlyDictionary<TKey, TValue?>.Values => ReadOnlyDictionary.Values;

        ICollection<TKey> IDictionary<TKey, TValue?>.   Keys   => Dictionary.Keys;
        ICollection<TValue?> IDictionary<TKey, TValue?>.Values => Dictionary.Values;

        public int  Count                                    => Dictionary.Count;
        public bool ContainsKey(TKey key)                    => Dictionary.ContainsKey(key);
        public bool TryGetValue(TKey key, out TValue? value) => Dictionary.TryGetValue(key, out value);
        public void Add(TKey         key, TValue?     value) => Dictionary.Add(key, value);

        public bool IsReadOnly                                                  => Collection.IsReadOnly;
        public bool Contains(KeyValuePair<TKey, TValue?> item)                  => Collection.Contains(item);
        public void Add(KeyValuePair<TKey, TValue?>      item)                  => Collection.Add(item);
        public void CopyTo(KeyValuePair<TKey, TValue?>[] array, int arrayIndex) => Collection.CopyTo(array, arrayIndex);

        IEnumerator<KeyValuePair<TKey, TValue?>> IEnumerable<KeyValuePair<TKey, TValue?>>.GetEnumerator() => Enumerable.GetEnumerator();
#endregion // GenericImplements

#region NonGenericImplements
        bool IDictionary.                 IsFixedSize                        => IDictionary.IsFixedSize;
        bool IDictionary.                 IsReadOnly                         => IDictionary.IsReadOnly;
        ICollection IDictionary.          Keys                               => IDictionary.Keys;
        ICollection IDictionary.          Values                             => IDictionary.Values;
        IDictionaryEnumerator IDictionary.GetEnumerator()                    => IDictionary.GetEnumerator();
        bool IDictionary.                 Contains(object key)               => IDictionary.Contains(key);
        void IDictionary.                 Add(object      key, object value) => IDictionary.Add(key, value);

        int ICollection.   Count                          => ICollection.Count;
        object ICollection.SyncRoot                       => ICollection.SyncRoot;
        bool ICollection.  IsSynchronized                 => ICollection.IsSynchronized;
        void ICollection.  CopyTo(Array array, int index) => ICollection.CopyTo(array, index);

        IEnumerator IEnumerable.GetEnumerator() => IEnumerable.GetEnumerator();

        void IDeserializationCallback.OnDeserialization(object sender) => IDeserializationCallback.OnDeserialization(sender);

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => ISerializable.GetObjectData(info, context);
#endregion // NonGenericImplements
    }
}
