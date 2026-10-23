using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace RazorClassLibrary.Dialog;

/// <summary>
/// A non-modal dialog in the Fluent 2 style that the user can move by dragging its header.
/// </summary>
/// <remarks>
/// <para>
/// The dialog is a native <c>&lt;dialog popover="manual"&gt;</c>. It opens in the browser's top layer, so it is
/// drawn above the page without a <c>z-index</c> and cannot be clipped or covered by its ancestors, and it is
/// non-modal, so the rest of the page stays interactive.
/// It is centered in the viewport every time it opens and is kept inside the viewport while being dragged.
/// </para>
/// <para>
/// Open it with <see cref="OpenAsync"/>; the built-in buttons call <see cref="ConfirmAsync"/> and
/// <see cref="CancelAsync"/>, which can also be called from custom <see cref="FooterContent"/>.
/// Requires an interactive render mode.
/// </para>
/// </remarks>
public partial class DraggableDialog
{
    private const string ModulePath = "./_content/RazorClassLibrary/Dialog/DraggableDialog.razor.js";

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private Task<IJSObjectReference>? moduleTask;

    private Task<IJSObjectReference> GetModuleAsync() =>
        moduleTask ??= JS.InvokeAsync<IJSObjectReference>("import", ModulePath).AsTask();

    /// <summary>
    /// Gets or sets the title shown in the header. Ignored when <see cref="HeaderContent"/> is set.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets custom header content that replaces <see cref="Title"/>. The header is the drag handle;
    /// the close button is always rendered next to it.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// Gets or sets the body of the dialog.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the text of the default confirm button. Defaults to <c>"Confirm"</c>.
    /// </summary>
    [Parameter]
    public string ConfirmText { get; set; } = "Confirm";

    /// <summary>
    /// Gets or sets the text of the default cancel button, also used as the accessible label of the close button.
    /// Defaults to <c>"Cancel"</c>.
    /// </summary>
    [Parameter]
    public string CancelText { get; set; } = "Cancel";

    /// <summary>
    /// Gets or sets custom footer content that replaces the default confirm and cancel buttons.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    /// <summary>
    /// Gets or sets whether <see cref="Class"/> replaces the built-in <c>dragable-dialog</c> class instead of being
    /// appended to it. Replacing it removes all built-in styles of the dialog surface.
    /// </summary>
    [Parameter]
    public bool OverrideClass { get; set; } = false;

    /// <summary>
    /// Gets or sets additional CSS classes for the <c>&lt;dialog&gt;</c> element.
    /// </summary>
    [Parameter]
    public string Class { get; set; } = string.Empty;
    private string ClassValue =>
        OverrideClass ? Class : $"dragable-dialog " + Class;

    /// <summary>
    /// Gets or sets whether <see cref="Style"/> replaces the generated inline style instead of being appended to it.
    /// When set, <see cref="Width"/> is ignored.
    /// </summary>
    [Parameter]
    public bool OverrideStyle { get; set; } = false;

    /// <summary>
    /// Gets or sets additional inline styles for the <c>&lt;dialog&gt;</c> element.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;
    private string StyleValue =>
        OverrideStyle || string.IsNullOrWhiteSpace(Width) ? Style : $"width:{Width}; {Style}";

    /// <summary>
    /// Gets or sets the CSS width of the dialog, e.g. <c>"600px"</c> or <c>"40rem"</c>. Defaults to <c>"600px"</c>;
    /// an empty value lets the content decide the width.
    /// </summary>
    [Parameter]
    public string Width { get; set; } = "600px";

    private async ValueTask Show()
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("showDialog", dialogEle);
    }

    private async ValueTask Hide()
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("closeDialog", dialogEle);
    }

    private ElementReference dialogEle;
    private ElementReference headerEle;
    private IJSObjectReference? dragHandle = null;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && dragHandle is null)
        {
            var module = await GetModuleAsync();
            dragHandle = await module.InvokeAsync<IJSObjectReference?>("enableDrag", dialogEle, headerEle);
        }
    }

    /// <summary>
    /// Gets or sets a guard invoked by <see cref="OpenAsync"/> before the dialog opens. Return <see langword="false"/>
    /// to keep it closed.
    /// </summary>
    [Parameter]
    public Func<ValueTask<bool>>? OnOpening { get; set; }

    /// <summary>
    /// Gets or sets a callback invoked after the dialog has opened.
    /// </summary>
    [Parameter]
    public EventCallback OnOpen { get; set; }

    /// <summary>
    /// Gets or sets a guard invoked by <see cref="ConfirmAsync"/> before the dialog closes, e.g. to validate input.
    /// Return <see langword="false"/> to keep it open.
    /// </summary>
    [Parameter]
    public Func<ValueTask<bool>>? OnConfirming { get; set; }

    /// <summary>
    /// Gets or sets a callback invoked after the dialog has been closed by <see cref="ConfirmAsync"/>.
    /// </summary>
    [Parameter]
    public EventCallback OnConfirm { get; set; }

    /// <summary>
    /// Gets or sets a guard invoked by <see cref="CancelAsync"/> (the cancel and close buttons) before the dialog
    /// closes, e.g. to ask about unsaved changes. Return <see langword="false"/> to keep it open.
    /// </summary>
    [Parameter]
    public Func<ValueTask<bool>>? OnCanceling { get; set; }

    /// <summary>
    /// Gets or sets a callback invoked after the dialog has been closed by <see cref="CancelAsync"/>.
    /// </summary>
    [Parameter]
    public EventCallback OnCancel { get; set; }

    /// <summary>
    /// Opens the dialog centered in the viewport, unless <see cref="OnOpening"/> returns <see langword="false"/>,
    /// then invokes <see cref="OnOpen"/>.
    /// </summary>
    public async Task OpenAsync()
    {
        bool canOpen = true;
        if (OnOpening is not null)
            canOpen = await OnOpening();

        if (!canOpen)
            return;

        await Show();

        if (OnOpen.HasDelegate)
            await OnOpen.InvokeAsync();
    }

    /// <summary>
    /// Closes the dialog, unless <see cref="OnConfirming"/> returns <see langword="false"/>, then invokes
    /// <see cref="OnConfirm"/>.
    /// </summary>
    public async Task ConfirmAsync()
    {
        bool canConfirm = true;
        if (OnConfirming is not null)
            canConfirm = await OnConfirming();

        if (!canConfirm)
            return;

        await Hide();

        if (OnConfirm.HasDelegate)
            await OnConfirm.InvokeAsync();
    }

    /// <summary>
    /// Closes the dialog, unless <see cref="OnCanceling"/> returns <see langword="false"/>, then invokes
    /// <see cref="OnCancel"/>.
    /// </summary>
    public async Task CancelAsync()
    {
        bool canCancel = true;
        if (OnCanceling is not null)
            canCancel = await OnCanceling();

        if (!canCancel)
            return;

        await Hide();

        if (OnCancel.HasDelegate)
            await OnCancel.InvokeAsync();
    }

    private bool disposedValue;

    /// <summary>
    /// Detaches the drag handlers and releases the JavaScript module.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (moduleTask is not null)
                {
                    var module = await moduleTask;
                    if (dragHandle is not null)
                    {
                        await module.InvokeVoidAsync("disableDrag", dragHandle);
                        await dragHandle.DisposeAsync();
                    }

                    await module.DisposeAsync();
                }
            }
        }
        catch (Exception e) when (e is JSDisconnectedException or OperationCanceledException)
        {
            //Ignore JSDisconnectedException and OperationCanceledException
        }
        GC.SuppressFinalize(this);
    }
}
