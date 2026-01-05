namespace Utilities;

public static partial class UtilitiesExtensions
{
    extension(scoped in bool? val)
    {
        /// <summary>
        /// Converts the <paramref name="val"/> to a specified string representation. Returns <see langword="null"/> if <paramref name="val"/> is <see langword="null"/>.
        /// </summary>
        /// <param name="trueString">The string to return when <paramref name="val"/> is <see langword="true"/>.</param>
        /// <param name="falseString">The string to return when <paramref name="val"/> is <see langword="false"/>.</param>
        /// <returns>The corresponding string representation, or <see langword="null"/> if <paramref name="val"/> is <see langword="null"/>.</returns>
        public string? ToString(string? trueString, string? falseString) =>
            val switch
            {
                true => trueString,
                false => falseString,
                null => null
            };
    }
}
