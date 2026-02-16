export function openPicker(inputElement) {
    if (inputElement && typeof inputElement.showPicker === "function") {
        inputElement.showPicker();
    } else if (inputElement && typeof inputElement.focus === "function") {
        inputElement.focus();
    }
}
