import { useId, useState, type ReactNode } from "react";
import { SidePanelContext } from "../../../contexts/layout/SidePanelContext";

type SidePanelProps = {
  children: ReactNode;
  title?: string;
  showTitle?: boolean;
  side?: "left" | "right";
  defaultOpen?: boolean;
  open?: boolean;
  onOpenChange?: (open: boolean) => void;
  /** Expanded width in px. */
  width?: number;
  /** Collapsed width in px. 48 fits a 24px icon with the item padding used here. */
  collapsedWidth?: number;
};

export function SidePanel({
  children,
  title = "Side panel",
  showTitle = true,
  side = "left",
  defaultOpen = true,
  open,
  onOpenChange,
  width = 280,
  collapsedWidth = 48,
}: SidePanelProps) {
  const contentId = useId();
  const titleId = useId();
  const [internalOpen, setInternalOpen] = useState(defaultOpen);

  const isControlled = open !== undefined;
  const isOpen = isControlled ? open : internalOpen;

  const toggle = () => {
    const next = !isOpen;
    if (!isControlled) setInternalOpen(next);
    onOpenChange?.(next);
  };

  const flipChevron = (side === "left") !== isOpen;

  const togglePosition = !isOpen
    ? "left-1/2 -translate-x-1/2"
    : side === "left"
      ? "right-2"
      : "left-2";

  return (
    <SidePanelContext.Provider value={{ isOpen, side }}>
      <aside
        aria-label={showTitle ? title : undefined}
        aria-labelledby={showTitle ? undefined : title}
        style={{ width: isOpen ? width : collapsedWidth }}
        className={[
          "relative shrink-0 overflow-hidden border-gray-200 bg-white",
          "transition-[width] duration-200 ease-in-out motion-reduce:transition-none",
          side === "left" ? "border-r" : "border-l",
        ].join(" ")}
      >
        {showTitle && (
          <h2
            id={titleId}
            // Width leaves room for the toggle button so long titles truncate instead of overlapping it
            style={{ width: width - 64 }}
            className={[
              "absolute top-2 h-8 truncate text-sm font-semibold leading-8 text-gray-900",
              side === "left" ? "left-4" : "left-12",
              "transition-opacity duration-200 motion-reduce:transition-none",
              isOpen ? "opacity-100 delay-75" : "pointer-events-none opacity-0",
            ].join(" ")}
          >
            {title}
          </h2>
        )}
        <button
          type="button"
          onClick={toggle}
          aria-expanded={isOpen}
          aria-controls={contentId}
          aria-label={isOpen ? `Collapse ${title}` : `Expand ${title}`}
          className={[
            "absolute top-2 z-10 grid h-8 w-8 place-items-center rounded-md",
            "text-gray-700 hover:bg-gray-100",
            "focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600",
            togglePosition,
          ].join(" ")}
        >
          <svg
            viewBox="0 0 24 24"
            width="20"
            height="20"
            aria-hidden="true"
            className={[
              "transition-transform duration-200 motion-reduce:transition-none",
              flipChevron ? "rotate-180" : "",
            ].join(" ")}
          >
            <path
              d="M15 6l-6 6 6 6"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
            />
          </svg>
        </button>

        {/* Fixed-width inner box so labels don't reflow while the panel animates.
            Stays visible when collapsed so the icons remain clickable. */}
        <div
          id={contentId}
          style={{ width }}
          className="box-border h-full overflow-y-auto px-2 pb-4 pt-12"
        >
          {children}
        </div>
      </aside>
    </SidePanelContext.Provider>
  );
}