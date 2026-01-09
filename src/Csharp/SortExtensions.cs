namespace Utilities;

/// <summary>
/// Represents an object that can be ordered within a collection.
/// </summary>
/// <remarks>Implement this interface to enable custom sorting of objects based on their sort order. The value of
/// <see cref="SortOrder"/> determines the relative position of the object when sorted.</remarks>
public interface ISortable
{
    /// <summary>
    /// Gets or sets the sort order of the object.
    /// </summary>
    public int SortOrder { get; set; }
}

public static partial class UtilitiesExtensions
{
    /// <typeparam name="T">A <see langword="class"/> that implements <see cref="ISortable"/>.</typeparam>
    extension<T>(IEnumerable<T> source) where T : class, ISortable
    {
        /// <summary>
        /// Sorts <paramref name="source"/> by <see cref="ISortable.SortOrder"/> and updates the <see cref="ISortable.SortOrder"/> values sequentially.
        /// </summary>
        public void Resort()
        {
            var ordered = source.OrderBy(x => x.SortOrder).ToArray();
            for(var i = 0; i < ordered.Length; i++)
            {
                ordered[i].SortOrder = i + 1;
            }
        }

        /// <summary>
        /// Gets the next available <see cref="ISortable.SortOrder"/> value from <paramref name="source"/>.
        /// </summary>
        /// <returns>The next available sort order value.</returns>
        public int GetNextOrder() =>
            source.Any() ? source.Max(x => x.SortOrder) + 1 : 1;
    }

    /// <typeparam name="T">A <see langword="class"/> that implements <see cref="ISortable"/>.</typeparam>
    extension<T>(IReadOnlyCollection<T> source) where T : class, ISortable
    {
        /// <summary>
        /// Gets the next available <see cref="ISortable.SortOrder"/> value from <paramref name="source"/>.
        /// </summary>
        /// <returns>The next available sort order value.</returns>
        public int GetNextOrder() =>
            source.Any() ? source.Max(x => x.SortOrder) + 1 : 1;

        /// <summary>
        /// Moves the specified item up in the sort order by swapping with the previous item.
        /// </summary>
        /// <param name="dto">The item to move up.</param>
        public void MoveUp(T dto)
        {
            if(source.Count > 1 && !source.IsFirst(dto))
            {
                source.Resort();
                var order = dto.SortOrder;
                var prev = source.Where(r => r.SortOrder < order).MaxBy(static r => r.SortOrder)!;
                var prevOrder = prev.SortOrder;
                prev.SortOrder = order;
                dto.SortOrder = prevOrder;
            }
        }

        /// <summary>
        /// Moves the specified item down in the sort order by swapping with the next item.
        /// </summary>
        /// <param name="dto">The item to move down.</param>
        public void MoveDown(T dto)
        {
            if(source.Count > 1 && !source.IsLast(dto))
            {
                source.Resort();
                var order = dto.SortOrder;
                var next = source.Where(r => r.SortOrder > order).MinBy(static r => r.SortOrder)!;
                var nextOrder = next.SortOrder;
                next.SortOrder = order;
                dto.SortOrder = nextOrder;
            }
        }

        /// <summary>
        /// Determines whether the specified item is first in the sort order.
        /// </summary>
        /// <param name="dto">The item to check.</param>
        /// <returns><see langword="true"/> if the item has the lowest sort order; otherwise, <see langword="false"/>.</returns>
        public bool IsFirst(T dto) =>
            source.Min(static v => v.SortOrder) == dto.SortOrder;

        /// <summary>
        /// Determines whether the specified item is last in the sort order.
        /// </summary>
        /// <param name="dto">The item to check.</param>
        /// <returns><see langword="true"/> if the item has the highest sort order; otherwise, <see langword="false"/>.</returns>
        public bool IsLast(T dto) =>
            source.Max(static v => v.SortOrder) == dto.SortOrder;
    }
}
