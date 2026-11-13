
import type { ReactNode } from "react";
import { SidePanel } from "./SidePanel";
import { SidePanelItem } from "./SidePanelItem";

const icon = {
  viewBox: "0 0 24 24",
  fill: "none",
  stroke: "currentColor",
  strokeWidth: 2,
  strokeLinecap: "round",
  strokeLinejoin: "round",
  className: "h-6 w-6",
} as const;

const MatchesIcon = () => (
  <svg {...icon}>
    <path d="M8 6h13M8 12h13M8 18h13M3 6h.01M3 12h.01M3 18h.01" />
  </svg>
);
const ScheduleIcon = () => (
  <svg {...icon}>
    <rect x="3" y="4" width="18" height="18" rx="2" />
    <path d="M16 2v4M8 2v4M3 10h18" />
  </svg>
);
const FriendsIcon = () => (
  <svg {...icon}>
    <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
    <circle cx="9" cy="7" r="4" />
    <path d="M23 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75" />
  </svg>
);
const LoginIcon = () => (
  <svg {...icon}>
    <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
    <circle cx="9" cy="7" r="4" />
  </svg>
);

export default function TestPage({children}: {children: ReactNode}){
  const path = window.location.pathname;

  return (
    <div className="flex h-screen">
      <SidePanel title="Navigation">
        <nav className="flex flex-col gap-1">
          <SidePanelItem href="/matches" icon={<MatchesIcon />} label="Matches" active={path === "/matches"} />
          <SidePanelItem href="/schedule" icon={<ScheduleIcon />} label="Schedule" active={path === "/schedule"} />
          <SidePanelItem href="/friends" icon={<FriendsIcon />} label="Friends" active={path === "/friends"} />
          <SidePanelItem href="/login" icon={<LoginIcon />} label="Sign in" active={path === "/login"} />
        </nav>
      </SidePanel>

      <main className="flex-1 p-6">{children}</main>
    </div>
  );
}