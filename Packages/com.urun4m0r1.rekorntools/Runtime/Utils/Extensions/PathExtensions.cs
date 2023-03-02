#nullable enable

using System.IO;

namespace Urun4m0r1.RekornTools.Utils
{
    public static class PathExtensions
    {
        private static class Cache
        {
            public static readonly char AltDirectorySeparatorChar = Path.AltDirectorySeparatorChar;
            public static readonly char DirectorySeparatorChar    = Path.DirectorySeparatorChar;

            public static readonly string DirectorySeparatorString          = DirectorySeparatorChar.ToString();
            public static readonly string RedundantDirectorySeparatorString = string.Concat(DirectorySeparatorString, DirectorySeparatorString);
        }

        /// <summary>
        /// Prepend directory separator char to the start of the path.<br/>
        /// Empty string will be returned if the path is null or whitespace.
        /// </summary>
        public static string PrependDirectorySeparator(this string? path)
        {
            return path.IsNullOrWhiteSpace()
                ? string.Empty
                : string.Concat(Cache.DirectorySeparatorString, path);
        }

        /// <summary>
        /// Append directory separator char to the end of the path.<br/>
        /// Empty string will be returned if the path is null or whitespace.
        /// </summary>
        public static string AppendDirectorySeparator(this string? path)
        {
            return path.IsNullOrWhiteSpace()
                ? string.Empty
                : string.Concat(path, Cache.DirectorySeparatorString);
        }

        /// <summary>
        /// Replace all alt directory separators to default directory separators.<br/>
        /// And remove all leading/trailing/redundant directory separators from the path.<br/>
        /// An empty string will be returned if the path is null or whitespace.
        /// </summary>
        public static string NormalizePath(this string? path)
        {
            // 1. return empty string if path is null or whitespace
            if (path.IsNullOrWhiteSpace())
                return string.Empty;

            // 2. replace all directory separator chars to the current platform directory separator char
            path = path.Replace(Cache.AltDirectorySeparatorChar, Cache.DirectorySeparatorChar);

            // 3. remove all redundant directory separator chars
            while (path.Contains(Cache.RedundantDirectorySeparatorString))
                path = path.Replace(Cache.RedundantDirectorySeparatorString, Cache.DirectorySeparatorString);

            // 4. remove leading and trailing directory separator chars
            path = path.TrimStart(Cache.DirectorySeparatorChar) ?? string.Empty;
            path = path.TrimEnd(Cache.DirectorySeparatorChar)   ?? string.Empty;

            return path;
        }
    }
}
