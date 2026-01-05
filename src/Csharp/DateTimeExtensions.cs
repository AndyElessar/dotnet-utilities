namespace Utilities;

public static partial class UtilitiesExtensions
{
    /// <param name="val">A nullable DateTime value.</param>
    extension(scoped in DateTime? val)
    {
        /// <summary>
        /// Returns an empty string if <see langword="null"/>, otherwise calls <see cref="DateTime.ToShortDateString"/>.
        /// </summary>
        /// <returns>The short date string representation, or an empty string if <see langword="null"/>.</returns>
        public string ToShortDateString() =>
            val.HasValue ? val.Value.ToShortDateString() : string.Empty;

        /// <summary>
        /// Returns an empty string if <see langword="null"/>, otherwise calls <see cref="DateTime.ToString(string)"/>.
        /// </summary>
        /// <param name="format">The format string. Defaults to "yyyy/MM/dd HH:mm:ss".</param>
        /// <returns>The formatted date string, or an empty string if <see langword="null"/>.</returns>
        public string ToString(string format = "yyyy/MM/dd HH:mm:ss") =>
            val.HasValue ? val.Value.ToString(format) : string.Empty;

        /// <summary>
        /// Converts to <see cref="DateOnly"/>, or returns <see langword="null"/> if the value is <see langword="null"/>.
        /// </summary>
        /// <returns>The <see cref="DateOnly"/> representation, or <see langword="null"/>.</returns>
        public DateOnly? ToDateOnly() =>
            val.HasValue ? DateOnly.FromDateTime(val.Value) : null;

        /// <summary>
        /// Converts to <see cref="TimeOnly"/>, or returns <see langword="null"/> if the value is <see langword="null"/>.
        /// </summary>
        /// <returns>The <see cref="TimeOnly"/> representation, or <see langword="null"/>.</returns>
        public TimeOnly? ToTimeOnly() =>
            val.HasValue ? TimeOnly.FromDateTime(val.Value) : null;
    }

    extension(scoped in DateOnly? val)
    {
        /// <summary>
        /// Returns an empty string if <see langword="null"/>, otherwise calls <see cref="DateOnly.ToString(string)"/>.
        /// </summary>
        /// <param name="format">The format string. Defaults to "yyyy/MM/dd".</param>
        /// <returns>The formatted date string, or an empty string if <see langword="null"/>.</returns>
        public string ToString(string format = "yyyy/MM/dd") =>
            val.HasValue ? val.Value.ToString(format) : string.Empty;
    }

    extension(scoped in TimeOnly? val)
    {
        /// <summary>
        /// Returns an empty string if <see langword="null"/>, otherwise calls <see cref="TimeOnly.ToString(string)"/>.
        /// </summary>
        /// <param name="format">The format string. Defaults to "HH:mm".</param>
        /// <returns>The formatted time string, or an empty string if <see langword="null"/>.</returns>
        public string ToString(string format = "HH:mm") =>
            val.HasValue ? val.Value.ToString(format) : string.Empty;
    }
}
