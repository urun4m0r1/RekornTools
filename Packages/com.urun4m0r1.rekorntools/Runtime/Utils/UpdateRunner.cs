#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;
using Urun4m0r1.RekornTools.DesignPatterns;
using Urun4m0r1.RekornTools.Utils;

namespace Urun4m0r1.RekornTools
{
    [DisallowMultipleComponent]
    [AddComponentMenu("RekornTools/Mono Singleton/Update Runner")]
    public sealed class UpdateRunner : MonoSingleton<UpdateRunner>, IDisposable, IRegisterable<IUpdatable>, IRegisterable<IFixedUpdatable>, IRegisterable<ILateUpdatable>
    {
#region UpdateCallbacks
        private readonly HashSet<IUpdatable>      _updatables      = new();
        private readonly HashSet<IFixedUpdatable> _fixedUpdatables = new();
        private readonly HashSet<ILateUpdatable>  _lateUpdatables  = new();

        private readonly Queue<IUpdatable>      _removingUpdatables      = new();
        private readonly Queue<IFixedUpdatable> _removingFixedUpdatables = new();
        private readonly Queue<ILateUpdatable>  _removingLateUpdatables  = new();

        private void Update()
        {
            foreach (var target in _removingUpdatables)
            {
                _updatables.Remove(target);
            }

            var deltaTime = Time.deltaTime;
            foreach (var updatable in _updatables)
            {
                if (updatable.IsUpdateEnabled)
                {
                    updatable.OnUpdate(deltaTime);
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (var target in _removingFixedUpdatables)
            {
                _fixedUpdatables.Remove(target);
            }

            var deltaTime = Time.fixedDeltaTime;
            foreach (var updatable in _fixedUpdatables)
            {
                if (updatable.IsFixedUpdateEnabled)
                {
                    updatable.OnFixedUpdate(deltaTime);
                }
            }
        }

        private void LateUpdate()
        {
            foreach (var target in _removingLateUpdatables)
            {
                _lateUpdatables.Remove(target);
            }

            var deltaTime = Time.deltaTime;
            foreach (var updatable in _lateUpdatables)
            {
                if (updatable.IsLateUpdateEnabled)
                {
                    updatable.OnLateUpdate(deltaTime);
                }
            }
        }
#endregion // UpdateCallbacks

#region IRegisterable<IUpdatable>
        public bool Contains(IUpdatable item) => _updatables.Contains(item);
        public void Add(IUpdatable      item) => _updatables.Add(item);
        public bool TryAdd(IUpdatable   item) => _updatables.Add(item);
        public void Remove(IUpdatable   item) => _removingUpdatables.TryEnqueue(item);
#endregion // IRegisterable<IUpdatable>

#region IRegisterable<IFixedUpdatable>
        public bool Contains(IFixedUpdatable item) => _fixedUpdatables.Contains(item);
        public void Add(IFixedUpdatable      item) => _fixedUpdatables.Add(item);
        public bool TryAdd(IFixedUpdatable   item) => _fixedUpdatables.Add(item);
        public void Remove(IFixedUpdatable   item) => _removingFixedUpdatables.TryEnqueue(item);
#endregion // IRegisterable<IFixedUpdatable>

#region IRegisterable<ILateUpdatable>
        public bool Contains(ILateUpdatable item) => _lateUpdatables.Contains(item);
        public void Add(ILateUpdatable      item) => _lateUpdatables.Add(item);
        public bool TryAdd(ILateUpdatable   item) => _lateUpdatables.Add(item);
        public void Remove(ILateUpdatable   item) => _removingLateUpdatables.TryEnqueue(item);
#endregion // IRegisterable<ILateUpdatable>

#region Destruction
        public void Dispose()
        {
            Clear();
            TrimExcess();
        }

        public void Clear()
        {
            _updatables.Clear();
            _fixedUpdatables.Clear();
            _lateUpdatables.Clear();

            _removingUpdatables.Clear();
            _removingFixedUpdatables.Clear();
            _removingLateUpdatables.Clear();
        }

        public void TrimExcess()
        {
            _updatables.TrimExcess();
            _fixedUpdatables.TrimExcess();
            _lateUpdatables.TrimExcess();

            _removingUpdatables.TrimExcess();
            _removingFixedUpdatables.TrimExcess();
            _removingLateUpdatables.TrimExcess();
        }
#endregion // Destruction
    }
}
