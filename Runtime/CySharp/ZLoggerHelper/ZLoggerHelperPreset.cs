#nullable enable

using System;
using System.Buffers;
using Cysharp.Text;
using Microsoft.Extensions.Logging;
using Rekorn.Tools.Unity;
using Rekorn.Tools.Utils;
using UnityEngine;
using ZLogger;

namespace Rekorn.Tools.ZLoggerHelper
{
    [Serializable]
    public sealed record ZLoggerHelperPreset : ISerializationCallbackReceiver
    {
        private static class TooltipMessage
        {
            public const string LogLevel = @"Unity's LogType to ZLogger's LogLevel mapping
LogType.Log: Trace/Debug/Information
LogType.Warning: Warning/Critical
LogType.Error: Error without Exception
LogException: Error with Exception
";

            public const string LogFormat = @"{0}: LogLevel
{1}: LogCategory
{2}: EventId
{3}: DateTime";

            public const string RollingFileFormat = @"{0}: Year
{1}: Month
{2}: Day
{3}: Sequence";
        }

        public static readonly ZLoggerHelperPreset Default = new();

#region Properties
        [field: Header("Log Message")]
        [field: Tooltip(TooltipMessage.LogLevel)]
        [field: SerializeField] public LogLevel MinimumLevel { get; private set; } = LogLevel.Trace;
        [field: SerializeField] public string GlobalCategory { get; private set; } = "Global";
        [field: Tooltip(TooltipMessage.LogFormat)]
        [field: Multiline]
        [field: SerializeField] public string PrefixFormat { get; private set; } = "[{0}] [{1}] ";
        [field: Tooltip(TooltipMessage.LogFormat)]
        [field: Multiline]
        [field: SerializeField] public string SuffixFormat { get; private set; } = "\n{2}: {3}";

#if UNITY_EDITOR
        [field: Header("Editor")]
        [field: SerializeField] public LogStyle LogStyle { get; private set; } = new(true, Color.white);
#endif // UNITY_EDITOR

        [field: Header("Unity")]
        [field: SerializeField] public bool UseUnityLogging { get; private set; } = true;
        [field: SerializeField] public bool UseStackTrack { get;   private set; } = false;

        [field: Header("Log File")]
        [field: SerializeField] public bool UseFileLogging { get;       private set; } = true;
        [field: SerializeField] public AppDataPath FileDataPath  { get; private set; } = AppDataPath.None;
        [field: SerializeField] public string?     FilePath      { get; private set; } = "Logs/";
        [field: SerializeField] public string?     FileExtension { get; private set; } = ".log";
        [field: SerializeField] public string?     FileName      { get; private set; } = "application";

        [field: Header("Rolling File")]
        [field: SerializeField] public bool UseRollingFileLogging { get;       private set; } = true;
        [field: SerializeField] public AppDataPath RollingFileDataPath  { get; private set; } = AppDataPath.None;
        [field: SerializeField] public string?     RollingFilePath      { get; private set; } = "Logs/";
        [field: SerializeField] public string?     RollingFileExtension { get; private set; } = ".log";
        [field: Tooltip(TooltipMessage.RollingFileFormat)]
        [field: SerializeField] public string RollingFileNameFormat { get; private set; } = "{0:D4}-{1:D2}-{2:D2}_{3:D3}";
        [field: SerializeField] public int RollingFileSizeKB { get;        private set; } = 1024;
#endregion // Properties

#region LogFormat
        public string FileUrl => ZString.Concat(FileDataPath.GetPath().AppendDirectorySeparator(), FilePath, FileName, FileExtension).NormalizePath();

        public string GetRollingFileUrl(DateTimeOffset dateTimeOffset, int sequence)
        {
            var yyyy = dateTimeOffset.Year;
            var mm   = dateTimeOffset.Month;
            var dd   = dateTimeOffset.Day;

            var rollingFileName = GetRollingFileName(yyyy, mm, dd, sequence);
            return GetRollingFileUrl(rollingFileName);
        }

        private string GetRollingFileName(int yyyy, int mm, int dd, int sequence)
        {
            return ZString.Format(RollingFileNameFormat, yyyy, mm, dd, sequence);
        }

        private string GetRollingFileUrl(string rollingFileName)
        {
            return ZString.Concat(RollingFileDataPath.GetPath().AppendDirectorySeparator(), RollingFilePath, rollingFileName, RollingFileExtension).NormalizePath();
        }

        public void FormatUnityPrefix(LogInfo info, IBufferWriter<byte> writer)
        {
#if UNITY_EDITOR
            var stringBuilder = ZString.CreateStringBuilder();

            var isBold   = LogStyle.IsBold;
            var useColor = LogStyle.Color.a > 0f;

            if (isBold) stringBuilder.Append("<b>");
            if (useColor)
            {
                stringBuilder.Append("<color=");
                stringBuilder.Append(LogStyle.Color.ToHtmlStringRGB());
                stringBuilder.Append(">");
            }

            stringBuilder.Append(PrefixFormat);

            if (useColor) stringBuilder.Append("</color>");
            if (isBold) stringBuilder.Append("</b>");

            var format = stringBuilder.ToString();
#else
            var format = PrefixFormat;
#endif // UNITY_EDITOR
            FormatLogInfo(info, writer, format);
        }

        public void FormatUnitySuffix(LogInfo info, IBufferWriter<byte> writer)
        {
            FormatLogInfo(info, writer, SuffixFormat);
        }

        public void FormatFilePrefix(LogInfo info, IBufferWriter<byte> writer)
        {
            FormatLogInfo(info, writer, PrefixFormat);
        }

        public void FormatFileSuffix(LogInfo info, IBufferWriter<byte> writer)
        {
            FormatLogInfo(info, writer, SuffixFormat);

            if (UseStackTrack)
            {
                var stackTrace = Environment.StackTrace;
                ZString.Utf8Format(writer, "\n{0}", stackTrace);
            }
        }

        private void FormatLogInfo(LogInfo info, IBufferWriter<byte> writer, string format)
        {
            var level    = info.LogLevel.ToString();
            var category = info.CategoryName;
            var eventId  = info.EventId.ToString();
            var dateTime = info.Timestamp.ToLocalTime().DateTime;
            ZString.Utf8Format(writer, format, level, category, eventId, dateTime);
        }
#endregion // LogFormat

#region Validation
        public void OnBeforeSerialize()  => Validate();
        public void OnAfterDeserialize() => Validate();

        private void Validate()
        {
            if (RollingFileSizeKB <= 1)
                RollingFileSizeKB = Default.RollingFileSizeKB;
        }
#endregion // Validation
    }
}
