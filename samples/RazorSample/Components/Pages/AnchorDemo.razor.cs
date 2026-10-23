using RazorClassLibrary.Anchor;

namespace RazorSample.Components.Pages;

public partial class AnchorDemo
{
    private static readonly AnchorName Trigger = AnchorName.From("--popup-trigger");

    // The gap to the anchor is a margin on the side facing it; fallbacks mirror it automatically.
    private static readonly IReadOnlyList<AreaChoice> Areas =
    [
        new("Below, left edges aligned", PositionArea.BottomSpanRight, "margin-block: 12px"),
        new("Below, centered", PositionArea.Bottom, "margin-block: 12px"),
        new("Below, right edges aligned", PositionArea.BottomSpanLeft, "margin-block: 12px"),
        new("Above, left edges aligned", PositionArea.TopSpanRight, "margin-block: 12px"),
        new("Right, top edges aligned", PositionArea.RightSpanBottom, "margin-inline: 12px"),
        new("Left, top edges aligned", PositionArea.LeftSpanBottom, "margin-inline: 12px"),
    ];

    private static readonly IReadOnlyList<FallbacksChoice> FallbackChoices =
    [
        new("default", "Default (flip, then shrink)", null),
        new("flip", "Flip only", [PositionTryOption.FlipBlock, PositionTryOption.FlipInline, PositionTryOption.FlipBlockInline]),
        new("flip-block", "Flip block only", [PositionTryOption.FlipBlock]),
        new("none", "None", []),
    ];

    private static readonly IReadOnlyList<EnumChoice<PositionVisibility>> VisibilityChoices =
    [
        new(PositionVisibility.AnchorsVisible, "anchors-visible"),
        new(PositionVisibility.NoOverflow, "no-overflow"),
        new(PositionVisibility.Always, "always"),
    ];

    private static readonly IReadOnlyList<string> Fruits = ["Apple", "Banana", "Cherry", "Grape", "Mango"];

    private readonly AnchorName _menuAnchor = AnchorName.New();
    private readonly AnchorName _clippedItem = AnchorName.New();

    private string _area = PositionArea.BottomSpanRight.Value;
    private string _fallbacks = "default";
    private PositionVisibility _visibility = PositionVisibility.AnchorsVisible;
    // Closed initially: the browser keeps using the last fallback that fit, so the popup is laid out only after the
    // playground has scrolled to its centered button.
    private bool _popupOpen;
    private int _notifications = 3;
    private string? _fruit;

    private AreaChoice SelectedArea => Areas.First(choice => choice.Area.Value == _area);

    private FallbacksChoice SelectedFallbacks => FallbackChoices.First(choice => choice.Key == _fallbacks);

    private sealed record AreaChoice(string Label, PositionArea Area, string Gap);

    private sealed record FallbacksChoice(string Key, string Label, IReadOnlyList<PositionTryOption>? Fallbacks);

    private sealed record EnumChoice<TValue>(TValue Value, string Label);
}
