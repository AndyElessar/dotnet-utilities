namespace RazorClassLibrary.Anchor;

/// <summary>
/// A CSS anchor name (<c>&lt;dashed-ident&gt;</c>) shared by an <see cref="AnchorTarget"/> and the
/// <see cref="AnchorFollower"/> components positioned against it.
/// </summary>
/// <remarks>
/// <see cref="ToString"/> returns <see cref="Value"/>, so a name can also be applied to any element directly,
/// e.g. <c>style="anchor-name: @name"</c>.
/// </remarks>
public sealed record AnchorName
{
    private const string GeneratedPrefix = "--rcl-anchor-";

    private AnchorName(string value) => Value = value;

    /// <summary>
    /// Gets the CSS dashed-ident, e.g. <c>--rcl-anchor-3f9a1c2b4d5e</c>.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new anchor name that is unique within the page.
    /// </summary>
    /// <returns>A name such as <c>--rcl-anchor-3f9a1c2b4d5e</c>.</returns>
    public static AnchorName New() =>
        new(string.Concat(GeneratedPrefix, Guid.NewGuid().ToString("N").AsSpan(0, 12)));

    /// <summary>
    /// Creates an anchor name from an existing CSS dashed-ident.
    /// </summary>
    /// <param name="value">A CSS dashed-ident such as <c>--my-anchor</c>.</param>
    /// <returns>The anchor name.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> is not a valid dashed-ident.</exception>
    public static AnchorName From(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return AnchorCss.IsDashedIdent(value)
            ? new(value)
            : throw new ArgumentException($"'{value}' is not a valid CSS <dashed-ident>, e.g. \"--my-anchor\".", nameof(value));
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}
