#nullable enable

using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    [CreateAssetMenu(menuName = "ScriptableValue/Level")]
    public class LevelValue : ScriptableObject
    {
        [SerializeField] private IntAction _value;
        [SerializeField] private IntAction _maxValue;

        public void Reset()
        {
            _value.ResetValue();
            _maxValue.ResetValue();
        }

        public float Value => Mathf.Clamp01((float)_value.Value / _maxValue.Value);
    }
}
