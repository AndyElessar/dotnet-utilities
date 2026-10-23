// Pressing these starts no drag, so form controls and links inside a draggable keep working.
const interactiveSelector =
    "input, textarea, select, button, a[href], label, summary, [contenteditable]:not([contenteditable='false'])";

// Movement in CSS pixels before a press becomes a drag, so a plain click is not swallowed.
const dragThreshold = 3;

/**
 * Restricts a value to the inclusive range [min, max].
 * @param {number} value The value to restrict.
 * @param {number} min The lower bound.
 * @param {number} max The upper bound.
 * @returns {number} The clamped value.
 */
function clamp(value, min, max) {
    return Math.min(Math.max(value, min), max);
}

/**
 * Reads the current translate of an element in pixels; the component only writes pixel values.
 * @param {HTMLElement} element The draggable element.
 * @returns {[number, number]} The x and y offsets.
 */
function readOffset(element) {
    const value = getComputedStyle(element).translate;
    if (!value || value === "none") return [0, 0];

    const [x = "0", y = "0"] = value.split(" ");
    return [Number.parseFloat(x) || 0, Number.parseFloat(y) || 0];
}

/**
 * Gets the visible padding box of an element in viewport coordinates.
 * @param {Element | null} element The bounding element.
 * @returns {{ left: number, top: number, right: number, bottom: number } | null} The box, or null without an element.
 */
function paddingBox(element) {
    if (!element) return null;

    const rect = element.getBoundingClientRect();
    const left = rect.left + element.clientLeft;
    const top = rect.top + element.clientTop;
    return { left, top, right: left + element.clientWidth, bottom: top + element.clientHeight };
}

/**
 * Makes an element draggable with pointer input (mouse, touch and pen) by updating its CSS translate.
 * @param {HTMLElement} element The element to move; it must have the data-draggable attribute.
 * @param {{ invokeMethodAsync: (name: string, ...args: unknown[]) => Promise<unknown> }} dotNetRef Receives
 * OnMoved(x, y) after a drag ends or the element is moved back inside its bounds.
 * @param {"container" | "parent" | "viewport" | "none"} bounds The area the element is kept inside of.
 * @param {HTMLElement | null} container The bounding element when bounds is "container"; null for no bounds.
 * @param {boolean} handles Whether only descendants with data-draggable-handle start a drag.
 * @param {boolean} disabled Whether dragging is disabled.
 * @returns {{ update: Function, dispose: () => void }} An object to update the options and to remove the listeners.
 */
export function createDraggable(element, dotNetRef, bounds, container, handles, disabled) {
    const options = { bounds, container, handles, disabled };
    const controller = new AbortController();
    const { signal } = controller;
    const resizeObserver = new ResizeObserver(() => fit());
    let drag = null;

    const isOwn = node => node.closest("[data-draggable]") === element;

    function getBounds() {
        switch (options.bounds) {
            case "viewport": {
                const root = document.documentElement;
                return { left: 0, top: 0, right: root.clientWidth, bottom: root.clientHeight };
            }
            case "parent":
                return paddingBox(element.parentElement);
            case "container":
                return paddingBox(options.container);
            default:
                return null;
        }
    }

    function moveTo(x, y) {
        element.style.translate = `${x}px ${y}px`;
    }

    function notify() {
        const [x, y] = readOffset(element);
        dotNetRef.invokeMethodAsync("OnMoved", x, y).catch(() => { /* The circuit is gone. */ });
    }

    // Moves the element back inside its bounds, e.g. after the container or the viewport shrank. An element larger
    // than its bounds is aligned to their top-left corner.
    function fit() {
        if (drag) return;
        const area = getBounds();
        if (!area) return;

        const rect = element.getBoundingClientRect();
        const dx = rect.width > area.right - area.left || rect.left < area.left
            ? area.left - rect.left
            : Math.min(0, area.right - rect.right);
        const dy = rect.height > area.bottom - area.top || rect.top < area.top
            ? area.top - rect.top
            : Math.min(0, area.bottom - rect.bottom);
        if (Math.abs(dx) < 0.5 && Math.abs(dy) < 0.5) return;

        const [x, y] = readOffset(element);
        moveTo(x + dx, y + dy);
        notify();
    }

    function observe() {
        resizeObserver.disconnect();
        resizeObserver.observe(element);
        const boundsElement = options.bounds === "parent" ? element.parentElement
            : options.bounds === "container" ? options.container
                : null;
        if (boundsElement) resizeObserver.observe(boundsElement);
    }

    element.addEventListener("pointerdown", e => {
        if (options.disabled || drag || !e.isPrimary || e.button !== 0) return;

        const target = e.target;
        if (!(target instanceof Element) || !isOwn(target)) return;

        const handle = target.closest("[data-draggable-handle]");
        if (options.handles && !(handle && isOwn(handle))) return;

        const interactive = target.closest(interactiveSelector);
        if (interactive && interactive !== element && interactive !== handle && element.contains(interactive)) return;

        const [x, y] = readOffset(element);
        const rect = element.getBoundingClientRect();
        const area = getBounds();
        // An element that is already partly outside its bounds can still move, just not further out.
        drag = {
            pointerId: e.pointerId,
            startX: e.clientX,
            startY: e.clientY,
            x,
            y,
            moved: false,
            minX: area ? Math.min(0, area.left - rect.left) : -Infinity,
            maxX: area ? Math.max(0, area.right - rect.right) : Infinity,
            minY: area ? Math.min(0, area.top - rect.top) : -Infinity,
            maxY: area ? Math.max(0, area.bottom - rect.bottom) : Infinity,
        };
        element.setPointerCapture(e.pointerId);
        e.preventDefault();
    }, { signal });

    element.addEventListener("pointermove", e => {
        if (drag?.pointerId !== e.pointerId) return;

        const rawX = e.clientX - drag.startX;
        const rawY = e.clientY - drag.startY;
        if (!drag.moved) {
            if (Math.hypot(rawX, rawY) < dragThreshold) return;
            drag.moved = true;
            element.setAttribute("data-dragging", "");
        }

        moveTo(drag.x + clamp(rawX, drag.minX, drag.maxX), drag.y + clamp(rawY, drag.minY, drag.maxY));
    }, { signal });

    // Fires after pointerup and pointercancel alike.
    element.addEventListener("lostpointercapture", e => {
        if (drag?.pointerId !== e.pointerId) return;

        const { moved } = drag;
        drag = null;
        if (!moved) return;

        element.removeAttribute("data-dragging");

        // Swallow the click that follows the drag, so dragging a clickable element does not activate it.
        const swallowClick = ev => {
            ev.stopPropagation();
            ev.preventDefault();
        };
        element.addEventListener("click", swallowClick, { capture: true, once: true, signal });
        setTimeout(() => element.removeEventListener("click", swallowClick, { capture: true }));

        notify();
    }, { signal });

    globalThis.addEventListener("resize", () => {
        if (options.bounds === "viewport") fit();
    }, { signal });

    observe();

    return {
        /**
         * Replaces the options; see {@link createDraggable}.
         */
        update(newBounds, newContainer, newHandles, newDisabled) {
            Object.assign(options, { bounds: newBounds, container: newContainer, handles: newHandles, disabled: newDisabled });
            observe();
            fit();
        },

        /**
         * Removes the listeners and stops observing resizes.
         */
        dispose() {
            controller.abort();
            resizeObserver.disconnect();
        },
    };
}