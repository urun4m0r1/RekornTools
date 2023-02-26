#nullable enable

using System.Diagnostics;
using UnityDebug = UnityEngine.Debug;

namespace Urun4m0r1.RekornTools
{
    internal static class Debug
    {
        private static readonly string Format  = "<b>[{0}]</b> {1}";
        private static readonly string Format2 = "<b>[{0}]</b> ({1}) {2}";

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void Log(object header, object owner, object message)
        {
            Log(string.Format(Format2, header, owner, message));
        }

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogWarning(object header, object owner, object message)
        {
            LogWarning(string.Format(Format2, header, owner, message));
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogError(object header, object owner, object message)
        {
            LogError(string.Format(Format2, header, owner, message));
        }

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void Log(object header, object message)
        {
            Log(string.Format(Format, header, message));
        }

        [Conditional("DEBUG_VERBOSE")]
        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogWarning(object header, object message)
        {
            LogWarning(string.Format(Format, header, message));
        }

        [DebuggerHidden]
        [DebuggerStepThrough]
        public static void LogError(object header, object message)
        {
            LogError(string.Format(Format, header, message));
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
