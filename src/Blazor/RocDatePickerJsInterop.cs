using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor;

public sealed class RocDatePickerJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public RocDatePickerJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Blazor/rocDatePicker.js").AsTask());
    }

    public async ValueTask OpenPickerAsync(ElementReference inputElement)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("openPicker", inputElement);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
