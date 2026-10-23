using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using RazorClassLibrary;

namespace RazorClassLibrary.Anchor;

/// <summary>
/// Wraps its content in an element that is a CSS anchor (<c>anchor-name</c>), so that <see cref="AnchorFollower"/>
/// components can be positioned against it. Followers placed inside the target follow it automatically.
/// </summary>
/// <remarks>
/// The wrapper is <c>display: inline-block</c> so the anchor box fits the content; override it with a class or <c>style</c>.
/// Do not make the wrapper the containing block of a nested follower (e.g. <c>position: relative</c> with
/// <see cref="AnchorPositionStrategy.Absolute"/>, or <c>transform</c>), because an element cannot anchor to its own containing block.
/// </remarks>
public partial class AnchorTarget : ComponentBase
{
    // Must match the CssScope of AnchorTarget.razor.css in RazorClassLibrary.csproj; the element is built by hand,
    // so the scope attribute is not added by the Razor compiler.
    internal const string CssScope = "rcl-anchor-target";

    private AnchorName? _generatedName;
    private AnchorName _name = null!;
    private string _style = string.Empty;

    /// <summary>
    /// Gets or sets the anchor name. When <see langword="null"/>, a unique name is generated;
    /// set it to link followers that are not inside the target.
    /// </summary>
    [Parameter]
    public AnchorName? Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the wrapper element. Defaults to <c>div</c>; use <c>span</c> inside phrasing content.
    /// </summary>
    [Parameter]
    public string Element { get; set; } = "div";

    /// <summary>
    /// Gets or sets the content that becomes the anchor.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional attributes for the wrapper element. <c>style</c> is merged after <c>anchor-name</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        ComponentCss.EnsureElementName(Element, nameof(AnchorTarget));

        _name = Name ?? (_generatedName ??= AnchorName.New());
        _style = ComponentCss.MergeStyle($"anchor-name: {_name.Value};", AdditionalAttributes);
    }

    private void BuildElement(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, Element);
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        builder.AddAttribute(2, "style", _style);
        builder.AddAttribute(3, CssScope);
        builder.AddContent(4, ChildContent);
        builder.CloseElement();
    }
}
