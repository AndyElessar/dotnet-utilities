namespace RazorClassLibrary.Draggable;

/// <summary>
/// The area that a <see cref="Draggable"/> is kept inside of while it is dragged and when that area is resized.
/// </summary>
public enum DraggableBounds
{
    /// <summary>
    /// No bounds: the element can be dragged anywhere.
    /// </summary>
    None,

    /// <summary>
    /// The visible area (padding box) of the enclosing <see cref="DraggableContainer"/>; unbounded without one.
    /// </summary>
    Container,

    /// <summary>
    /// The visible area (padding box) of the parent element.
    /// </summary>
    Parent,

    /// <summary>
    /// The viewport, excluding scrollbars.
    /// </summary>
    Viewport,
}