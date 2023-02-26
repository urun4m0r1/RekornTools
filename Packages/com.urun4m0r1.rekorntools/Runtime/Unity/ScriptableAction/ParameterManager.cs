#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    public class ParameterManager : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> _parameters = new List<ScriptableObject>();

        public void ResetAllParameters()
        {
            foreach (var parameter in _parameters)
            {
                (parameter as IResetable).ResetValue();
            }
        }
    }
}
