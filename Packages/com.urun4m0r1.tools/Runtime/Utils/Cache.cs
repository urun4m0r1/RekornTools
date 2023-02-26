#nullable enable

using System;
using System.Collections.Generic;

namespace Rekorn.Tools.Utils
{
    public class Cache<TKey, TValue> : IDisposable where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue?> _cache = new();

        private readonly Func<TKey, TValue?> _generator;

        public Cache(Func<TKey, TValue?> generator) => _generator = generator;

        public bool IsCached(TKey key) => _cache.ContainsKey(key);

        public bool IsCached(TValue value) => _cache.ContainsValue(value);

        public TValue? Request(TKey key)
        {
            if (!_cache.TryGetValue(key, out var value))
            {
                value = _generator(key);
                _cache.Add(key, value);
            }

            return value;
        }

        public void Add(TKey key, TValue value) => _cache.TryAdd(key, value);

        public void Remove(TKey key) => _cache.Remove(key);

        public IEnumerable<TKey> Keys => _cache.Keys;

        public IEnumerable<TValue?> Values => _cache.Values;

        public void Clear()
        {
            _cache.Clear();
            _cache.TrimExcess();
        }

#region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                DisposeManagedResources();
            }

            _disposed = true;
        }

        private void DisposeManagedResources()
        {
            Clear();
        }
#endregion // IDisposable
    }
}
