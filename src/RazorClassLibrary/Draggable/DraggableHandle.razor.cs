using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using RazorClassLibrary;
using RazorClassLibrary.Anchor;

namespace RazorClassLibrary.Draggable;

/// <summary>
/// Marks the part of a <see cref="Draggable"/> that starts a drag, e.g. a title bar. Once a draggable contains a
/// handle, pressing the rest of it no longer drags it. A draggable can have several handles.
/// </summary>
public partial class DraggableHandle : ComponentBase, IDisposable
{
    private Draggable? _owner;

    /// <summary>
    /// Gets or sets the name of the handle element. Defaults to <c>div</c>.
    /// </summary>
    [Parameter]
    public string Element { get; set; } = "div";

    /// <summary>
    /// Gets or sets the content of the handle.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional attributes for the handle element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter]
    private Draggable? Owner { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        _owner = Owner
            ?? throw new InvalidOperationException($"{nameof(DraggableHandle)} must be placed inside a {nameof(Draggable)}.");
        _owner.AddHandle();
    }

    /// <inheritdoc/>
    protected override void OnParametersSet() => ComponentCss.EnsureElementName(Element, nameof(DraggableHandle));

    /// <inheritdoc/>
    public void Dispose()
    {
        _owner?.RemoveHandle();
        _owner = null;
        GC.SuppressFinalize(this);
    }

    private void BuildElement(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, Element);
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "data-draggable-handle", true);
        builder.AddContent(3, ChildContent);
        builder.CloseElement();
    }
}