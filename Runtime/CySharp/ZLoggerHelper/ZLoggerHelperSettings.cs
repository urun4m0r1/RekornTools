#nullable enable

using UnityEngine;

namespace Rekorn.Tools.ZLoggerHelper
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
