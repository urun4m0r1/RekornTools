#nullable enable

using UnityEngine;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class StringHtmlExtensions
    {
        public static string Bold(this string str)
        {
            return $"<b>{str}</b>";
        }

        public static string Italic(this string str)
        {
            return $"<i>{str}</i>";
        }

        public static string Size(this string str, int size)
        {
            return $"<size={size.ToString()}>{str}</size>";
        }

        public static string Color(this string str, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{str}</color>";
        }

        public static string Color(this string str, Color32 color)
        {
            return Color(str, (Color)color);
        }
    }
}
