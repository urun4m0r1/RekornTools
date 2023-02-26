#nullable enable

using System;
using UnityEngine;

namespace Urun4m0r1.RekornTools.Unity
{
    /// <summary>
    /// Type of Unity application data path.
    /// <seealso cref="UnityEngine.Application"/>
    /// </summary>
    public enum AppDataPath
    {
        /// <summary>
        /// Empty string.
        /// </summary>
        None,
        /// <summary>
        /// Path to the console log file, or an empty string if the current platform does not support log files.
        /// <br/><a href="https://docs.unity3d.com/ScriptReference/Application-consoleLogPath.html">Unity Docs</a>
        /// <seealso cref="UnityEngine.Application.consoleLogPath"/>
        /// <example>%UserProfile%/AppData/LocalLow/%Company%/%Product%/Player.log</example>
        /// <remarks>
        /// Note that the option returns the path to the log file, not the directory where the log file is located.
        /// </remarks>
        /// </summary>
        ConsoleLogPath,
        /// <summary>
        /// (Read Only) Path to the game data folder on the target device.
        /// <br/><a href="https://docs.unity3d.com/ScriptReference/Application-dataPath.html">Unity Docs</a>
        /// <seealso cref="UnityEngine.Application.dataPath"/>
        /// <example>./Assets</example>
        /// </summary>
        DataPath,
        /// <summary>
        /// (Read Only) Path to a persistent data directory.
        /// <br/><a href="https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html">Unity Docs</a>
        /// <seealso cref="UnityEngine.Application.persistentDataPath"/>
        /// <example>%UserProfile%/AppData/LocalLow/%Company%/%Product%</example>
        /// </summary>
        PersistentDataPath,
        /// <summary>
        /// (Read Only) Path to the StreamingAssets folder.
        /// <br/><a href="https://docs.unity3d.com/ScriptReference/Application-streamingAssetsPath.html">Unity Docs</a>
        /// <seealso cref="UnityEngine.Application.streamingAssetsPath"/>
        /// <example>./Assets/StreamingAssets</example>
        /// </summary>
        StreamingAssetsPath,
        /// <summary>
        /// (Read Only) Path to a temporary data / cache directory.
        /// <br/><a href="https://docs.unity3d.com/ScriptReference/Application-temporaryCachePath.html">Unity Docs</a>
        /// <seealso cref="UnityEngine.Application.temporaryCachePath"/>
        /// <example>%Temp%/%Company%/%Product%</example>
        /// </summary>
        TemporaryCachePath,
    }

    /// <summary>
    /// Helper class for getting the path to the Unity application data.
    /// <seealso cref="AppDataPath"/>
    /// <seealso cref="UnityEngine.Application"/>
    /// </summary>
    public static class AppDataPathHelper
    {
        /// <summary>
        /// Gets the path to the Unity application data.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException"/>
        public static string GetPath(this AppDataPath appDataPath) => appDataPath switch
        {
            AppDataPath.None                => string.Empty,
            AppDataPath.ConsoleLogPath      => Application.consoleLogPath,
            AppDataPath.DataPath            => Application.dataPath,
            AppDataPath.PersistentDataPath  => Application.persistentDataPath,
            AppDataPath.StreamingAssetsPath => Application.streamingAssetsPath,
            AppDataPath.TemporaryCachePath  => Application.temporaryCachePath,
            _                               => throw new ArgumentOutOfRangeException(nameof(appDataPath), appDataPath, null!),
        } ?? string.Empty;
    }
}
