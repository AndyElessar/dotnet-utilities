namespace RazorClassLibrary.Draggable;

/// <summary>
/// The distance in CSS pixels that a <see cref="Draggable"/> has been moved from its layout position
/// (applied as <c>translate</c>).
/// </summary>
/// <param name="X">The horizontal offset; positive moves right.</param>
/// <param name="Y">The vertical offset; positive moves down.</param>
public readonly record struct DragOffset(double X, double Y)
{
    /// <summary>
    /// Gets the offset of an element that has not been moved.
    /// </summary>
    public static DragOffset Zero => default;
}