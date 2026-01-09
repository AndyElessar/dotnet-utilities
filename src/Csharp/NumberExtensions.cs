namespace Utilities;

public static partial class UtilitiesExtensions
{
    extension(scoped in decimal? val)
    {
        /// <summary>
        /// Formats the value using the specified format string, or returns <see langword="null"/> if the value is <see langword="null"/>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <returns>The formatted string, or <see langword="null"/>.</returns>
        public string? ToString(string format) =>
            val.HasValue ? val.Value.ToString(format) : null;

        /// <summary>
        /// Gets a value indicating whether the value is an integer (has no fractional part).
        /// </summary>
        public bool IsInteger => val.HasValue && val.Value % 1 == 0;
    }

    extension(scoped in decimal val)
    {
        /// <summary>
        /// Gets a value indicating whether the value is an integer (has no fractional part).
        /// </summary>
        public bool IsInteger => val % 1 == 0;
    }

    extension(scoped in double? val)
    {
        /// <summary>
        /// Formats the value using the specified format string, or returns <see langword="null"/> if the value is <see langword="null"/>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <returns>The formatted string, or <see langword="null"/>.</returns>
        public string? ToString(string format) =>
            val.HasValue ? val.Value.ToString(format) : null;

        /// <summary>
        /// Gets a value indicating whether the value is an integer (has no fractional part).
        /// </summary>
        public bool IsInteger => val % 1 == 0;
    }

    extension(scoped in int? val)
    {
        /// <summary>
        /// Formats the value using the specified format string, or returns <see langword="null"/> if the value is <see langword="null"/>.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <returns>The formatted string, or <see langword="null"/>.</returns>
        public string? ToString(string format) =>
            val.HasValue ? val.Value.ToString(format) : null;
    }

    extension(scoped in int val)
    {
        /// <summary>
        /// Determines whether the value is a power of two.
        /// </summary>
        /// <returns><see langword="true"/> if the value is a positive power of two; otherwise, <see langword="false"/>.</returns>
        public bool IsPowerOfTwo() => val > 0 && (val & (val - 1)) == 0;
    }
}
