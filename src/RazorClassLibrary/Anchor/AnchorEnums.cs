namespace RazorClassLibrary.Anchor;

/// <summary>
/// When an <see cref="AnchorFollower"/> is hidden (CSS <c>position-visibility</c>).
/// </summary>
public enum PositionVisibility
{
    /// <summary>
    /// Hidden while the anchor is fully clipped by a box between it and the follower's containing block, e.g. scrolled
    /// out of a scrollable panel (<c>anchors-visible</c>). Scrolling the viewport does not hide a fixed follower.
    /// </summary>
    AnchorsVisible,

    /// <summary>Hidden while the follower overflows its containing block (<c>no-overflow</c>).</summary>
    NoOverflow,

    /// <summary>Always shown (<c>always</c>).</summary>
    Always,
}

/// <summary>
/// How the fallback positions are ordered before they are tried (CSS <c>position-try-order</c>).
/// </summary>
public enum PositionTryOrder
{
    /// <summary>In the declared order (<c>normal</c>).</summary>
    Normal,

    /// <summary>The option with the most available height first (<c>most-height</c>).</summary>
    MostHeight,

    /// <summary>The option with the most available width first (<c>most-width</c>).</summary>
    MostWidth,

    /// <summary>The option with the most available block size first (<c>most-block-size</c>).</summary>
    MostBlockSize,

    /// <summary>The option with the most available inline size first (<c>most-inline-size</c>).</summary>
    MostInlineSize,
}

/// <summary>
/// The HTML <c>popover</c> state of an <see cref="AnchorFollower"/>.
/// </summary>
public enum AnchorPopoverMode
{
    /// <summary>Not a popover: the follower is shown while it is rendered.</summary>
    None,

    /// <summary><c>popover="auto"</c>: closes on click outside or Esc, and closes other auto popovers when opened.</summary>
    Auto,

    /// <summary><c>popover="manual"</c>: closes only when explicitly hidden.</summary>
    Manual,

    /// <summary><c>popover="hint"</c>: for tooltips; opening it does not close auto popovers.</summary>
    Hint,
}

/// <summary>
/// The CSS <c>position</c> of an <see cref="AnchorFollower"/>.
/// </summary>
public enum AnchorPositionStrategy
{
    /// <summary><c>position: fixed</c>: not clipped by scrolling or <c>overflow</c> ancestors.</summary>
    Fixed,

    /// <summary><c>position: absolute</c>: positioned within the nearest positioned ancestor.</summary>
    Absolute,
}
