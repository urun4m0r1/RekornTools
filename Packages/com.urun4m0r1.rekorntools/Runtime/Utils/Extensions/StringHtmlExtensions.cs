#nullable enable

using UnityEngine;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class StringHtmlExtensions
    {
        public static string Bold(this string str) => $"<b>{str}</b>";

        public static string Italic(this string str) => $"<i>{str}</i>";

        public static string Size(this string str, int size) => $"<size={size.ToString()}>{str}</size>";

        public static string Color(this string str, Color color) => $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{str}</color>";

        public static string Color(this string str, Color32 color) => $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{str}</color>";
    }
}
