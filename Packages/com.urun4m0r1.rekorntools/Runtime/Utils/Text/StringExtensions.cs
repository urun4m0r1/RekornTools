#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? value)
        {
            return string.IsNullOrWhiteSpace(value!);
        }

        public static bool IsNullOrEmpty([NotNullWhen(false)] this string? value)
        {
            return string.IsNullOrEmpty(value!);
        }

        public static string ThrowIfWhiteSpace(this string? value)
        {
            if (value.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(value));

            return value;
        }

        public static string ThrowIfEmpty(this string? value)
        {
            if (value.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(value));

            return value;
        }

        public static string? NormalizeLineFeed(this string? lines, LineFeed replacement = LineFeed.System)
        {
            if (lines.IsNullOrEmpty())
                return lines;

            // Normalize line endings to Unix.
            // If you directly replace line endings here, you will get unexpected results like "\r\r\n" or "\r\n\r\n".
            // So, first normalize to Unix, and then replace it to the target line ending.
            lines = lines.Replace(LineFeedString.Windows, LineFeedString.Unix)
                         .Replace(LineFeedString.Mac, LineFeedString.Unix);

            // If the target line ending is Unix, return it.
            var replacementString = replacement.GetLineFeedString();
            if (replacementString == LineFeedString.Unix)
                return lines;

            // Replace Unix line endings to the target line ending.
            lines = lines.Replace(LineFeedString.Unix, replacementString);
            return lines;
        }

        public static IEnumerable<string?>? SplitByLineFeed(this string? lines, LineFeed lineFeed = LineFeed.System, StringSplitOptions options = StringSplitOptions.None)
        {
            var normalizedLines = lines.NormalizeLineFeed(lineFeed);
            return normalizedLines?.Split(lineFeed.GetLineFeedString(), options);
        }
    }
}
