using System.Text.RegularExpressions;

namespace RazorClassLibrary.Test.Anchor;

[Category(Constants.AnchorCategory)]
internal class AnchorValueTests
{
    #region AnchorName

    [Test]
    public async Task AnchorName_New_ShouldCreateUniqueDashedIdent()
    {
        // Act
        var first = AnchorName.New();
        var second = AnchorName.New();

        // Assert
        await Assert.That(Regex.IsMatch(first.Value, "^--rcl-anchor-[0-9a-f]{12}$")).IsTrue();
        await Assert.That(second).IsNotEqualTo(first);
    }

    [Test]
    [Arguments("--a")]
    [Arguments("--my-anchor")]
    [Arguments("--anchor_1")]
    [Arguments("--錨點")]
    public async Task AnchorName_From_ValidDashedIdent_ShouldKeepValue(string value)
    {
        // Act
        var name = AnchorName.From(value);

        // Assert
        await Assert.That(name.Value).IsEqualTo(value);
        await Assert.That(name.ToString()).IsEqualTo(value);
    }

    [Test]
    [Arguments("")]
    [Arguments("--")]
    [Arguments("-a")]
    [Arguments("my-anchor")]
    [Arguments("--a b")]
    [Arguments("--a; color: red")]
    [Arguments("--a}")]
    [Arguments("--a\"")]
    public async Task AnchorName_From_InvalidValue_ShouldThrowArgumentException(string value)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            AnchorName.From(value);
            await Task.CompletedTask;
        });
    }

    [Test]
    public async Task AnchorName_SameValue_ShouldBeEqual()
    {
        // Assert
        await Assert.That(AnchorName.From("--a")).IsEqualTo(AnchorName.From("--a"));
    }

    #endregion

    #region PositionArea

    [Test]
    public async Task PositionArea_Presets_ShouldMapToCssKeywords()
    {
        // Assert
        await Assert.That(PositionArea.BottomSpanRight.Value).IsEqualTo("bottom span-right");
        await Assert.That(PositionArea.TopLeft.Value).IsEqualTo("top left");
        await Assert.That(PositionArea.LeftSpanBottom.Value).IsEqualTo("left span-bottom");
        await Assert.That(PositionArea.Center.ToString()).IsEqualTo("center");
    }

    [Test]
    [Arguments("block-end span-inline-end", "block-end span-inline-end")]
    [Arguments("  top  ", "top")]
    public async Task PositionArea_Custom_Keywords_ShouldCreateArea(string value, string expected)
    {
        // Act
        var area = PositionArea.Custom(value);

        // Assert
        await Assert.That(area.Value).IsEqualTo(expected);
    }

    [Test]
    [Arguments("")]
    [Arguments("   ")]
    [Arguments("top; color: red")]
    [Arguments("top/bottom")]
    [Arguments("anchor(--a)")]
    public async Task PositionArea_Custom_InvalidValue_ShouldThrowArgumentException(string value)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            PositionArea.Custom(value);
            await Task.CompletedTask;
        });
    }

    #endregion

    #region PositionTryOption

    [Test]
    public async Task PositionTryOption_Area_ShouldUseAreaValue()
    {
        // Act
        var option = PositionTryOption.Area(PositionArea.TopSpanLeft);

        // Assert
        await Assert.That(option.Value).IsEqualTo("top span-left");
    }

    [Test]
    public async Task PositionTryOption_Rule_ValidName_ShouldUseName()
    {
        // Act
        var option = PositionTryOption.Rule("--my-fallback");

        // Assert
        await Assert.That(option.Value).IsEqualTo("--my-fallback");
    }

    [Test]
    [Arguments("my-fallback")]
    [Arguments("--a, flip-block")]
    public async Task PositionTryOption_Rule_InvalidName_ShouldThrowArgumentException(string name)
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            PositionTryOption.Rule(name);
            await Task.CompletedTask;
        });
    }

    [Test]
    public async Task DefaultFallbacks_ShouldFlipThenUseWideRules()
    {
        // Act
        var values = AnchorFollower.DefaultFallbacks.Select(o => o.Value);

        // Assert
        await Assert.That(string.Join(", ", values))
            .IsEqualTo("flip-block, flip-inline, flip-block flip-inline, --rcl-anchor-top-wide, --rcl-anchor-bottom-wide");
    }

    #endregion
}
