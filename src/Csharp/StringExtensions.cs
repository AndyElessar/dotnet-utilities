namespace Utilities;

public static partial class UtilitiesExtensions
{
    /// <param name="source">The source string.</param>
    extension(string? source)
    {
        /// <summary>
        /// Newline separators (\r\n, \n).
        /// </summary>
        public static string[] NewLineSeparators => ["\r\n", "\n"];

        /// <summary>
        /// Removes all newline characters (string.NewLineSeparators).
        /// </summary>
        /// <returns>The string with all newlines removed.</returns>
        public string RemoveNewLine()
        {
            if(string.IsNullOrWhiteSpace(source))
                return string.Empty;

            var seps = string.NewLineSeparators;
            for(int i = 0; i < seps.Length; i++)
            {
                source = source.Replace(seps[i], string.Empty);
            }
            return source;
        }

        /// <summary>
        /// Removes all space characters.
        /// </summary>
        /// <returns>The string with all spaces removed.</returns>
        public string RemoveSpace()
        {
            if(string.IsNullOrWhiteSpace(source))
                return string.Empty;

            return source.Replace(" ", string.Empty);
        }

        /// <summary>
        /// Counts the number of occurrences of <paramref name="val"/> in <paramref name="source"/>.
        /// </summary>
        /// <param name="val">The string to search for.</param>
        /// <returns>The number of occurrences.</returns>
        public int CountString(string val)
        {
            if(string.IsNullOrEmpty(source) || string.IsNullOrEmpty(val))
                return 0;

            int count = 0;
            int index = 0;
            while((index = source.IndexOf(val, index, StringComparison.Ordinal)) != -1)
            {
                count++;
                index += val.Length;
            }
            return count;
        }

        /// <summary>
        /// Counts the total number of occurrences of strings in <paramref name="vals"/> within <paramref name="source"/>.
        /// </summary>
        /// <param name="vals">The list of strings to search for.</param>
        /// <returns>The total number of occurrences.</returns>
        public int CountString(IReadOnlyList<string> vals)
        {
            if(string.IsNullOrEmpty(source))
                return 0;

            int count = 0;
            for(int i = 0; i < vals.Count; i++)
            {
                string val = vals[i];
                if(string.IsNullOrEmpty(val))
                    continue;

                int index = 0;
                while((index = source.IndexOf(val, index, StringComparison.Ordinal)) != -1)
                {
                    count++;
                    index += val.Length;
                }
            }
            return count;
        }

        /// <summary>
        /// Counts the number of newline characters (string.NewLineSeparators).
        /// </summary>
        /// <returns>The number of newline occurrences.</returns>
        public int CountNewLine() => source.CountString(string.NewLineSeparators);

        /// <summary>
        /// Counts the number of occurrences of <paramref name="val"/> in <paramref name="source"/>.
        /// </summary>
        /// <param name="val">The character to search for.</param>
        /// <returns>The number of occurrences.</returns>
        public int CountChar(char val)
        {
            if(string.IsNullOrEmpty(source))
                return 0;

            int count = 0;
            int index = 0;
            while((index = source.IndexOf(val, index)) != -1)
            {
                count++;
                index += 1;
            }
            return count;
        }

        /// <summary>
        /// Counts the total number of occurrences of characters in <paramref name="vals"/> within <paramref name="source"/>.
        /// </summary>
        /// <param name="vals">The list of characters to search for.</param>
        /// <returns>The total number of occurrences.</returns>
        public int CountChar(IReadOnlyList<char> vals)
        {
            if(string.IsNullOrEmpty(source))
                return 0;

            int count = 0;
            for(int i = 0; i < vals.Count; i++)
            {
                char val = vals[i];

                int index = 0;
                while((index = source.IndexOf(val, index)) != -1)
                {
                    count++;
                    index += 1;
                }
            }
            return count;
        }

        /// <summary>
        /// Counts the number of space characters.
        /// </summary>
        /// <returns>The number of space occurrences.</returns>
        public int CountSpace() => source.CountChar(' ');

        /// <summary>
        /// Gets the last <paramref name="count"/> characters as a new string.
        /// </summary>
        /// <param name="count">The number of characters to take.</param>
        /// <returns>The last characters, or the entire string if shorter than count.</returns>
        public string TakeLast(int count)
        {
            if(source is null)
                return string.Empty;

            if(source.Length <= count)
                return source;
            else
                return source[^count..];
        }

        /// <summary>
        /// Returns <paramref name="alternative"/> if the string is <see langword="null"/> or empty.
        /// </summary>
        /// <param name="alternative">The alternative string to return.</param>
        /// <returns>The original string, or the alternative if null or empty.</returns>
        public string IsNullOrEmptyThen(string alternative) => string.IsNullOrEmpty(source) ? alternative : source;

        /// <summary>
        /// Returns <paramref name="alternative"/> if the string is <see langword="null"/>, empty, or consists only of whitespace.
        /// </summary>
        /// <param name="alternative">The alternative string to return.</param>
        /// <returns>The original string, or the alternative if null, empty, or whitespace.</returns>
        public string IsNullOrWhiteSpaceThen(string alternative) => string.IsNullOrWhiteSpace(source) ? alternative : source;

        /// <summary>
        /// Creates a string by repeating <paramref name="val"/> for <paramref name="count"/> times.
        /// </summary>
        /// <param name="count">The number of repetitions.</param>
        /// <param name="val">The string to repeat.</param>
        /// <returns>The repeated string.</returns>
        public static string Repeat(int count, string val) =>
            (count, val) switch
            {
                { count: <= 0 } => string.Empty,
                { count: 1 } => val,
                { count: > 0, val.Length: 0 } => string.Empty,
                var data => string.Create(count * val.Length, data, CreateString)
            };

        /// <summary>
        /// Creates a string by repeating the current string for <paramref name="count"/> times.
        /// </summary>
        /// <param name="count">The number of repetitions.</param>
        /// <returns>The repeated string.</returns>
        public string Repeat(int count) => source is null ? string.Empty : Repeat(count, source);

        public static string operator *(string val, int count) => Repeat(count, val);
    }

    extension(ReadOnlySpan<char> source)
    {
        /// <summary>
        /// Creates a string by repeating <paramref name="val"/> for <paramref name="count"/> times.
        /// </summary>
        /// <param name="count">The number of repetitions.</param>
        /// <param name="val">The span of characters to repeat.</param>
        /// <returns>The repeated character span.</returns>
        public static ReadOnlySpan<char> Repeat(int count, ReadOnlySpan<char> val) =>
            (count, val: val.ToString()) switch
            {
                { count: <= 0 } => string.Empty,
                { count: 1 } => val,
                { count: > 0, val.Length: 0 } => string.Empty,
                var data => string.Create(count * val.Length, data, CreateString)
            };

        /// <summary>
        /// Creates a string by repeating the current span for <paramref name="count"/> times.
        /// </summary>
        /// <param name="count">The number of repetitions.</param>
        /// <returns>The repeated character span.</returns>
        public ReadOnlySpan<char> Repeat(int count) => Repeat(count, source);

        public static ReadOnlySpan<char> operator *(ReadOnlySpan<char> val, int count) => Repeat(count, val);
    }

    private static void CreateString(Span<char> span, (int count, string val) state)
    {
        var valSpan = state.val.AsSpan();
        for(var i = 0; i < state.count; i++)
        {
            valSpan.CopyTo(span[(i * state.val.Length)..]);
        }
    }

    extension(char source)
    {
        /// <summary>
        /// Creates a string by repeating <paramref name="val"/> for <paramref name="count"/> times.
        /// </summary>
        /// <param name="count">The number of repetitions.</param>
        /// <param name="val">The character to repeat.</param>
        /// <returns>The repeated string.</returns>
        public static string Repeat(int count, char val) =>
            count switch
            {
                <= 0 => string.Empty,
                _ => new string(val, count)
            };

        /// <summary>
        /// Creates a string by repeating the current character for <paramref name="count"/> times.
        /// </summary>
        /// <param name="count">The number of repetitions.</param>
        /// <returns>The repeated string.</returns>
        public string Repeat(int count) => Repeat(count, source);

        public static string operator *(char val, int count) => Repeat(count, val);
    }

    extension(IEnumerable<string?> source)
    {
        /// <summary>
        /// Joins the string collection into a single string using <paramref name="separator"/> as the delimiter.
        /// </summary>
        /// <param name="separator">The separator character.</param>
        /// <returns>The joined string.</returns>
        public string Join(char separator) => string.Join(separator, source);
        /// <summary>
        /// Joins the string collection into a single string using <paramref name="separator"/> as the delimiter.
        /// </summary>
        /// <param name="separator">The separator string.</param>
        /// <returns>The joined string.</returns>
        public string Join(string? separator) => string.Join(separator, source);
    }

    extension(ReadOnlySpan<string?> source)
    {
        /// <summary>
        /// Joins the string span into a single string using <paramref name="separator"/> as the delimiter.
        /// </summary>
        /// <param name="separator">The separator character.</param>
        /// <returns>The joined string.</returns>
        public string Join(char separator) => string.Join(separator, source);
        /// <summary>
        /// Joins the string span into a single string using <paramref name="separator"/> as the delimiter.
        /// </summary>
        /// <param name="separator">The separator string.</param>
        /// <returns>The joined string.</returns>
        public string Join(string? separator) => string.Join(separator, source);
    }

    extension(string?[] source)
    {
        /// <summary>
        /// Joins the string array into a single string using <paramref name="separator"/> as the delimiter.
        /// </summary>
        /// <param name="separator">The separator character.</param>
        /// <returns>The joined string.</returns>
        public string Join(char separator) => string.Join(separator, source);
        /// <summary>
        /// Joins the string array into a single string using <paramref name="separator"/> as the delimiter.
        /// </summary>
        /// <param name="separator">The separator string.</param>
        /// <returns>The joined string.</returns>
        public string Join(string? separator) => string.Join(separator, source);
    }
}
