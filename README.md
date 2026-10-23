# dotnet-utilities

A collection of .NET utilities and helper functions.  
Feel free to copy and use any code from this repository in your own projects!

## Features

### A bunch of C# Extensions

Includes extensions for:

- Collections
- Strings
- DateTime?, DateOnly? and TimeOnly?
- Numbers
- Enums
- Task? and ValueTask?
- Safe fire and forget for async methods
- Control Flow
- Duck Type

### ROC (Republic of China) Calendar Types

Custom date/time types for Taiwan's ROC calendar system:

- **RocDateTime** - A `DateTime` equivalent using ROC year (e.g., 114 instead of 2025)
- **RocDateOnly** - A `DateOnly` equivalent using ROC year
- Includes System.Text.Json, EntityFrameworkCore and Dapper converters for serialization and database mapping

### Statistics

Basic statistical calculation extensions:

- **Average**
- **Stdev**
- **Deviation**
- **LinearRegression**
- **RSquared** - Coefficient of determination

### Anchor Components (Blazor)

Components in `RazorClassLibrary.Anchor` that attach one element to another with
[CSS anchor positioning](https://developer.mozilla.org/en-US/docs/Web/CSS/CSS_anchor_positioning), without any JavaScript:

- **AnchorTarget** - Makes its content an anchor (`anchor-name`); the name is generated unless `Name` is set
- **AnchorFollower** - Places its content next to an anchor (`position-anchor`, `position-area`, `position-try-fallbacks`, `position-visibility`), optionally as a popover
- **AnchorName**, **PositionArea**, **PositionTryOption** - Validated anchor names, areas and fallbacks

```razor
@* Nested: follows the enclosing target. *@
<AnchorTarget>
    <img src="avatar.png" alt="Avatar" />
    <AnchorFollower Area="PositionArea.TopRight" Fallbacks="[]">3</AnchorFollower>
</AnchorTarget>

@* Named: the target and the follower can be anywhere; any element with anchor-name works as a target. *@
<input style="anchor-name: @_search" />
<AnchorFollower Anchor="_search" MatchAnchorWidth="true" style="margin-block: 4px">Suggestions</AnchorFollower>

@* Implicit: a popover without Anchor follows the button that opened it. *@
<button popovertarget="info">Info</button>
<AnchorFollower id="info" PopoverMode="AnchorPopoverMode.Auto" Area="PositionArea.Right">Details</AnchorFollower>

@code {
    private readonly AnchorName _search = AnchorName.New();
}
```

- By default the follower is placed below the anchor with the left edges aligned. When it does not fit, it flips above, aligns the right edges, or both; as a last resort it is centered above or below the anchor with its height limited to the available space (add `overflow: auto` to scroll). `Fallbacks="[]"` disables the fallbacks.
- Use `margin` for the gap to the anchor; it is mirrored when the position flips.
- `Visibility` hides the follower while its anchor is clipped by a scroll container (`AnchorsVisible`, the default) or while the follower overflows (`NoOverflow`).
- Use `PopoverMode` (or a plain `popover` attribute) to render the follower in the top layer, so it is not clipped by `overflow` or covered by `z-index`.
- The base styles are in the `rcl-anchor` cascade layer, so application styles override them. Browsers without anchor positioning show popovers in the middle of the viewport.
- See `samples/RazorSample` for a Microsoft Fluent UI Blazor sample app, including an interactive `/anchor` demo.

### Draggable Components (Blazor)

Components in `RazorClassLibrary.Draggable` that let the user move content with a mouse, touch or pen
(interactive render modes only):

- **Draggable** - Moves its content with the CSS `translate` property, so the surrounding layout does not shift; `@bind-Offset` gets or sets the position
- **DraggableContainer** - Keeps the draggables inside it within its padding box
- **DraggableHandle** - Restricts dragging to a part of the draggable, e.g. a title bar

```razor
<DraggableContainer style="position: relative; height: 300px">
    <Draggable @bind-Offset="_offset">Drag me anywhere in the container</Draggable>

    <Draggable Bounds="DraggableBounds.Parent">
        <DraggableHandle>Title bar</DraggableHandle>
        <p>Only the title bar drags this one.</p>
    </Draggable>
</DraggableContainer>

@code {
    private DragOffset _offset;
}
```

- `Bounds` is `Container` (the enclosing `DraggableContainer`; unbounded without one), `Parent`, `Viewport` or `None`. When the bounds are resized, the draggable is moved back inside.
- Form controls, buttons and links inside a draggable keep working, and a drag does not click the element.
- `Disabled` stops dragging; the base styles (`cursor`, `touch-action`) are in the `rcl-draggable` cascade layer.
- See the `/draggable` demo in `samples/RazorSample`.

### DraggableDialog (Blazor)

`RazorClassLibrary.Dialog.DraggableDialog` is a non-modal native dialog styled with Fluent 2 tokens. It opens in the
browser's top layer, so it is not clipped or covered by ancestor stacking contexts, and the rest of the page remains
interactive. The dialog opens centered in the viewport and can be moved by dragging its header.

```razor
<button @onclick="() => _dialog!.OpenAsync()">Open dialog</button>

<DraggableDialog @ref="_dialog" Title="Confirm changes"
                 OnConfirm="@SaveAsync">
    <p>Do you want to save these changes?</p>
</DraggableDialog>

@code {
    private DraggableDialog? _dialog;

    private Task SaveAsync()
    {
        // Save changes.
        return Task.CompletedTask;
    }
}
```

- `OpenAsync` opens the dialog; the default confirm and cancel buttons call `ConfirmAsync` and `CancelAsync`.
- `OnOpening`, `OnConfirming` and `OnCanceling` are optional guards. Return `false` to prevent the corresponding open or close action.
- `OnOpen`, `OnConfirm` and `OnCancel` run after their corresponding action succeeds.
- `HeaderContent` replaces `Title`, and `FooterContent` replaces the default buttons. Custom footer buttons can call `ConfirmAsync` or `CancelAsync`.
- `ChildContent` provides the dialog body; `Width`, `Class` and `Style` customize its appearance.
- Requires an interactive render mode. See the `/dialog` demo in `samples/RazorSample`.
