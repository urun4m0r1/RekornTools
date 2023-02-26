#nullable enable

using System;
using System.Globalization;
using Cysharp.Text;
using UnityEngine;

namespace Urun4m0r1.RekornTools
{
    public static class ColorUtils
    {
        /// <summary>Parse color from hex code.</summary>
        /// <param name="hexCode">Hex code. (e.g. #FF0000, #FF0000FF, FF0000, FF0000FF, ...)</param>
        /// <param name="color">Parsed color.</param>
        public static bool TryParseColor(this string? hexCode, out Color color)
        {
            var result = TryParseColor32(hexCode, out var color32);
            color = color32;
            return result;
        }

        /// <summary>Parse color from hex code.</summary>
        /// <param name="hexCode">Hex code. (e.g. #FF0000, #FF0000FF, FF0000, FF0000FF, ...)</param>
        /// <param name="color32">Parsed color.</param>
        public static bool TryParseColor32(this string? hexCode, out Color32 color32)
        {
            if (string.IsNullOrWhiteSpace(hexCode!))
            {
                color32 = default;
                return false;
            }

            if (hexCode.StartsWith('#'))
                hexCode = hexCode[1..];

            if (hexCode.Length == 3)
                hexCode = ZString.Format("{0}{0}{1}{1}{2}{2}", hexCode[0], hexCode[1], hexCode[2]);

            if (hexCode.Length == 6)
                hexCode = ZString.Concat(hexCode, "FF");

            if (hexCode.Length != 8)
            {
                color32 = default;
                return false;
            }

            var r = ParseColorFromSource(0..2);
            var g = ParseColorFromSource(2..4);
            var b = ParseColorFromSource(4..6);
            var a = ParseColorFromSource(6..8);

            byte ParseColorFromSource(Range range)
            {
                return byte.Parse(hexCode[range], NumberStyles.HexNumber);
            }

            color32 = new Color32(r, g, b, a);
            return true;
        }

        /// <inheritdoc cref="ColorUtils.ToRgbHexCode(Color32)"/>
        public static string ToRgbHexCode(this Color color)
        {
            return ((Color32)color).ToRgbHexCode();
        }

        /// <inheritdoc cref="ColorUtils.ToRgbaHexCode(Color32)"/>
        public static string ToRgbaHexCode(this Color color)
        {
            return ((Color32)color).ToRgbaHexCode();
        }

        /// <summary>Convert input color to RGB hex code.</summary>
        /// <returns>RGB hex code string. (e.g. #FF0000)</returns>
        public static string ToRgbHexCode(this Color32 color32)
        {
            return ZString.Format("#{0:X2}{1:X2}{2:X2}", color32.r, color32.g, color32.b);
        }

        /// <summary>Convert input color to RGBA hex code.</summary>
        /// <returns>RGBA hex code string. (e.g. #FF0000FF)</returns>
        public static string ToRgbaHexCode(this Color32 color32)
        {
            return ZString.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color32.r, color32.g, color32.b, color32.a);
        }
    }
}
