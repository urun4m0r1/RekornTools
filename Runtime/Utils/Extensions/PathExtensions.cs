#nullable enable

using System.IO;

namespace Rekorn.Tools.Utils
{
    public static class PathExtensions
    {
        /// <summary>
        /// Append directory separator char to the end of the path.<br/>
        /// Empty string will be returned if the path is null or whitespace.
        /// </summary>
        public static string AppendDirectorySeparator(this string? path)
        {
            return string.IsNullOrWhiteSpace(path!)
                ? string.Empty
                : string.Concat(path, Path.DirectorySeparatorChar.ToString());
        }

        public static string NormalizePath(this string? path)
        {
            // 1. return empty string if path is null or whitespace
            if (string.IsNullOrWhiteSpace(path!))
                return string.Empty;

            // 2. replace all directory separator chars to the current platform directory separator char
            var altSeparator = Path.AltDirectorySeparatorChar;
            var separator    = Path.DirectorySeparatorChar;
            path = path.Replace(altSeparator, separator);

            // 3. remove all redundant directory separator chars
            var separatorString    = separator.ToString();
            var redundantSeparator = string.Concat(separatorString, separatorString);

            while (path.Contains(redundantSeparator))
                path = path.Replace(redundantSeparator, separatorString);

            // 4. remove leading and trailing directory separator chars
            path = path.TrimStart(separator) ?? string.Empty;
            path = path.TrimEnd(separator) ?? string.Empty;

            return path;
        }
    }
}
