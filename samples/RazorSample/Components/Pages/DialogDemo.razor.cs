using RazorClassLibrary.Dialog;

namespace RazorSample.Components.Pages;

public partial class DialogDemo
{
    private const int MaxLogEntries = 8;

    private readonly List<string> _log = [];

    private DraggableDialog? _basic;
    private DraggableDialog? _guarded;
    private DraggableDialog? _custom;

    private string _projectName = "dotnet-utilities";
    private string? _draftName;
    private string? _nameError;
    private bool _hasUnsavedChanges;

    private void Log(string message)
    {
        _log.Insert(0, $"{DateTime.Now:HH:mm:ss}  {message}");
        if (_log.Count > MaxLogEntries)
            _log.RemoveAt(_log.Count - 1);
    }

    private ValueTask<bool> OnRenameOpeningAsync()
    {
        _draftName = _projectName;
        _nameError = null;
        _hasUnsavedChanges = false;
        Log("Guards: OnOpening -> true");
        return ValueTask.FromResult(true);
    }

    private ValueTask<bool> OnRenameConfirmingAsync()
    {
        _nameError = string.IsNullOrWhiteSpace(_draftName) ? "Enter a project name." : null;
        var canClose = _nameError is null;
        Log($"Guards: OnConfirming -> {canClose.ToString().ToLowerInvariant()}");
        StateHasChanged();
        return ValueTask.FromResult(canClose);
    }

    private void OnRenameConfirm()
    {
        _projectName = _draftName!.Trim();
        Log($"Guards: OnConfirm, renamed to \"{_projectName}\"");
    }

    private ValueTask<bool> OnRenameCancelingAsync()
    {
        var canClose = !_hasUnsavedChanges;
        Log($"Guards: OnCanceling -> {canClose.ToString().ToLowerInvariant()}");
        StateHasChanged();
        return ValueTask.FromResult(canClose);
    }
}
