#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Urun4m0r1.RekornTools.DesignPatterns
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private List<ObjectPool> _pools;

        public void ResetAllPools()
        {
            foreach (var pool in _pools)
            {
                pool.ResetPool();
            }
        }
    }
}
