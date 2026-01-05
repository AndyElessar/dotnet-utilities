namespace Utilities;

public static partial class UtilitiesExtensions
{
    extension<T>(T) where T : new()
    {
        /// <summary>
        /// Generates <paramref name="count"/> new instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="count">The number of instances to generate.</param>
        /// <returns>An enumerable of newly created instances.</returns>
        public static IEnumerable<T> Generate(int count)
        {
            for(int i = 0; i < count; i++)
            {
                yield return new T();
            }
        }
    }

    extension<T>(T)
    {
        /// <summary>
        /// Generates <paramref name="count"/> instances of <typeparamref name="T"/> using the specified <paramref name="factory"/>.
        /// </summary>
        /// <param name="count">The number of instances to generate.</param>
        /// <param name="factory">The factory method to create each instance.</param>
        /// <returns>An enumerable of newly created instances.</returns>
        public static IEnumerable<T> Generate(int count, Func<T> factory)
        {
            for(int i = 0; i < count; i++)
            {
                yield return factory();
            }
        }

        /// <summary>
        /// Generates <paramref name="count"/> instances of <typeparamref name="T"/> using the specified <paramref name="factory"/>.
        /// </summary>
        /// <param name="count">The number of instances to generate.</param>
        /// <param name="factory">The factory method to create each instance, with the zero-based index as parameter.</param>
        /// <returns>An enumerable of newly created instances.</returns>
        public static IEnumerable<T> Generate(int count, Func<int, T> factory)
        {
            for(int i = 0; i < count; i++)
            {
                yield return factory(i);
            }
        }
    }
}
