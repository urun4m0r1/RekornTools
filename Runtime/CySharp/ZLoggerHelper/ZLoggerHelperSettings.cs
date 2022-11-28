#nullable enable

using System;
using Microsoft.Extensions.Logging;
using UnityEngine;

namespace Rekorn.Tools.ZLoggerHelper
{
    [CreateAssetMenu(
        fileName = nameof(ZLoggerHelperSettings)
      , menuName = Define.AssetMenuPath + nameof(ZLoggerHelperSettings)
      , order = Define.AssetMenuOrder
    )]
    public sealed class ZLoggerHelperSettings : ScriptableObject
    {
        private static readonly string s_settingsPath = nameof(ZLoggerHelperSettings);

        [field: SerializeField] public ZLoggerHelperPreset Preset { get; private set; } = new();

        internal static ZLoggerHelperPreset GetPreset()
        {
            var settings = Resources.Load<ZLoggerHelperSettings>(s_settingsPath);
            if (settings == null)
            {
                Debug.LogError($"{nameof(ZLoggerHelperSettings)} not found at path {s_settingsPath}");
                return ZLoggerHelperPreset.Default;
            }

            return settings.Preset;
        }

        private void Reset()
        {
            Preset = new ZLoggerHelperPreset();
        }
    }

    [Serializable]
    public sealed class ZLoggerHelperPreset : ISerializationCallbackReceiver
    {
        public static readonly ZLoggerHelperPreset Default = new();

        // LogLevels are translate to
        // * Trace/Debug/Information -> LogType.Log
        // * Warning/Critical        -> LogType.Warning
        // * Error without Exception -> LogType.Error
        // * Error with Exception    -> LogException
        [field: SerializeField] public LogLevel LogMinimumLevel { get; private set; } = LogLevel.Trace;

        [field: SerializeField] public string GlobalLogCategory { get; private set; } = "Global";

        // LogCategory
        [field: SerializeField] public string LogPrefixFormat { get; private set; } = "<b>[{0}]</b>";

        // LogLevel, EventId, DateTime
        [field: SerializeField] public string LogSuffixFormat { get; private set; } = "\n\n[{0}] ({1}) {2}";

        [field: SerializeField] public bool IsFileLogEnabled        { get; private set; } = true;
        [field: SerializeField] public bool IsRollingFileLogEnabled { get; private set; } = true;

        [field: SerializeField] public string? LogFilePath      { get; private set; } = "Logs/";
        [field: SerializeField] public string? LogFileName      { get; private set; } = "application";
        [field: SerializeField] public string? LogFileExtension { get; private set; } = ".log";

        // YYYY-MM-DD_NNN
        [field: SerializeField] public string LogRollingFileNameFormat { get; private set; } = "{0:D4}-{1:D2}-{2:D2}_{3:D3}";

        [field: SerializeField] public int LogFileRollSizeKB { get; private set; } = 1024;

        public void OnBeforeSerialize()  => Validate();
        public void OnAfterDeserialize() => Validate();

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(GlobalLogCategory))
                GlobalLogCategory = Default.GlobalLogCategory;

            if (string.IsNullOrWhiteSpace(LogPrefixFormat))
                LogPrefixFormat = Default.LogPrefixFormat;

            if (string.IsNullOrWhiteSpace(LogSuffixFormat))
                LogSuffixFormat = Default.LogSuffixFormat;

            if (string.IsNullOrWhiteSpace(LogRollingFileNameFormat))
                LogRollingFileNameFormat = Default.LogRollingFileNameFormat;
        }
    }
}
