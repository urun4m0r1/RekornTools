#nullable enable

using System;
using System.Collections.Generic;

namespace Rekorn.Tools.Utils
{
    public static class StringExtensions
    {
        public const string UnixLineFeed    = "\n";
        public const string WindowsLineFeed = "\r\n";
        public const string MacLineFeed     = "\r";

        public static string SystemLineFeed => Environment.NewLine;

        public static bool IsNullOrWhiteSpace(this string? source) => string.IsNullOrWhiteSpace(source!);

        public static bool IsNullOrEmpty(this string? source) => string.IsNullOrEmpty(source!);

        public static string ThrowIfWhiteSpace(this string? source)
        {
            if (string.IsNullOrWhiteSpace(source!)) throw new ArgumentNullException(nameof(source));
            return source;
        }

        public static string ThrowIfEmpty(this string? source)
        {
            if (string.IsNullOrEmpty(source!)) throw new ArgumentNullException(nameof(source));
            return source;
        }

        public static string? NormalizeLineFeed(this string? lines, string? lineFeed = null)
        {
            if (string.IsNullOrEmpty(lines!)) return lines;

            lineFeed ??= SystemLineFeed;

            if (lineFeed is not UnixLineFeed or WindowsLineFeed or MacLineFeed)
            {
                throw new ArgumentOutOfRangeException(nameof(lineFeed), "Unknown target line ending character(s).");
            }

            lines = lines.Replace(WindowsLineFeed, UnixLineFeed)
                         .Replace(MacLineFeed, UnixLineFeed);

            if (lineFeed != UnixLineFeed)
            {
                lines = lines.Replace(UnixLineFeed, lineFeed);
            }

            return lines;
        }

        public static IEnumerable<string?>? SplitByLineFeed(this string? lines, string? lineFeed = null, StringSplitOptions options = StringSplitOptions.None)
        {
            lineFeed ??= SystemLineFeed;
            var normalizeLines = lines.NormalizeLineFeed(lineFeed);
            return normalizeLines?.Split(lineFeed, options);
        }
    }
}
