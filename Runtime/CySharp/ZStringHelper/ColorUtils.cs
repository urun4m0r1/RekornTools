#nullable enable

using System;
using System.Globalization;
using Cysharp.Text;
using UnityEngine;

namespace Rekorn.Tools
{
    public static class ColorUtils
    {
        public static bool TryParseHtmlString(this string htmlString, out Color32 color32)
        {
            if (htmlString.StartsWith('#'))
            {
                htmlString = htmlString[1..];
            }

            if (htmlString.Length == 3)
            {
                htmlString = ZString.Format("{0}{0}{1}{1}{2}{2}", htmlString[0], htmlString[1], htmlString[2]);
            }

            if (htmlString.Length == 6)
            {
                htmlString = ZString.Concat(htmlString, "FF");
            }

            if (htmlString.Length == 8)
            {
                var r = ParseHexColor(0..2);
                var g = ParseHexColor(2..4);
                var b = ParseHexColor(4..6);
                var a = ParseHexColor(6..8);

                byte ParseHexColor(Range range)
                {
                    return byte.Parse(htmlString[range], NumberStyles.HexNumber);
                }

                color32 = new Color32(r, g, b, a);
                return true;
            }

            color32 = default;
            return false;
        }

        public static string ToHtmlStringRGB(this Color color)
        {
            return ((Color32)color).ToHtmlStringRGB();
        }

        public static string ToHtmlStringRGBA(this Color color)
        {
            return ((Color32)color).ToHtmlStringRGBA();
        }

        public static string ToHtmlStringRGB(this Color32 color32)
        {
            return ZString.Format("#{0:X2}{1:X2}{2:X2}", color32.r, color32.g, color32.b);
        }

        public static string ToHtmlStringRGBA(this Color32 color32)
        {
            return ZString.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color32.r, color32.g, color32.b, color32.a);
        }
    }
}
