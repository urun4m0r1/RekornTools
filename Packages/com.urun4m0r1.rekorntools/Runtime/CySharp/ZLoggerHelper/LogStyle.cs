#nullable enable

using System;
using UnityEngine;

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    [Serializable]
    public struct LogStyle
    {
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


        [SerializeField] private bool  _isBold;
        [SerializeField] private Color _color;


        public LogStyle(bool isBold = default, Color color = default) : this()
        {
            IsBold = isBold;
            Color  = color;
        }
    }
}
