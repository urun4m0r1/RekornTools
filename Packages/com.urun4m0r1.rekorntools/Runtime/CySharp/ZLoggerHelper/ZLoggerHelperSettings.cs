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
        [SerializeField] private ZLoggerHelperPreset _preset = new();

        private static string GetSettingsPath()
        {
#if UNITY_EDITOR
            return "Editor/" + nameof(ZLoggerHelperSettings);
#else
            return nameof(ZLoggerHelperSettings);
#endif // UNITY_EDITOR
        }

        internal static ZLoggerHelperPreset GetPreset()
        {
            var settingsPath = GetSettingsPath();
            var settings     = Resources.Load<ZLoggerHelperSettings>(settingsPath);
            if (ReferenceEquals(settings!, null!))
            {
                Debug.LogWarning($"{nameof(ZLoggerHelperSettings)} not found at path {settingsPath} in Resources folder.");
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
