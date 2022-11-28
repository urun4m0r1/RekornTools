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
    [HelpURL("https://github.com/Cysharp/ZLogger")]
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

        [field: Header("Log Message")]
        [field: Tooltip(@"Unity's LogType to ZLogger's LogLevel mapping
LogType.Log: Trace/Debug/Information
LogType.Warning: Warning/Critical
LogType.Error: Error without Exception
LogException: Error with Exception
")]
        [field: SerializeField] public LogLevel LogMinimumLevel { get; private set; } = LogLevel.Trace;
        [field: SerializeField] public string GlobalLogCategory { get; private set; } = "Global";
        [field: Tooltip("{0}: LogCategory")]
        [field: SerializeField] public string LogPrefixFormat { get; private set; } = "<b>[{0}]</b>";
        [field: Tooltip("{0}: LogLevel, {1}: EventId, {2}: DateTime")]
        [field: Multiline]
        [field: SerializeField] public string LogSuffixFormat { get; private set; } = "\n----------\n[{0}] ({1}) {2}";

        [field: Header("Log File")]
        [field: SerializeField] public bool IsFileLogEnabled { get;    private set; } = true;
        [field: SerializeField] public string? LogFilePath      { get; private set; } = "Logs/";
        [field: SerializeField] public string? LogFileExtension { get; private set; } = ".log";
        [field: SerializeField] public string? LogFileName      { get; private set; } = "application";

        [field: Header("Log Rolling File")]
        [field: SerializeField] public bool IsRollingFileLogEnabled { get;    private set; } = true;
        [field: SerializeField] public string? RollingLogFilePath      { get; private set; } = "Logs/";
        [field: SerializeField] public string? RollingLogFileExtension { get; private set; } = ".log";
        [field: Tooltip("{0}: Year, {1}: Month, {2}: Day, {3}: Sequence")]
        [field: SerializeField] public string LogRollingFileNameFormat { get; private set; } = "{0:D4}-{1:D2}-{2:D2}_{3:D3}";
        [field: SerializeField] public int LogFileRollSizeKB { get;           private set; } = 1024;


        public void OnBeforeSerialize()  => Validate();
        public void OnAfterDeserialize() => Validate();

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(GlobalLogCategory))
                GlobalLogCategory = Default.GlobalLogCategory;

            if (string.IsNullOrWhiteSpace(LogPrefixFormat)
             || LogPrefixFormat.Contains("{0}") == false)
                LogPrefixFormat = Default.LogPrefixFormat;

            if (string.IsNullOrWhiteSpace(LogSuffixFormat)
             || LogSuffixFormat.Contains("{0}") == false
             || LogSuffixFormat.Contains("{1}") == false
             || LogSuffixFormat.Contains("{2}") == false)
                LogSuffixFormat = Default.LogSuffixFormat;

            if (string.IsNullOrWhiteSpace(LogRollingFileNameFormat)
             || LogRollingFileNameFormat.Contains("{0}") == false
             || LogRollingFileNameFormat.Contains("{1}") == false
             || LogRollingFileNameFormat.Contains("{2}") == false
             || LogRollingFileNameFormat.Contains("{3}") == false)
                LogRollingFileNameFormat = Default.LogRollingFileNameFormat;
        }
    }
}
