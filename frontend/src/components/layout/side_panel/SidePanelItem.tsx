import type { ComponentPropsWithoutRef, ElementType, ReactNode } from "react";
import { useSidePanel } from "../../../contexts/layout/SidePanelContext";

type SidePanelItemProps<T extends ElementType> = {
  icon: ReactNode;
  label: string;
  active?: boolean;
  /** Render as another element or component, e.g. react-router's NavLink. Defaults to "a". */
  as?: T;
} & Omit<ComponentPropsWithoutRef<T>, "as" | "children" | "className" | "title">;

export function SidePanelItem<T extends ElementType = "a">({
  icon,
  label,
  active = false,
  as,
  ...rest
}: SidePanelItemProps<T>) {
  const { isOpen } = useSidePanel();
  const Component: ElementType = as ?? "a";

  return (
    <Component
      {...rest}
      // Native tooltip while collapsed, since the label is hidden
      title={isOpen ? undefined : label}
      aria-current={active ? "page" : undefined}
      className={[
        "flex items-center overflow-hidden rounded-md p-1",
        "transition-[width] duration-200 ease-in-out motion-reduce:transition-none",
        "focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-inset focus-visible:ring-blue-600",
        // Collapsed: a 32px square that lines up with the toggle button
        isOpen ? "w-full" : "w-8",
        active
          ? "bg-blue-50 text-blue-700"
          : "text-gray-700 hover:bg-gray-100",
      ].join(" ")}
    >
      <span aria-hidden="true" className="grid h-6 w-6 shrink-0 place-items-center">
        {icon}
      </span>

      {/* Stays in the DOM when collapsed, so screen readers still get the link name */}
      <span
        className={[
          "ml-3 whitespace-nowrap text-sm font-medium",
          "transition-opacity duration-200 motion-reduce:transition-none",
          isOpen ? "opacity-100 delay-75" : "opacity-0",
        ].join(" ")}
      >
        {label}
      </span>
    </Component>
  );
}