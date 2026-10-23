using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace RazorClassLibrary.Test.Draggable;

[Category(Constants.DraggableCategory)]
internal class DraggableTests : BunitContext
{
    private const string ModulePath = "./_content/RazorClassLibrary/Draggable/Draggable.razor.js";

    private readonly BunitJSInterop _module;
    private readonly BunitJSInterop _instance;

    public DraggableTests()
    {
        _module = JSInterop.SetupModule(ModulePath);
        _instance = _module.SetupModule(invocation => invocation.Identifier == "createDraggable");
        _instance.SetupVoid(invocation => invocation.Identifier == "update").SetVoidResult();
        _instance.SetupVoid("dispose").SetVoidResult();
    }

    [Test]
    public async Task Render_ShouldAddScopeMarkersAndChildContent()
    {
        // Act
        var cut = Render<RazorClassLibrary.Draggable.Draggable>(p => p
            .AddChildContent("<span>Item</span>")
            .AddUnmatched("class", "card"));

        // Assert
        var element = cut.Find("div");
        await Assert.That(element.HasAttribute("rcl-draggable")).IsTrue();
        await Assert.That(element.HasAttribute("data-draggable")).IsTrue();
        await Assert.That(element.HasAttribute("data-handles")).IsFalse();
        await Assert.That(element.HasAttribute("data-disabled")).IsFalse();
        await Assert.That(element.HasAttribute("style")).IsFalse();
        await Assert.That(element.ClassName).IsEqualTo("card");
        await Assert.That(element.InnerHtml).Contains("<span>Item</span>");
    }

    [Test]
    public async Task Render_WithOffset_ShouldTranslateWithInvariantCulture()
    {
        // Arrange
        var culture = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");

        try
        {
            // Act
            var cut = Render<RazorClassLibrary.Draggable.Draggable>(p => p
                .Add(x => x.Offset, new DragOffset(1.5, -20))
                .AddUnmatched("style", "color: red;"));

            // Assert
            await Assert.That(cut.Find("div").GetAttribute("style")).IsEqualTo("translate: 1.5px -20px; color: red;");
        }
        finally
        {
            CultureInfo.CurrentCulture = culture;
        }
    }

    [Test]
    public async Task Render_WithElementAndDisabled_ShouldUseElementAndMarkDisabled()
    {
        // Act
        var cut = Render<RazorClassLibrary.Draggable.Draggable>(p => p
            .Add(x => x.Element, "section")
            .Add(x => x.Disabled, true));

        // Assert
        await Assert.That(cut.Find("section").HasAttribute("data-disabled")).IsTrue();
    }

    [Test]
    public async Task Render_WithInvalidElement_ShouldThrow()
    {
        // Act & Assert
        await Assert.That(() => Render<RazorClassLibrary.Draggable.Draggable>(p => p.Add(x => x.Element, "div onclick=x")))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Render_WithHandle_ShouldMarkDraggableAndHandle()
    {
        // Act
        var cut = Render<RazorClassLibrary.Draggable.Draggable>(p => p.AddChildContent<DraggableHandle>(h => h
            .Add(x => x.Element, "header")
            .AddChildContent("Title")));

        // Assert
        await Assert.That(cut.Find("[data-draggable]").HasAttribute("data-handles")).IsTrue();
        await Assert.That(cut.Find("header").HasAttribute("data-draggable-handle")).IsTrue();
        await Assert.That((bool)_module.VerifyInvoke("createDraggable").Arguments[4]!).IsTrue();
    }

    [Test]
    public async Task Render_HandleOutsideDraggable_ShouldThrow()
    {
        // Act & Assert
        await Assert.That(() => Render<DraggableHandle>()).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Render_InContainer_ShouldPassContainerElement()
    {
        // Act
        var cut = Render<DraggableContainer>(p => p
            .Add(x => x.Element, "section")
            .AddChildContent<RazorClassLibrary.Draggable.Draggable>());

        // Assert
        var invocation = _module.VerifyInvoke("createDraggable");
        await Assert.That(cut.Find("section").InnerHtml).Contains("data-draggable");
        await Assert.That(invocation.Arguments[2]).IsEqualTo("container");
        await Assert.That(invocation.Arguments[3]).IsTypeOf<ElementReference>();
    }

    [Test]
    public async Task Render_InContainerWithOtherBounds_ShouldNotPassContainer()
    {
        // Act
        Render<DraggableContainer>(p => p.AddChildContent<RazorClassLibrary.Draggable.Draggable>(d => d
            .Add(x => x.Bounds, DraggableBounds.Viewport)));

        // Assert
        var invocation = _module.VerifyInvoke("createDraggable");
        await Assert.That(invocation.Arguments[2]).IsEqualTo("viewport");
        await Assert.That(invocation.Arguments[3]).IsNull();
    }

    [Test]
    public async Task Render_WhenOptionsChange_ShouldUpdateModule()
    {
        // Arrange
        var cut = Render<RazorClassLibrary.Draggable.Draggable>();

        // Act
        cut.Render(p => p.AddUnmatched("title", "unrelated"));
        cut.Render(p => p.Add(x => x.Disabled, true).Add(x => x.Bounds, DraggableBounds.Parent));

        // Assert
        var update = _instance.VerifyInvoke("update");
        await Assert.That(update.Arguments[0]).IsEqualTo("parent");
        await Assert.That((bool)update.Arguments[3]!).IsTrue();
        await Assert.That(_module.Invocations["createDraggable"].Count).IsEqualTo(1);
    }

    [Test]
    public async Task OnMoved_ShouldRenderOffsetAndInvokeOffsetChanged()
    {
        // Arrange
        DragOffset? changed = null;
        var cut = Render<RazorClassLibrary.Draggable.Draggable>(p => p
            .Add(x => x.OffsetChanged, offset => changed = offset));

        // Act
        await cut.InvokeAsync(() => cut.Instance.OnMoved(10, 20.25));

        // Assert
        await Assert.That(changed).IsEqualTo(new DragOffset(10, 20.25));
        await Assert.That(cut.Find("div").GetAttribute("style")).IsEqualTo("translate: 10px 20.25px;");
    }

    [Test]
    public async Task Render_WithUnchangedOffset_ShouldKeepDraggedOffset()
    {
        // Arrange
        var cut = Render<RazorClassLibrary.Draggable.Draggable>(p => p.Add(x => x.Offset, new DragOffset(5, 5)));
        await cut.InvokeAsync(() => cut.Instance.OnMoved(30, 40));

        // Act
        cut.Render(p => p.Add(x => x.Offset, new DragOffset(5, 5)));
        var unchanged = cut.Find("div").GetAttribute("style");
        cut.Render(p => p.Add(x => x.Offset, DragOffset.Zero));

        // Assert
        await Assert.That(unchanged).IsEqualTo("translate: 30px 40px;");
        await Assert.That(cut.Find("div").HasAttribute("style")).IsFalse();
    }

    [Test]
    public async Task DisposeAsync_ShouldDisposeModuleInstance()
    {
        // Arrange
        var cut = Render<RazorClassLibrary.Draggable.Draggable>();

        // Act
        await cut.Instance.DisposeAsync();

        // Assert
        _instance.VerifyInvoke("dispose");
        await Assert.That(_instance.Invocations["dispose"].Count).IsEqualTo(1);
    }
}