#nullable enable

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Urun4m0r1.RekornTools.DesignPatterns
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private Poolable poolObj;
        [SerializeField] private int initialAllocateCount = 10;
        [SerializeField] private int additionalAllocationCount = 5;

        internal void RegisterReset(Action onReset)
        {
            throw new NotImplementedException();
        }

        private Stack<Poolable> _poolStack = new Stack<Poolable>();
        private List<Action> _onReset = new List<Action>();

        private void Start()
        {
            Allocate(initialAllocateCount);
        }

        private void Allocate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Poolable? allocateObj = Instantiate(poolObj, this.gameObject.transform);
                allocateObj.Create(this);
                _poolStack.Push(allocateObj);
            }
        }

        public GameObject Pop()
        {
            //if every poolstack is active, allocate new object
            if (_poolStack.Count == 0)
            {
                Allocate(additionalAllocationCount);
            }

            Poolable? obj = _poolStack.Pop();
            obj.gameObject.SetActive(true);
            return obj.gameObject;
        }

        public void Push(Poolable obj)
        {
            // TODO: Push를 까먹으면 좀비 오브젝트가 생길 수 있음
            obj.gameObject.SetActive(false);
            _poolStack.Push(obj);
        }

        public void RegisterResetAction(Action? onReset)
        {
            _onReset.Add(onReset);
        }

        public void ResetPool()
        {
            foreach (var item in _onReset)
            {
                item();
            }
        }
    }
}
