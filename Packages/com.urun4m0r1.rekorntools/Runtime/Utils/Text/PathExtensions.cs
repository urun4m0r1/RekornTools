#nullable enable

namespace Urun4m0r1.RekornTools.Utils
{
    public static class PathExtensions
    {
        private static class DirectorySeparatorCache
        {
            public static readonly string PrimaryString      = DirectorySeparator.Primary.ToString();
            public static readonly string RedundantSeparator = string.Concat(PrimaryString, PrimaryString);
        }

        /// <summary>
        /// Prepend directory separator char to the start of the path.<br/>
        /// Empty string will be returned if the path is null or whitespace.
        /// </summary>
        public static string PrependDirectorySeparator(this string? path)
        {
            return path.IsNullOrWhiteSpace()
                ? string.Empty
                : string.Concat(DirectorySeparatorCache.PrimaryString, path);
        }

        /// <summary>
        /// Append directory separator char to the end of the path.<br/>
        /// Empty string will be returned if the path is null or whitespace.
        /// </summary>
        public static string AppendDirectorySeparator(this string? path)
        {
            return path.IsNullOrWhiteSpace()
                ? string.Empty
                : string.Concat(path, DirectorySeparatorCache.PrimaryString);
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
            path = path.Replace(DirectorySeparator.Alt, DirectorySeparator.Primary);

            // 3. remove all redundant directory separator chars
            while (path.Contains(DirectorySeparatorCache.RedundantSeparator))
                path = path.Replace(DirectorySeparatorCache.RedundantSeparator, DirectorySeparatorCache.PrimaryString);

            // 4. remove leading and trailing directory separator chars
            path = path.TrimStart(DirectorySeparator.Primary) ?? string.Empty;
            path = path.TrimEnd(DirectorySeparator.Primary)   ?? string.Empty;

            return path;
        }
    }
}
