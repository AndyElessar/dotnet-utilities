/**
 * Makes a dialog draggable by a handle. Pointer capture keeps the drag active even when the pointer leaves the handle;
 * CSS positions and clamps the dialog inside the viewport.
 * @param {HTMLDialogElement} dialog The dialog to move.
 * @param {HTMLElement} handle The element that starts a drag, e.g. the dialog header.
 * @returns {{ dispose: () => void } | null} A handle whose dispose() removes the listeners, or null when either
 * element is missing.
 */
export function enableDrag(dialog, handle) {
    if (!dialog || !handle) return null;

    const controller = new AbortController();
    const { signal } = controller;
    let startX = 0;
    let startY = 0;

    handle.addEventListener("pointerdown", e => {
        if (!e.isPrimary || e.button !== 0) return;

        const rect = dialog.getBoundingClientRect();
        startX = e.clientX - rect.left;
        startY = e.clientY - rect.top;
        handle.setPointerCapture(e.pointerId);
    }, { signal });

    handle.addEventListener("pointermove", e => {
        if (!handle.hasPointerCapture(e.pointerId)) return;

        dialog.style.setProperty("--x", `${e.clientX - startX}px`);
        dialog.style.setProperty("--y", `${e.clientY - startY}px`);
    }, { signal });

    return {
        dispose: () => controller.abort(),
    };
}

/**
 * Removes the listeners added by {@link enableDrag}.
 * @param {{ dispose: () => void } | null} dragInstance The value returned by enableDrag; null is ignored.
 */
export function disableDrag(dragInstance) {
    dragInstance?.dispose();
}

/**
 * Opens a `popover="manual"` dialog in the top layer, so no z-index is needed and ancestors cannot clip or
 * cover it. It stays non-modal. Clearing the previous drag offsets lets CSS center it again.
 * @param {HTMLDialogElement} dialog The dialog to open.
 */
export function showDialog(dialog) {
    if (!dialog || dialog.matches(":popover-open")) return;

    dialog.style.removeProperty("--x");
    dialog.style.removeProperty("--y");
    dialog.showPopover();
}

/**
 * Closes an open dialog; the browser returns focus to the element that had it before the dialog opened.
 * @param {HTMLDialogElement} dialog The dialog to close.
 */
export function closeDialog(dialog) {
    if (!dialog?.matches(":popover-open")) return;

    dialog.hidePopover();
}
