#nullable enable

using System.Diagnostics;
using UnityDebug = UnityEngine.Debug;

namespace Urun4m0r1.RekornTools.Tests
{
    internal static class Debug
    {
        private static readonly string s_format  = "<b>[{0}]</b> {1}";
        private static readonly string s_format2 = "<b>[{0}]</b> ({1}) {2}";

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void Log(object header, object owner, object message)
        {
            Log(string.Format(s_format2, header, owner, message));
        }

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogWarning(object header, object owner, object message)
        {
            LogWarning(string.Format(s_format2, header, owner, message));
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogError(object header, object owner, object message)
        {
            LogError(string.Format(s_format2, header, owner, message));
        }

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void Log(object header, object message)
        {
            Log(string.Format(s_format, header, message));
        }

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogWarning(object header, object message)
        {
            LogWarning(string.Format(s_format, header, message));
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogError(object header, object message)
        {
            LogError(string.Format(s_format, header, message));
        }

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void Log(object message)
        {
            UnityDebug.Log(message);
        }

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogWarning(object message)
        {
            UnityDebug.LogWarning(message);
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogError(object message)
        {
            UnityDebug.LogError(message);
        }
    }
}
