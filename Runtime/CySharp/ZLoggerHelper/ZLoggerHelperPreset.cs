﻿#nullable enable

using System;
using System.Buffers;
using Cysharp.Text;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;

namespace Rekorn.Tools.ZLoggerHelper
{
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
        [field: SerializeField] public LogLevel MinimumLevel { get; private set; } = LogLevel.Trace;
        [field: SerializeField] public string GlobalCategory { get; private set; } = "Global";
        [field: Tooltip("{0}: LogCategory")]
        [field: SerializeField] public string PrefixFormat { get; private set; } = "<b>[{0}]</b>";
        [field: Tooltip("{0}: LogLevel, {1}: EventId, {2}: DateTime")]
        [field: Multiline]
        [field: SerializeField] public string SuffixFormat { get; private set; } = "\n----------\n[{0}] ({1}) {2}";

        [field: Header("Unity")]
        [field: SerializeField] public bool UseUnityLogging { get; private set; } = true;

        [field: Header("Log File")]
        [field: SerializeField] public bool UseFileLogging { get;   private set; } = true;
        [field: SerializeField] public string? FilePath      { get; private set; } = "Logs/";
        [field: SerializeField] public string? FileExtension { get; private set; } = ".log";
        [field: SerializeField] public string? FileName      { get; private set; } = "application";

        [field: Header("Rolling File")]
        [field: SerializeField] public bool UseRollingFileLogging { get;   private set; } = true;
        [field: SerializeField] public string? RollingFilePath      { get; private set; } = "Logs/";
        [field: SerializeField] public string? RollingFileExtension { get; private set; } = ".log";
        [field: Tooltip("{0}: Year, {1}: Month, {2}: Day, {3}: Sequence")]
        [field: SerializeField] public string RollingFileNameFormat { get; private set; } = "{0:D4}-{1:D2}-{2:D2}_{3:D3}";
        [field: SerializeField] public int RollingFileSizeKB { get;        private set; } = 1024;

        public string FileUrl => ZString.Concat(FilePath, FileName, FileExtension);

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
            return ZString.Concat(RollingFilePath, rollingFileName, RollingFileExtension);
        }

        public void FormatSuffix(LogInfo info, IBufferWriter<byte> writer)
        {
            var level    = info.LogLevel.ToString();
            var eventId  = info.EventId.ToString();
            var dateTime = info.Timestamp.ToLocalTime().DateTime;
            ZString.Utf8Format(writer, SuffixFormat, level, eventId, dateTime);
        }

        public void FormatPrefix(LogInfo info, IBufferWriter<byte> writer)
        {
            var category = info.CategoryName;
            ZString.Utf8Format(writer, PrefixFormat, category);
        }

        public void OnBeforeSerialize()  => Validate();
        public void OnAfterDeserialize() => Validate();

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(GlobalCategory))
                GlobalCategory = Default.GlobalCategory;

            if (string.IsNullOrWhiteSpace(PrefixFormat)
             || !PrefixFormat.Contains("{0}"))
                PrefixFormat = Default.PrefixFormat;

            if (string.IsNullOrWhiteSpace(SuffixFormat)
             || !SuffixFormat.Contains("{0}")
             || !SuffixFormat.Contains("{1}")
             || !SuffixFormat.Contains("{2}"))
                SuffixFormat = Default.SuffixFormat;

            if (string.IsNullOrWhiteSpace(RollingFileNameFormat)
             || !RollingFileNameFormat.Contains("{0}")
             || !RollingFileNameFormat.Contains("{1}")
             || !RollingFileNameFormat.Contains("{2}")
             || !RollingFileNameFormat.Contains("{3}"))
                RollingFileNameFormat = Default.RollingFileNameFormat;
        }
    }
}