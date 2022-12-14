using UnityEngine;

namespace Rekorn.Tools.Unity
{
    [CreateAssetMenu(menuName = "ScriptableValue/Level")]
    public class LevelValue : ScriptableObject
    {
        [SerializeField] private IntAction value;
        [SerializeField] private IntAction maxValue;

        public void Reset()
        {
            value.ResetValue();
            maxValue.ResetValue();
        }

        public float Value => Mathf.Clamp01((float)value.Value / maxValue.Value);
    }
}
