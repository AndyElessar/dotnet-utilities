namespace Utilities;

public static partial class UtilitiesExtensions
{
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The enum value.</param>
    extension<T>(T value) where T : struct, Enum
    {
        /// <summary>
        /// Gets all individual flags (powers of two: 1, 2, 4, ...) defined in the enum.
        /// </summary>
        /// <returns>An enumerable of individual flag values.</returns>
        public static IEnumerable<T> GetAllFlags() =>
            Enum.GetValues<T>().Where(v => IsPowerOfTwo(Convert.ToInt32(v)));

        /// <summary>
        /// Gets all individual flags that are set in <paramref name="value"/>.
        /// </summary>
        /// <returns>An enumerable of individual flag values contained in the value.</returns>
        public IEnumerable<T> GetFlags() =>
            GetAllFlags<T>().Where(f => value.HasFlag(f));

        /// <summary>
        /// Counts the number of individual flags set in <paramref name="value"/>.
        /// </summary>
        /// <returns>The count of individual flags.</returns>
        public int CountFlags() =>
            GetAllFlags<T>().Count(f => value.HasFlag(f));
    }
}
