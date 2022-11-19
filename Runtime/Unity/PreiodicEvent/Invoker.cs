#nullable enable

using System;

namespace Rekorn.Tools.Unity
{
    public enum ReinvokeAction
    {
        Ignore,
        Restart,
        Stop,
    }

    [Flags] public enum Invoker
    {
        None                = 0,
        Manually            = 1 << 0,
        Awake               = 1 << 1,
        Start               = 1 << 2,
        OnEnable            = 1 << 3,
        OnApplicationResume = 1 << 4,
        OnApplicationFocus  = 1 << 5,
    }

    [Flags] public enum Interrupter
    {
        None                   = 0,
        Manually               = 1 << 0,
        OnDestroy              = 1 << 1,
        OnDisable              = 1 << 2,
        OnApplicationQuit      = 1 << 3,
        OnApplicationPause     = 1 << 4,
        OnApplicationFocusLost = 1 << 5,
    }
}
