using System.Collections.Generic;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    public class ParameterManager : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> parameters = new List<ScriptableObject>();

        public void ResetAllParameters()
        {
            foreach (var parameter in parameters)
            {
                (parameter as IResetable).ResetValue();
            }
        }
    }
}
