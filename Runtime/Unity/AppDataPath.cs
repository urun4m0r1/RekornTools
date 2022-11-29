#nullable enable

using System;
using UnityEngine;

namespace Rekorn.Tools.Unity
{
    public enum AppDataPath
    {
        None,
        PersistentDataPath,
        StreamingAssetsPath,
        DataPath,
        TemporaryCachePath,
        ConsoleLogPath,
    }

    public static class AppDataPathHelper
    {
        public static string GetAppDataPath(this AppDataPath appDataPath) => appDataPath switch
        {
            AppDataPath.None                => string.Empty,
            AppDataPath.PersistentDataPath  => Application.persistentDataPath,
            AppDataPath.StreamingAssetsPath => Application.streamingAssetsPath,
            AppDataPath.DataPath            => Application.dataPath,
            AppDataPath.TemporaryCachePath  => Application.temporaryCachePath,
            AppDataPath.ConsoleLogPath      => Application.consoleLogPath,
            _                               => throw new ArgumentOutOfRangeException(nameof(appDataPath), appDataPath, null!),
        } ?? string.Empty;
    }
}
