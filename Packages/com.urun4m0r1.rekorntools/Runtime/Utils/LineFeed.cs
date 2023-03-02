#nullable enable

using System;

namespace Urun4m0r1.RekornTools.Utils
{
    public enum LineFeed
    {
        System,
        Unix,
        Windows,
        Mac,
    }

    public static class LineFeedString
    {
        public static readonly string System  = Environment.NewLine;
        public static readonly string Unix    = "\n";
        public static readonly string Windows = "\r\n";
        public static readonly string Mac     = "\r";
    }

    public static class LineFeedExtensions
    {
        public static string GetLineFeedString(this LineFeed lineFeed) => lineFeed switch
        {
            LineFeed.System  => LineFeedString.System,
            LineFeed.Unix    => LineFeedString.Unix,
            LineFeed.Windows => LineFeedString.Windows,
            LineFeed.Mac     => LineFeedString.Mac,
            _                => throw new ArgumentOutOfRangeException(nameof(lineFeed), "Unknown target line ending character(s)."),
        };
    }
}
