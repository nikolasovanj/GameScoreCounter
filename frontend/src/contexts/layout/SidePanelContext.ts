import { createContext, useContext } from "react";

type SidePanelContextValue = { isOpen: boolean; side: "left" | "right" };

export const SidePanelContext = createContext<SidePanelContextValue | null>(null);

export function useSidePanel() {
  const ctx = useContext(SidePanelContext);
  if (!ctx) throw new Error("useSidePanel must be used inside <SidePanel>");
  return ctx;
}