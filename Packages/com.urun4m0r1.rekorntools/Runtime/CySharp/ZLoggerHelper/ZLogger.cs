#nullable enable

using System;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    public sealed class ZLogger : IDisposable
    {
        public ILogger Global { get; }

        public bool IsDisposed { get; private set; }

        public ILogger<T> Create<T>()                 => _loggerFactory.CreateLogger<T>();
        public ILogger    Create(string categoryName) => _loggerFactory.CreateLogger(categoryName);


        private readonly ZLoggerHelperPreset _preset;
        private readonly ILoggerFactory      _loggerFactory;


        public ZLogger()
        {
            Info("ZLogger initializing...");
            {
                _preset = ZLoggerHelperSettings.GetPreset();

                // Standard LoggerFactory does not work on IL2CPP,
                // But you can use ZLogger's UnityLoggerFactory instead,
                // it works on IL2CPP, all platforms(includes mobile).
                _loggerFactory = UnityLoggerFactory.Create(ConfigureLogger)!;

                Global = _loggerFactory.CreateLogger(_preset.GlobalCategory);
            }
            Info("ZLogger initialized.");


            void ConfigureLogger(ILoggingBuilder builder)
            {
                builder.SetMinimumLevel(_preset.MinimumLevel);

                if (_preset.UseCategoryLevelFilter)
                    builder.AddFilter(_preset.GetCategoryLevelFilter());

                if (_preset.UseUnityLogging)
                    builder.AddZLoggerUnityDebug(ConfigureUnityLog);

                if (_preset.UseFileLogging)
                    builder.AddZLoggerFile(_preset.FileUrl, ConfigureFileLog);

                if (_preset.UseRollingFileLogging)
                    builder.AddZLoggerRollingFile(
                        fileNameSelector: (dt, i) => _preset.GetRollingFileUrl(dt, i),
                        timestampPattern: static t => t.ToLocalTime().Date,
                        rollSizeKB: _preset.RollingFileSizeKB,
                        configure: ConfigureFileLog);

                void ConfigureUnityLog(ZLoggerOptions options)
                {
                    options.PrefixFormatter = (writer, info) => _preset.FormatUnityPrefix(info, writer);
                    options.SuffixFormatter = (writer, info) => _preset.FormatUnitySuffix(info, writer);
                }

                void ConfigureFileLog(ZLoggerOptions options)
                {
                    options.PrefixFormatter = (writer, info) => _preset.FormatFilePrefix(info, writer);
                    options.SuffixFormatter = (writer, info) => _preset.FormatFileSuffix(info, writer);
                }
            }
        }

        public void Dispose()
        {
            DisposeLogger();
            GC.SuppressFinalize(this);
        }

        ~ZLogger()
        {
            DisposeLogger();
        }

        private void DisposeLogger()
        {
            Info("ZLogger disposing...");
            {
                if (IsDisposed)
                {
                    Info("ZLogger already disposed.");
                    return;
                }

                _loggerFactory.Dispose();
                IsDisposed = true;
            }
            Info("ZLogger disposed.");
        }

        private static void Info(string message)
        {
            Debug.Log($"<color=cyan><b>[{nameof(ZLogger)}]</b></color> {message}");
        }
    }
}
