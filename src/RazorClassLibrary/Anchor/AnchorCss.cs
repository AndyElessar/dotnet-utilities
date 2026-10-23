namespace RazorClassLibrary.Anchor;

/// <summary>
/// CSS helpers for the anchor components. Values written into inline styles are restricted to identifier
/// and keyword characters so they cannot inject extra declarations.
/// </summary>
internal static class AnchorCss
{
    public static bool IsDashedIdent(string? value)
    {
        if (value is not { Length: > 2 } || !value.StartsWith("--", StringComparison.Ordinal))
        {
            return false;
        }

        foreach (var c in value.AsSpan(2))
        {
            if (!(char.IsAsciiLetterOrDigit(c) || c is '-' or '_' || c >= '\u0080'))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsKeywordList(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        foreach (var c in value)
        {
            if (!(char.IsAsciiLetter(c) || c is '-' or ' '))
            {
                return false;
            }
        }

        return true;
    }

    extension(PositionVisibility value)
    {
        public string ToCss() => value switch
        {
            PositionVisibility.AnchorsVisible => "anchors-visible",
            PositionVisibility.NoOverflow => "no-overflow",
            PositionVisibility.Always => "always",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
    }

    extension(PositionTryOrder value)
    {
        public string ToCss() => value switch
        {
            PositionTryOrder.Normal => "normal",
            PositionTryOrder.MostHeight => "most-height",
            PositionTryOrder.MostWidth => "most-width",
            PositionTryOrder.MostBlockSize => "most-block-size",
            PositionTryOrder.MostInlineSize => "most-inline-size",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
    }

    extension(AnchorPositionStrategy value)
    {
        public string ToCss() => value switch
        {
            AnchorPositionStrategy.Fixed => "fixed",
            AnchorPositionStrategy.Absolute => "absolute",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
    }

    extension(AnchorPopoverMode value)
    {
        /// <summary>
        /// Gets the <c>popover</c> attribute value, or <see langword="null"/> when the follower is not a popover.
        /// </summary>
        public string? ToAttributeValue() => value switch
        {
            AnchorPopoverMode.None => null,
            AnchorPopoverMode.Auto => "auto",
            AnchorPopoverMode.Manual => "manual",
            AnchorPopoverMode.Hint => "hint",
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
    }
}
