#nullable enable

using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using UnityEngine;
using Urun4m0r1.RekornTools.Unity;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    public static partial class LogManager
    {
        public static ILogger    Global                      => GetLogger().Global;
        public static ILogger<T> Create<T>() where T : class => GetLogger().Create<T>();
        public static ILogger    Create(string categoryName) => GetLogger().Create(categoryName);
        public static bool       IsDisposed                  => GetLogger().IsDisposed;

        private static ZLogger? s_logger;

        private static ZLogger GetLogger()
        {
#if UNITY_EDITOR
            return GetLoggerEditor();
#else
            return GetLoggerStandalone();
#endif
        }

        [UsedImplicitly]
        private static ZLogger GetLoggerStandalone()
        {
            if (s_logger is not null)
                return s_logger;

            Info(nameof(GetLoggerStandalone));
            s_logger = new ZLogger();
            RegisterEvent();
            return s_logger;

            static void RegisterEvent()
            {
                UnityEventManager.ApplicationQuit += OnApplicationQuit;
            }

            static void UnregisterEvent()
            {
                UnityEventManager.ApplicationQuit -= OnApplicationQuit;
            }

            static void OnApplicationQuit() => Dispose(nameof(OnApplicationQuit));

            static void Dispose(string message)
            {
                UnregisterEvent();

                if (s_logger is null)
                    return;

                Info(message);
                s_logger?.Dispose();
                s_logger = null;
            }
        }

        private static void Info(string message)
        {
            Debug.Log($"<color=cyan><b>[{nameof(LogManager)}]</b></color> {message}");
        }
    }
}
