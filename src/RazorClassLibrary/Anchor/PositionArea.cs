namespace RazorClassLibrary.Anchor;

/// <summary>
/// A CSS <c>position-area</c> value: the cells of the 3×3 grid around the anchor that the follower is placed in.
/// </summary>
/// <remarks>
/// The presets cover the common placements. Use <see cref="Custom(string)"/> for any other value,
/// e.g. logical keywords such as <c>block-end span-inline-end</c>.
/// </remarks>
public sealed record PositionArea
{
    private PositionArea(string value) => Value = value;

    /// <summary>
    /// Gets the CSS value, e.g. <c>bottom span-right</c>.
    /// </summary>
    public string Value { get; }

    /// <summary>Above the anchor, centered (<c>top</c>).</summary>
    public static PositionArea Top { get; } = new("top");

    /// <summary>Below the anchor, centered (<c>bottom</c>).</summary>
    public static PositionArea Bottom { get; } = new("bottom");

    /// <summary>Left of the anchor, centered (<c>left</c>).</summary>
    public static PositionArea Left { get; } = new("left");

    /// <summary>Right of the anchor, centered (<c>right</c>).</summary>
    public static PositionArea Right { get; } = new("right");

    /// <summary>Over the anchor, centered (<c>center</c>).</summary>
    public static PositionArea Center { get; } = new("center");

    /// <summary>Diagonally above-left of the anchor (<c>top left</c>).</summary>
    public static PositionArea TopLeft { get; } = new("top left");

    /// <summary>Diagonally above-right of the anchor (<c>top right</c>).</summary>
    public static PositionArea TopRight { get; } = new("top right");

    /// <summary>Diagonally below-left of the anchor (<c>bottom left</c>).</summary>
    public static PositionArea BottomLeft { get; } = new("bottom left");

    /// <summary>Diagonally below-right of the anchor (<c>bottom right</c>).</summary>
    public static PositionArea BottomRight { get; } = new("bottom right");

    /// <summary>Above the anchor, right edges aligned, extending to the left (<c>top span-left</c>).</summary>
    public static PositionArea TopSpanLeft { get; } = new("top span-left");

    /// <summary>Above the anchor, left edges aligned, extending to the right (<c>top span-right</c>).</summary>
    public static PositionArea TopSpanRight { get; } = new("top span-right");

    /// <summary>Below the anchor, right edges aligned, extending to the left (<c>bottom span-left</c>).</summary>
    public static PositionArea BottomSpanLeft { get; } = new("bottom span-left");

    /// <summary>Below the anchor, left edges aligned, extending to the right (<c>bottom span-right</c>).</summary>
    public static PositionArea BottomSpanRight { get; } = new("bottom span-right");

    /// <summary>Left of the anchor, bottom edges aligned, extending upwards (<c>left span-top</c>).</summary>
    public static PositionArea LeftSpanTop { get; } = new("left span-top");

    /// <summary>Left of the anchor, top edges aligned, extending downwards (<c>left span-bottom</c>).</summary>
    public static PositionArea LeftSpanBottom { get; } = new("left span-bottom");

    /// <summary>Right of the anchor, bottom edges aligned, extending upwards (<c>right span-top</c>).</summary>
    public static PositionArea RightSpanTop { get; } = new("right span-top");

    /// <summary>Right of the anchor, top edges aligned, extending downwards (<c>right span-bottom</c>).</summary>
    public static PositionArea RightSpanBottom { get; } = new("right span-bottom");

    /// <summary>
    /// Creates a <c>position-area</c> from CSS keywords.
    /// </summary>
    /// <param name="value">One or two <c>position-area</c> keywords, e.g. <c>block-end span-inline-end</c>.</param>
    /// <returns>The position area.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> contains anything other than keywords.</exception>
    public static PositionArea Custom(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return AnchorCss.IsKeywordList(value)
            ? new(value.Trim())
            : throw new ArgumentException($"'{value}' is not a valid position-area; use keywords only, e.g. \"block-end span-inline-end\".", nameof(value));
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}
