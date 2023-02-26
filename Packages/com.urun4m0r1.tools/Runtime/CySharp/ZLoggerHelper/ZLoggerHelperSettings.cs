#nullable enable

using UnityEngine;

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    [CreateAssetMenu(
        fileName = nameof(ZLoggerHelperSettings)
      , menuName = Define.AssetMenuPath + nameof(ZLoggerHelperSettings)
      , order = Define.AssetMenuOrder
    )]
    [HelpURL("https://github.com/Cysharp/ZLogger")]
    public sealed class ZLoggerHelperSettings : ScriptableObject
    {
#if UNITY_EDITOR
        private static readonly string s_settingsPath = nameof(ZLoggerHelperSettings) + "_Editor";
#else
        private static readonly string s_settingsPath = nameof(ZLoggerHelperSettings);
#endif // UNITY_EDITOR

        [SerializeField] private ZLoggerHelperPreset _preset = new();

        internal static ZLoggerHelperPreset GetPreset()
        {
            // FIXME: Resources.Load 는 MonoBehaviour 생성자에서 호출할 수 없는 문제가 있음.
            var settings = Resources.Load<ZLoggerHelperSettings>(s_settingsPath);
            if (settings == null)
            {
                Debug.LogWarning($"{nameof(ZLoggerHelperSettings)} not found at path {s_settingsPath} in Resources folder.");
                return ZLoggerHelperPreset.Default;
            }

            return settings._preset;
        }

        private void Reset()
        {
            _preset = new ZLoggerHelperPreset();
        }
    }
}
