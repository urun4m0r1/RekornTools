#nullable enable
using System;
using UnityEngine;

namespace Rekorn.Tools.ZLoggerHelper
{
    [Serializable]
    public struct LogStyle
    {
        [field: SerializeField] public bool  IsBold { get; private set; }
        [field: SerializeField] public Color Color  { get; private set; }

        public LogStyle(bool isBold, Color color)
        {
            IsBold = isBold;
            Color  = color;
        }
    }
}