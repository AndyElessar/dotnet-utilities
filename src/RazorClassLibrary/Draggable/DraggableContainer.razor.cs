using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using RazorClassLibrary;
using RazorClassLibrary.Anchor;

namespace RazorClassLibrary.Draggable;

/// <summary>
/// Wraps its content in an element that bounds every <see cref="Draggable"/> inside it whose
/// <see cref="Draggable.Bounds"/> is <see cref="DraggableBounds.Container"/> (the default).
/// </summary>
/// <remarks>
/// Draggables are kept inside the visible padding box of the container. Give it a size, e.g. a height, so there is
/// room to move; a draggable larger than the container stays aligned to its top-left corner.
/// </remarks>
public partial class DraggableContainer : ComponentBase
{
    /// <summary>
    /// Gets or sets the name of the container element. Defaults to <c>div</c>.
    /// </summary>
    [Parameter]
    public string Element { get; set; } = "div";

    /// <summary>
    /// Gets or sets the content of the container.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional attributes for the container element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    internal ElementReference ElementReference { get; private set; }

    /// <inheritdoc/>
    protected override void OnParametersSet() => ComponentCss.EnsureElementName(Element, nameof(DraggableContainer));

    private void BuildElement(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, Element);
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddElementReferenceCapture(2, element => ElementReference = element);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }
}