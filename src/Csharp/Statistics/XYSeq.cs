using System.Numerics;

namespace Utilities.Statistics;

/// <summary>
/// Represents a pair of X and Y data sequences for statistical calculations.<br/>
/// <see cref="X"/> and <see cref="Y"/> are guaranteed to have the same length.
/// </summary>
/// <typeparam name="T">The numeric type of the data elements.</typeparam>
public readonly ref struct XYSeq<T> where T : struct, INumber<T>
{
    /// <summary>
    /// The X data sequence.
    /// </summary>
    public readonly ReadOnlySpan<T> X;

    /// <summary>
    /// The Y data sequence.
    /// </summary>
    public readonly ReadOnlySpan<T> Y;

    /// <summary>
    /// Gets the length of the data sequences.
    /// </summary>
    public int Length => X.Length;

    /// <summary>
    /// Initializes a new instance of the <see cref="XYSeq{T}"/> struct.
    /// </summary>
    /// <param name="x">The X data sequence.</param>
    /// <param name="y">The Y data sequence.</param>
    /// <exception cref="ArgumentException">Thrown when the lengths of x and y do not match.</exception>
    public XYSeq(ReadOnlySpan<T> x, ReadOnlySpan<T> y)
    {
        if (x.Length != y.Length)
            throw new ArgumentException("The lengths of x and y do not match!");

        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets the X and Y values at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>A tuple containing the X and Y values at the specified index.</returns>
    public (T X, T Y) this[int index] => (X[index], Y[index]);

    /// <summary>
    /// Returns an enumerator that iterates through the XY data pairs.
    /// </summary>
    /// <returns>An enumerator for the XY data pairs.</returns>
    public Enumerator GetEnumerator() => new(this);

    /// <summary>
    /// Enumerator for iterating through XY data pairs.
    /// </summary>
    public ref struct Enumerator
    {
        private readonly XYSeq<T> _data;
        private int _index;

        internal Enumerator(XYSeq<T> data)
        {
            _data = data;
            _index = -1;
        }

        /// <summary>
        /// Gets the current XY data pair.
        /// </summary>
        public readonly (T X, T Y) Current => (_data.X[_index], _data.Y[_index]);

        /// <summary>
        /// Advances the enumerator to the next element.
        /// </summary>
        /// <returns>True if the enumerator was successfully advanced; false if the enumerator has passed the end of the collection.</returns>
        public bool MoveNext()
        {
            int index = _index + 1;
            if (index < _data.Length)
            {
                _index = index;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Resets the enumerator to its initial position.
        /// </summary>
        public void Reset() => _index = -1;
    }
}
