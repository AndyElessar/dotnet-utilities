namespace Utilities;

public static partial class UtilitiesExtensions
{
    extension<T>(IReadOnlyCollection<T> source)
    {
        /// <summary>
        /// Determines whether the collection contains any elements.
        /// </summary>
        /// <returns><see langword="true"/> if the collection contains at least one element; otherwise, <see langword="false"/>.</returns>
        public bool Any() => source.Count > 0;

        /// <summary>
        /// Gets a value indicating whether the collection is empty.
        /// </summary>
        public bool IsEmpty => source.Count == 0;
    }

    extension<T>(IEnumerable<T> source)
    {
        /// <summary>
        /// Gets a value indicating whether the sequence is empty.
        /// </summary>
        public bool IsEmpty => !source.Any();

        /// <summary>
        /// Determines whether all keys selected by <paramref name="keySelector"/> are unique.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function to extract the key from each element.</param>
        /// <returns><see langword="true"/> if all keys are unique; otherwise, <see langword="false"/>.</returns>
        public bool IsUnique<TKey>(Func<T, TKey> keySelector) =>
            source.IsEmpty || source.GroupBy(keySelector).All(g => g.Count() == 1);

        /// <summary>
        /// Determines whether any keys selected by <paramref name="keySelector"/> are duplicated.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="keySelector">A function to extract the key from each element.</param>
        /// <returns><see langword="true"/> if any keys are duplicated; otherwise, <see langword="false"/>.</returns>
        public bool IsNotUnique<TKey>(Func<T, TKey> keySelector) =>
            source.Any() && source.GroupBy(keySelector).Any(g => g.Count() > 1);
    }
}
