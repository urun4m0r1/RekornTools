using System.Collections.Generic;
using UnityEngine;

namespace Rekorn.Tools.DesignPatterns
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private List<ObjectPool> pools;

        public void ResetAllPools()
        {
            foreach (var pool in pools)
            {
                pool.ResetPool();
            }
        }
    }
}
