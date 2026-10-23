namespace RazorClassLibrary.Anchor;

/// <summary>
/// One entry of the CSS <c>position-try-fallbacks</c> list: a position the browser tries, in order,
/// when the follower would overflow at its current position.
/// </summary>
public sealed record PositionTryOption
{
    internal const string TopWideRuleName = "--rcl-anchor-top-wide";
    internal const string BottomWideRuleName = "--rcl-anchor-bottom-wide";

    private PositionTryOption(string value) => Value = value;

    /// <summary>
    /// Gets the CSS value, e.g. <c>flip-block</c>.
    /// </summary>
    public string Value { get; }

    /// <summary>Mirrors the position across the block axis, e.g. below → above (<c>flip-block</c>).</summary>
    public static PositionTryOption FlipBlock { get; } = new("flip-block");

    /// <summary>Mirrors the position across the inline axis, e.g. left-aligned → right-aligned (<c>flip-inline</c>).</summary>
    public static PositionTryOption FlipInline { get; } = new("flip-inline");

    /// <summary>Mirrors the position across the start-start/end-end diagonal, e.g. below → right (<c>flip-start</c>).</summary>
    public static PositionTryOption FlipStart { get; } = new("flip-start");

    /// <summary>Mirrors the position across both axes (<c>flip-block flip-inline</c>).</summary>
    public static PositionTryOption FlipBlockInline { get; } = new("flip-block flip-inline");

    /// <summary>
    /// Above the anchor across the full width, shrinking <c>max-height</c> to the space above the anchor.
    /// </summary>
    /// <remarks>
    /// Uses a built-in <c>@position-try</c> rule. Give non-popover followers <c>overflow: auto</c> so the content can scroll;
    /// popovers already have it.
    /// </remarks>
    public static PositionTryOption TopWide { get; } = new(TopWideRuleName);

    /// <summary>
    /// Below the anchor across the full width, shrinking <c>max-height</c> to the space below the anchor.
    /// </summary>
    /// <remarks>
    /// Uses a built-in <c>@position-try</c> rule. Give non-popover followers <c>overflow: auto</c> so the content can scroll;
    /// popovers already have it.
    /// </remarks>
    public static PositionTryOption BottomWide { get; } = new(BottomWideRuleName);

    /// <summary>
    /// Tries another <c>position-area</c>.
    /// </summary>
    /// <param name="area">The position area to try.</param>
    /// <returns>The fallback option.</returns>
    public static PositionTryOption Area(PositionArea area)
    {
        ArgumentNullException.ThrowIfNull(area);
        return new(area.Value);
    }

    /// <summary>
    /// Tries a custom <c>@position-try</c> rule defined in the application's CSS.
    /// </summary>
    /// <param name="name">The rule name, e.g. <c>--my-fallback</c>.</param>
    /// <returns>The fallback option.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is not a valid dashed-ident.</exception>
    public static PositionTryOption Rule(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        return AnchorCss.IsDashedIdent(name)
            ? new(name)
            : throw new ArgumentException($"'{name}' is not a valid @position-try name, e.g. \"--my-fallback\".", nameof(name));
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}
