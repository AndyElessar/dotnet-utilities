namespace RazorClassLibrary.Test.Anchor;

[Category(Constants.AnchorCategory)]
internal class AnchorFollowerTests : BunitContext
{
    private const string DefaultPositioning =
        "position-area: bottom span-right; "
        + "position-try-fallbacks: flip-block, flip-inline, flip-block flip-inline, --rcl-anchor-top-wide, --rcl-anchor-bottom-wide; "
        + "position-visibility: anchors-visible;";

    #region Anchor

    [Test]
    public async Task Render_WithoutAnchor_ShouldLeavePositionAnchorUnset()
    {
        // Act
        var cut = Render<AnchorFollower>();

        // Assert
        await Assert.That(cut.Find("div").GetAttribute("style")).IsEqualTo($"position: fixed; {DefaultPositioning}");
    }

    [Test]
    public async Task Render_WithAnchor_ShouldFollowAnchor()
    {
        // Act
        var cut = Render<AnchorFollower>(p => p.Add(x => x.Anchor, AnchorName.From("--trigger")));

        // Assert
        await Assert.That(cut.Find("div").GetAttribute("style"))
            .IsEqualTo($"position: fixed; position-anchor: --trigger; {DefaultPositioning}");
    }

    [Test]
    public async Task Render_InsideAnchorTarget_ShouldFollowEnclosingTarget()
    {
        // Act
        var cut = Render<AnchorTarget>(p => p
            .Add(x => x.Name, AnchorName.From("--avatar"))
            .AddChildContent<AnchorFollower>());

        // Assert
        await Assert.That(cut.Find("[rcl-anchor-follower]").GetAttribute("style"))
            .IsEqualTo($"position: fixed; position-anchor: --avatar; {DefaultPositioning}");
    }

    [Test]
    public async Task Render_InsideAnchorTargetWithoutName_ShouldFollowGeneratedName()
    {
        // Act
        var cut = Render<AnchorTarget>(p => p.AddChildContent<AnchorFollower>());

        // Assert
        var targetName = cut.Find("[rcl-anchor-target]").GetAttribute("style")!["anchor-name: ".Length..^1];
        await Assert.That(cut.Find("[rcl-anchor-follower]").GetAttribute("style"))
            .Contains($"position-anchor: {targetName};");
    }

    [Test]
    public async Task Render_WithAnchorInsideAnchorTarget_ShouldPreferAnchor()
    {
        // Act
        var cut = Render<AnchorTarget>(p => p
            .Add(x => x.Name, AnchorName.From("--avatar"))
            .AddChildContent<AnchorFollower>(f => f.Add(x => x.Anchor, AnchorName.From("--other"))));

        // Assert
        await Assert.That(cut.Find("[rcl-anchor-follower]").GetAttribute("style"))
            .Contains("position-anchor: --other;");
    }

    #endregion

    #region Positioning

    [Test]
    public async Task Render_CustomPositioning_ShouldWriteAllProperties()
    {
        // Act
        var cut = Render<AnchorFollower>(p => p
            .Add(x => x.Anchor, AnchorName.From("--trigger"))
            .Add(x => x.Area, PositionArea.TopSpanLeft)
            .Add(x => x.Fallbacks, [PositionTryOption.FlipStart, PositionTryOption.Area(PositionArea.Left), PositionTryOption.Rule("--mine")])
            .Add(x => x.TryOrder, PositionTryOrder.MostHeight)
            .Add(x => x.Visibility, PositionVisibility.NoOverflow)
            .Add(x => x.Strategy, AnchorPositionStrategy.Absolute)
            .Add(x => x.MatchAnchorWidth, true));

        // Assert
        await Assert.That(cut.Find("div").GetAttribute("style")).IsEqualTo(
            "position: absolute; position-anchor: --trigger; position-area: top span-left; "
            + "position-try-fallbacks: flip-start, left, --mine; position-try-order: most-height; "
            + "position-visibility: no-overflow; min-width: anchor-size(width);");
    }

    [Test]
    public async Task Render_EmptyFallbacks_ShouldDisableFallbacks()
    {
        // Act
        var cut = Render<AnchorFollower>(p => p.Add(x => x.Fallbacks, []));

        // Assert
        await Assert.That(cut.Find("div").GetAttribute("style")).Contains("position-try-fallbacks: none;");
    }

    [Test]
    public async Task Render_ParameterChanged_ShouldUpdateStyle()
    {
        // Arrange
        var cut = Render<AnchorFollower>();

        // Act
        cut.Render(p => p.Add(x => x.Area, PositionArea.Top).Add(x => x.Visibility, PositionVisibility.Always));

        // Assert
        var style = cut.Find("div").GetAttribute("style");
        await Assert.That(style).Contains("position-area: top;");
        await Assert.That(style).Contains("position-visibility: always;");
    }

    #endregion

    #region Popover

    [Test]
    [Arguments(AnchorPopoverMode.Auto, "auto")]
    [Arguments(AnchorPopoverMode.Manual, "manual")]
    [Arguments(AnchorPopoverMode.Hint, "hint")]
    public async Task Render_PopoverMode_ShouldWritePopoverAttribute(AnchorPopoverMode mode, string expected)
    {
        // Act
        var cut = Render<AnchorFollower>(p => p.Add(x => x.PopoverMode, mode));

        // Assert
        await Assert.That(cut.Find("div").GetAttribute("popover")).IsEqualTo(expected);
    }

    [Test]
    public async Task Render_PopoverModeNone_ShouldNotWritePopoverAttribute()
    {
        // Act
        var cut = Render<AnchorFollower>();

        // Assert
        await Assert.That(cut.Find("div").HasAttribute("popover")).IsFalse();
    }

    [Test]
    public async Task Render_PopoverAttribute_ShouldPassThrough()
    {
        // Act
        var cut = Render<AnchorFollower>(p => p.AddUnmatched("popover", "manual"));

        // Assert
        await Assert.That(cut.Find("div").GetAttribute("popover")).IsEqualTo("manual");
    }

    #endregion

    #region Element & attributes

    [Test]
    public async Task Render_ShouldAddCssScopeAndChildContent()
    {
        // Act
        var cut = Render<AnchorFollower>(p => p.AddChildContent("<p>Hello</p>"));

        // Assert
        var element = cut.Find("div");
        await Assert.That(element.HasAttribute("rcl-anchor-follower")).IsTrue();
        await Assert.That(element.QuerySelector("p")?.TextContent).IsEqualTo("Hello");
    }

    [Test]
    public async Task Render_ElementAndAttributes_ShouldRenderElementAndMergeStyle()
    {
        // Act
        var cut = Render<AnchorFollower>(p => p
            .Add(x => x.Element, "ul")
            .AddUnmatched("id", "menu")
            .AddUnmatched("class", "menu")
            .AddUnmatched("style", "margin-block: 12px"));

        // Assert
        var element = cut.Find("ul");
        await Assert.That(element.Id).IsEqualTo("menu");
        await Assert.That(element.ClassName).IsEqualTo("menu");
        await Assert.That(element.GetAttribute("style")).IsEqualTo($"position: fixed; {DefaultPositioning} margin-block: 12px");
    }

    [Test]
    public async Task Render_InvalidElement_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            Render<AnchorFollower>(p => p.Add(x => x.Element, "div onclick"));
            await Task.CompletedTask;
        });
    }

    [Test]
    public async Task Render_NullArea_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            Render<AnchorFollower>(p => p.Add(x => x.Area, null!));
            await Task.CompletedTask;
        });
    }

    [Test]
    public async Task Render_NullFallback_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            Render<AnchorFollower>(p => p.Add(x => x.Fallbacks, [PositionTryOption.FlipBlock, null!]));
            await Task.CompletedTask;
        });
    }

    #endregion
}
