using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using RazorClassLibrary;

namespace RazorClassLibrary.Anchor;

/// <summary>
/// Positions its content next to an anchor with CSS anchor positioning, and lets the browser move it to a fallback
/// position when it would overflow. No JavaScript is involved: the follower stays attached while scrolling or resizing.
/// </summary>
/// <remarks>
/// <para>
/// The anchor is <see cref="Anchor"/> or, when it is not set, the enclosing <see cref="AnchorTarget"/>. Without either,
/// <c>position-anchor</c> is not set and the browser uses the implicit anchor, e.g. the button that opened the popover.
/// </para>
/// <para>
/// Use <c>margin</c> for the gap to the anchor; it is mirrored automatically when the position flips.
/// </para>
/// </remarks>
public partial class AnchorFollower : ComponentBase
{
    // Must match the CssScope of AnchorFollower.razor.css in RazorClassLibrary.csproj; the element is built by hand,
    // so the scope attribute is not added by the Razor compiler.
    internal const string CssScope = "rcl-anchor-follower";

    private string? _popover;
    private string _style = string.Empty;

    /// <summary>
    /// Gets the fallbacks used when <see cref="Fallbacks"/> is not set: flip the block axis, the inline axis, both,
    /// then <see cref="PositionTryOption.TopWide"/> and <see cref="PositionTryOption.BottomWide"/>.
    /// </summary>
    public static IReadOnlyList<PositionTryOption> DefaultFallbacks { get; } =
    [
        PositionTryOption.FlipBlock,
        PositionTryOption.FlipInline,
        PositionTryOption.FlipBlockInline,
        PositionTryOption.TopWide,
        PositionTryOption.BottomWide,
    ];

    /// <summary>
    /// Gets or sets the anchor to follow (<c>position-anchor</c>). Defaults to the enclosing <see cref="AnchorTarget"/>.
    /// </summary>
    [Parameter]
    public AnchorName? Anchor { get; set; }

    /// <summary>
    /// Gets or sets where the follower is placed relative to the anchor (<c>position-area</c>).
    /// Defaults to <see cref="PositionArea.BottomSpanRight"/>.
    /// </summary>
    [Parameter]
    public PositionArea Area { get; set; } = PositionArea.BottomSpanRight;

    /// <summary>
    /// Gets or sets the positions tried in order when <see cref="Area"/> overflows (<c>position-try-fallbacks</c>).
    /// <see langword="null"/> uses <see cref="DefaultFallbacks"/>; an empty list disables the fallbacks.
    /// </summary>
    [Parameter]
    public IReadOnlyList<PositionTryOption>? Fallbacks { get; set; }

    /// <summary>
    /// Gets or sets how the fallbacks are ordered before they are tried (<c>position-try-order</c>).
    /// </summary>
    [Parameter]
    public PositionTryOrder TryOrder { get; set; }

    /// <summary>
    /// Gets or sets when the follower is hidden (<c>position-visibility</c>).
    /// Defaults to <see cref="PositionVisibility.AnchorsVisible"/>.
    /// </summary>
    [Parameter]
    public PositionVisibility Visibility { get; set; } = PositionVisibility.AnchorsVisible;

    /// <summary>
    /// Gets or sets the <c>popover</c> attribute. A popover is rendered in the top layer, so it is not clipped by
    /// <c>overflow</c> or covered by <c>z-index</c>. A <c>popover</c> attribute can also be passed directly.
    /// </summary>
    [Parameter]
    public AnchorPopoverMode PopoverMode { get; set; }

    /// <summary>
    /// Gets or sets the CSS <c>position</c>. Defaults to <see cref="AnchorPositionStrategy.Fixed"/>.
    /// </summary>
    [Parameter]
    public AnchorPositionStrategy Strategy { get; set; }

    /// <summary>
    /// Gets or sets whether the follower is at least as wide as the anchor (<c>min-width: anchor-size(width)</c>),
    /// e.g. for drop-down lists.
    /// </summary>
    [Parameter]
    public bool MatchAnchorWidth { get; set; }

    /// <summary>
    /// Gets or sets the name of the follower element. Defaults to <c>div</c>.
    /// </summary>
    [Parameter]
    public string Element { get; set; } = "div";

    /// <summary>
    /// Gets or sets the content of the follower.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional attributes for the follower element. <c>style</c> is merged after the positioning styles.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [CascadingParameter]
    private AnchorName? EnclosingAnchor { get; set; }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        ComponentCss.EnsureElementName(Element, nameof(AnchorFollower));
        if (Area is null)
        {
            throw new InvalidOperationException($"{nameof(AnchorFollower)}.{nameof(Area)} cannot be null.");
        }

        var style = new StringBuilder(256)
            .Append("position: ").Append(Strategy.ToCss()).Append(';');

        if ((Anchor ?? EnclosingAnchor) is { } anchor)
        {
            style.Append(" position-anchor: ").Append(anchor.Value).Append(';');
        }

        style.Append(" position-area: ").Append(Area.Value).Append(';');
        style.Append(" position-try-fallbacks: ");
        AppendFallbacks(style, Fallbacks ?? DefaultFallbacks);
        style.Append(';');

        if (TryOrder != PositionTryOrder.Normal)
        {
            style.Append(" position-try-order: ").Append(TryOrder.ToCss()).Append(';');
        }

        style.Append(" position-visibility: ").Append(Visibility.ToCss()).Append(';');

        if (MatchAnchorWidth)
        {
            style.Append(" min-width: anchor-size(width);");
        }

        _popover = PopoverMode.ToAttributeValue();
        _style = ComponentCss.MergeStyle(style.ToString(), AdditionalAttributes);
    }

    private static void AppendFallbacks(StringBuilder style, IReadOnlyList<PositionTryOption> fallbacks)
    {
        if (fallbacks.Count == 0)
        {
            style.Append("none");
            return;
        }

        for (var i = 0; i < fallbacks.Count; i++)
        {
            var fallback = fallbacks[i]
                ?? throw new InvalidOperationException($"{nameof(AnchorFollower)}.{nameof(Fallbacks)} cannot contain null.");

            if (i > 0)
            {
                style.Append(", ");
            }

            style.Append(fallback.Value);
        }
    }

    private void BuildElement(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, Element);
        builder.AddMultipleAttributes(1, AdditionalAttributes);
        if (_popover is not null)
        {
            builder.AddAttribute(2, "popover", _popover);
        }

        builder.AddAttribute(3, "style", _style);
        builder.AddAttribute(4, CssScope);
        builder.AddContent(5, ChildContent);
        builder.CloseElement();
    }
}
