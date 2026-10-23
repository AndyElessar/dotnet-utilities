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
