#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using Urun4m0r1.RekornTools.DesignPatterns;
using UnityEngine;

namespace Urun4m0r1.RekornTools
{
    [DisallowMultipleComponent]
    [AddComponentMenu("RekornTools/Mono Singleton/Coroutine Runner")]
    public sealed class CoroutineRunner : MonoSingleton<CoroutineRunner>, IDisposable, IRegisterable<IEnumerator>
    {
        private readonly Dictionary<IEnumerator, Coroutine> _coroutines = new();

#region IRegisterable<IEnumerator>
        public bool Contains(IEnumerator item) => _coroutines.ContainsKey(item);

        public void Add(IEnumerator item)
        {
            if (_coroutines.ContainsKey(item))
            {
                throw new ArgumentException($"{nameof(CoroutineRunner)} already contains {item}");
            }

            _coroutines.Add(item, StartCoroutine(item));
        }

        public bool TryAdd(IEnumerator item)
        {
            if (_coroutines.ContainsKey(item))
            {
                return false;
            }

            _coroutines.Add(item, StartCoroutine(item));
            return true;
        }

        public void Remove(IEnumerator item)
        {
            if (_coroutines.TryGetValue(item, out var coroutine))
            {
                StopCoroutine(coroutine!);
                _coroutines.Remove(item);
            }
        }
#endregion // IRegisterable<IEnumerator>

#region Destruction
        public void Dispose()
        {
            Clear();
            TrimExcess();
        }

        public void Clear()
        {
            foreach (var coroutine in _coroutines.Values)
            {
                StopCoroutine(coroutine);
            }

            _coroutines.Clear();
        }

        public void TrimExcess()
        {
            _coroutines.TrimExcess();
        }
#endregion // Destruction
    }
}
