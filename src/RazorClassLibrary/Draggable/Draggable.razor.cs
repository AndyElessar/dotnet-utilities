using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using RazorClassLibrary.Anchor;

namespace RazorClassLibrary.Draggable;

/// <summary>
/// Lets the user move its content with a mouse, touch or pen by dragging it. The content is moved with the CSS
/// <c>translate</c> property, so it keeps its place in the layout and the surrounding content does not shift.
/// </summary>
/// <remarks>
/// <para>
/// By default the whole element starts a drag, except form controls, buttons and links inside it. Add a
/// <see cref="DraggableHandle"/> to drag only by that part.
/// </para>
/// <para>
/// <see cref="Bounds"/> keeps the element inside the enclosing <see cref="DraggableContainer"/>, its parent element
/// or the viewport, also when that area is resized.
/// </para>
/// </remarks>
public partial class Draggable : ComponentBase, IAsyncDisposable
{
    // Must match the CssScope of Draggable.razor.css in RazorClassLibrary.csproj; the element is built by hand,
    // so the scope attribute is not added by the Razor compiler.
    internal const string CssScope = "rcl-draggable";

    private const string ModulePath = "./_content/RazorClassLibrary/Draggable/Draggable.razor.js";

    private ElementReference _element;
    private Task<IJSObjectReference>? _moduleTask;
    private IJSObjectReference? _instance;
    private DotNetObjectReference<Draggable>? _dotNetRef;
    private DragOffset _offset;
    private DragOffset? _lastOffsetParameter;
    private int _handleCount;
    private (DraggableBounds Bounds, string? ContainerId, bool Handles, bool Disabled)? _jsOptions;
    private bool _disposed;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    [CascadingParameter]
    private DraggableContainer? Container { get; set; }

    /// <summary>
    /// Gets or sets the name of the draggable element. Defaults to <c>div</c>.
    /// </summary>
    [Parameter]
    public string Element { get; set; } = "div";

    /// <summary>
    /// Gets or sets the content to drag.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the area that the element is kept inside of. Defaults to <see cref="DraggableBounds.Container"/>,
    /// the enclosing <see cref="DraggableContainer"/>; without a container the element is not bounded.
    /// </summary>
    [Parameter]
    public DraggableBounds Bounds { get; set; } = DraggableBounds.Container;

    /// <summary>
    /// Gets or sets how far the element has been moved from its layout position. Setting it moves the element
    /// (it is not clamped to <see cref="Bounds"/> until the user drags it or the bounds are resized).
    /// Use <c>@bind-Offset</c> to keep it in sync.
    /// </summary>
    [Parameter]
    public DragOffset Offset { get; set; }

    /// <summary>
    /// Gets or sets a callback that is invoked when the user has dragged the element, or when it was moved back
    /// inside its bounds, with the new offset.
    /// </summary>
    [Parameter]
    public EventCallback<DragOffset> OffsetChanged { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether dragging is disabled. The element keeps its current offset.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets additional attributes for the draggable element. A <c>style</c> attribute is merged with the
    /// <c>translate</c> set by the component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        AnchorCss.EnsureElementName(Element, nameof(Draggable));
        if (!Enum.IsDefined(Bounds))
        {
            throw new InvalidOperationException($"{nameof(Draggable)}.{nameof(Bounds)} has an invalid value '{Bounds}'.");
        }

        // Adopt the parameter only when the parent changes it, so a parent that does not bind Offset does not
        // move the element back to its initial offset whenever it re-renders.
        if (_lastOffsetParameter != Offset)
        {
            _lastOffsetParameter = Offset;
            _offset = Offset;
        }
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var container = Bounds == DraggableBounds.Container ? Container?.ElementReference : null;
        var options = (Bounds, container?.Id, _handleCount > 0, Disabled);
        if (_jsOptions == options || _disposed)
        {
            return;
        }

        var wasCreated = _jsOptions is not null;
        _jsOptions = options;
        try
        {
            if (!wasCreated)
            {
                var module = await GetModuleAsync();
                _dotNetRef ??= DotNetObjectReference.Create(this);
                _instance = await module.InvokeAsync<IJSObjectReference>(
                    "createDraggable", _element, _dotNetRef, ToJs(options.Bounds), container, options.Item3, options.Disabled);
                if (_disposed)
                {
                    await DisposeInstanceAsync();
                }
            }
            else if (_instance is not null)
            {
                await _instance.InvokeVoidAsync("update", ToJs(options.Bounds), container, options.Item3, options.Disabled);
            }
        }
        catch (JSDisconnectedException)
        {
        }
    }

    /// <summary>
    /// Called by the JavaScript module after a drag, or after the element was moved back inside its bounds.
    /// </summary>
    /// <param name="x">The new horizontal offset in CSS pixels.</param>
    /// <param name="y">The new vertical offset in CSS pixels.</param>
    [JSInvokable]
    public async Task OnMoved(double x, double y)
    {
        _offset = new DragOffset(x, y);
        // Re-render so the rendered translate matches the DOM; otherwise setting Offset back to the last rendered
        // value would not be applied.
        StateHasChanged();
        await OffsetChanged.InvokeAsync(_offset);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        _disposed = true;
        try
        {
            await DisposeInstanceAsync();
            if (_moduleTask is { IsCompletedSuccessfully: true })
            {
                await (await _moduleTask).DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
        }

        _dotNetRef?.Dispose();
        GC.SuppressFinalize(this);
    }

    internal void AddHandle()
    {
        if (_handleCount++ == 0)
        {
            StateHasChanged();
        }
    }

    internal void RemoveHandle()
    {
        if (--_handleCount == 0 && !_disposed)
        {
            StateHasChanged();
        }
    }

    private static string ToJs(DraggableBounds bounds) => bounds switch
    {
        DraggableBounds.None => "none",
        DraggableBounds.Container => "container",
        DraggableBounds.Parent => "parent",
        DraggableBounds.Viewport => "viewport",
        _ => throw new ArgumentOutOfRangeException(nameof(bounds), bounds, null),
    };

    private Task<IJSObjectReference> GetModuleAsync() =>
        _moduleTask ??= JS.InvokeAsync<IJSObjectReference>("import", ModulePath).AsTask();

    private async Task DisposeInstanceAsync()
    {
        if (_instance is null)
        {
            return;
        }

        var instance = _instance;
        _instance = null;
        await instance.InvokeVoidAsync("dispose");
        await instance.DisposeAsync();
    }

    private void BuildElement(RenderTreeBuilder builder)
    {
        var translate = _offset == DragOffset.Zero
            ? string.Empty
            : string.Create(CultureInfo.InvariantCulture, $"translate: {_offset.X}px {_offset.Y}px;");
        var style = AnchorCss.MergeStyle(translate, AdditionalAttributes);

        builder.OpenElement(0, Element);
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, CssScope);
        builder.AddAttribute(3, "data-draggable", true);
        builder.AddAttribute(4, "data-handles", _handleCount > 0);
        builder.AddAttribute(5, "data-disabled", Disabled);
        builder.AddAttribute(6, "style", string.IsNullOrWhiteSpace(style) ? null : style.Trim());
        builder.AddElementReferenceCapture(7, element => _element = element);
        builder.AddContent(8, ChildContent);
        builder.CloseElement();
    }
}