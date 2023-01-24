#nullable enable

using System;
using UnityEngine;

namespace Rekorn.Tools.ZLoggerHelper
{
    [Serializable]
    public struct LogStyle
    {
        [SerializeField] private bool  _isBold;
        [SerializeField] private Color _color;

        public bool IsBold
        {
            get => _isBold;
            set => _isBold = value;
        }

        public Color Color
        {
            get => _color;
            set => _color = value;
        }

        public LogStyle(bool isBold = default, Color color = default) : this()
        {
            IsBold = isBold;
            Color  = color;
        }
    }
}
