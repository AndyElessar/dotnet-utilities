using System.Text.RegularExpressions;

namespace RazorClassLibrary.Test.Anchor;

[Category(Constants.AnchorCategory)]
internal class AnchorTargetTests : BunitContext
{
    [Test]
    public async Task Render_WithoutName_ShouldGenerateUniqueAnchorName()
    {
        // Act
        var first = Render<AnchorTarget>().Find("div").GetAttribute("style");
        var second = Render<AnchorTarget>().Find("div").GetAttribute("style");

        // Assert
        await Assert.That(Regex.IsMatch(first!, "^anchor-name: --rcl-anchor-[0-9a-f]{12};$")).IsTrue();
        await Assert.That(second).IsNotEqualTo(first);
    }

    [Test]
    public async Task Render_WithoutName_ShouldKeepGeneratedNameAcrossRenders()
    {
        // Arrange
        var cut = Render<AnchorTarget>();
        var before = cut.Find("div").GetAttribute("style");

        // Act
        cut.Render(p => p.AddUnmatched("title", "updated"));

        // Assert
        await Assert.That(cut.Find("div").GetAttribute("style")).IsEqualTo(before);
    }

    [Test]
    public async Task Render_WithName_ShouldUseName()
    {
        // Act
        var cut = Render<AnchorTarget>(p => p.Add(x => x.Name, AnchorName.From("--trigger")));

        // Assert
        await Assert.That(cut.Find("div").GetAttribute("style")).IsEqualTo("anchor-name: --trigger;");
    }

    [Test]
    public async Task Render_ShouldAddCssScopeAndChildContent()
    {
        // Act
        var cut = Render<AnchorTarget>(p => p.AddChildContent("<button>Open</button>"));

        // Assert
        var element = cut.Find("div");
        await Assert.That(element.HasAttribute("rcl-anchor-target")).IsTrue();
        await Assert.That(element.QuerySelector("button")?.TextContent).IsEqualTo("Open");
    }

    [Test]
    public async Task Render_ElementAndAttributes_ShouldRenderElementAndMergeStyle()
    {
        // Act
        var cut = Render<AnchorTarget>(p => p
            .Add(x => x.Name, AnchorName.From("--trigger"))
            .Add(x => x.Element, "span")
            .AddUnmatched("class", "trigger")
            .AddUnmatched("style", "left: 12px"));

        // Assert
        var element = cut.Find("span");
        await Assert.That(element.ClassName).IsEqualTo("trigger");
        await Assert.That(element.GetAttribute("style")).IsEqualTo("anchor-name: --trigger; left: 12px");
    }

    [Test]
    [Arguments("")]
    [Arguments("div onclick")]
    [Arguments("1div")]
    public async Task Render_InvalidElement_ShouldThrowInvalidOperationException(string element)
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            Render<AnchorTarget>(p => p.Add(x => x.Element, element));
            await Task.CompletedTask;
        });
    }
}
