import type { ReactNode } from "react";

type LandingPageProps = {
  loginHref?: string;
  registerHref?: string;
};

// ---- Placeholder content: replace with real copy later ----
const APP_NAME = "Game Score Counter";

const HERO = {
  eyebrow: "Placeholder tagline",
  title: "Headline goes here, short and punchy",
  text: "This is temporary text. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
};

const iconProps = {
  viewBox: "0 0 24 24",
  fill: "none",
  stroke: "currentColor",
  strokeWidth: 2,
  strokeLinecap: "round",
  strokeLinejoin: "round",
  className: "h-6 w-6",
  "aria-hidden": true,
} as const;

const FEATURES: { title: string; text: string; icon: ReactNode }[] = [
  {
    title: "Feature one",
    text: "Placeholder description. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip.",
    icon: (
      <svg {...iconProps}>
        <path d="M8 6h13M8 12h13M8 18h13M3 6h.01M3 12h.01M3 18h.01" />
      </svg>
    ),
  },
  {
    title: "Feature two",
    text: "Placeholder description. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore.",
    icon: (
      <svg {...iconProps}>
        <rect x="3" y="4" width="18" height="18" rx="2" />
        <path d="M16 2v4M8 2v4M3 10h18" />
      </svg>
    ),
  },
  {
    title: "Feature three",
    text: "Placeholder description. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia.",
    icon: (
      <svg {...iconProps}>
        <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
        <circle cx="9" cy="7" r="4" />
        <path d="M23 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75" />
      </svg>
    ),
  },
];

const STEPS = [
  { title: "Step one", text: "Placeholder text. Sed ut perspiciatis unde omnis iste natus error sit voluptatem." },
  { title: "Step two", text: "Placeholder text. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit." },
  { title: "Step three", text: "Placeholder text. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet." },
];

const SAMPLE_ROWS = [
  { name: "Player One", score: 42, winner: true , position: 1},
  { name: "Player Two", score: 37, winner: false , position: 2},
  { name: "Player Three", score: 29, winner: false ,position: 3},
];

// ---- Shared styles ----
const focusRing =
  "focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-600 focus-visible:ring-offset-2";
const primaryButton =
  `inline-flex items-center justify-center rounded-md bg-blue-600 px-5 py-2.5 text-sm font-semibold text-white shadow-sm hover:bg-blue-700 ${focusRing}`;
const secondaryButton =
  `inline-flex items-center justify-center rounded-md border border-gray-300 bg-white px-5 py-2.5 text-sm font-semibold text-gray-900 shadow-sm hover:bg-gray-50 ${focusRing}`;
const container = "mx-auto w-full max-w-6xl px-4 sm:px-6";

export function LandingPage({
  loginHref = "/login",
  registerHref = "/register",
}: LandingPageProps) {
  return (
    <div className="min-h-screen bg-white text-gray-900">
      <a
        href="#main"
        className="sr-only focus:not-sr-only focus:fixed focus:left-4 focus:top-4 focus:z-50 focus:rounded-md focus:bg-white focus:px-4 focus:py-2 focus:text-sm focus:font-medium focus:shadow-lg"
      >
        Skip to content
      </a>

      {/* Header */}
      <header className="border-b border-gray-200 bg-white">
        <div className={`${container} flex h-16 items-center justify-between`}>
          <a href="/" className={`rounded-md text-lg font-bold tracking-tight ${focusRing}`}>
            {APP_NAME}
          </a>
          <nav aria-label="Account" className="flex items-center gap-2">
            <a
              href={loginHref}
              className={`rounded-md px-3 py-2 text-sm font-medium text-gray-700 hover:bg-gray-100 ${focusRing}`}
            >
              Sign in
            </a>
            <a href={registerHref} className={primaryButton}>
              Register
            </a>
          </nav>
        </div>
      </header>

      <main id="main">
        {/* Hero */}
        <section aria-labelledby="hero-heading" className="bg-blue-50">
          <div className={`${container} grid items-center gap-12 py-16 md:grid-cols-2 md:py-24`}>
            <div>
              <p className="text-sm font-semibold uppercase tracking-wide text-blue-700">
                {HERO.eyebrow}
              </p>
              <h1
                id="hero-heading"
                className="mt-3 text-4xl font-bold tracking-tight sm:text-5xl"
              >
                {HERO.title}
              </h1>
              <p className="mt-5 max-w-xl text-lg text-gray-600">{HERO.text}</p>
              <div className="mt-8 flex flex-wrap gap-3">
                <a href={registerHref} className={primaryButton}>
                  Get started
                </a>
                <a href={loginHref} className={secondaryButton}>
                  Sign in
                </a>
              </div>
            </div>

            {/* Decorative mock scoreboard; hidden from assistive tech since it's placeholder art */}
            <div aria-hidden="true" className="mx-auto w-full max-w-md">
              <div className="rounded-xl border border-gray-200 bg-white p-5 shadow-lg">
                <div className="flex items-center justify-between">
                  <span className="text-sm font-semibold">Game name</span>
                  <span className="rounded-full bg-gray-100 px-2.5 py-0.5 text-xs font-medium text-gray-600">
                    Round 3
                  </span>
                </div>
                <ul className="mt-4 divide-y divide-gray-100">
                  {SAMPLE_ROWS.map((row) => (
                    <li key={row.name} className="flex items-center justify-between py-3">
                      <span className="flex items-center gap-3">
                        <span className="grid h-8 w-8 place-items-center rounded-full bg-gray-100 text-xs font-semibold text-gray-600">
                          {row.position}
                        </span>
                        <span className="text-sm font-medium">{row.name}</span>
                        {row.winner && (
                          <span className="rounded-full bg-green-100 px-2 py-0.5 text-xs font-medium text-green-700">
                            Leading
                          </span>
                        )}
                      </span>
                      <span className="text-lg font-bold tabular-nums">{row.score}</span>
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          </div>
        </section>

        {/* Features */}
        <section aria-labelledby="features-heading" className="py-16 md:py-24">
          <div className={container}>
            <div className="mx-auto max-w-2xl text-center">
              <h2 id="features-heading" className="text-3xl font-bold tracking-tight">
                Section heading goes here
              </h2>
              <p className="mt-4 text-gray-600">
                Placeholder intro text. Lorem ipsum dolor sit amet, consectetur adipiscing elit.
              </p>
            </div>

            <ul className="mt-12 grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
              {FEATURES.map((feature) => (
                <li
                  key={feature.title}
                  className="rounded-xl border border-gray-200 bg-white p-6 shadow-sm"
                >
                  <span className="grid h-11 w-11 place-items-center rounded-lg bg-blue-50 text-blue-700">
                    {feature.icon}
                  </span>
                  <h3 className="mt-4 text-lg font-semibold">{feature.title}</h3>
                  <p className="mt-2 text-sm leading-6 text-gray-600">{feature.text}</p>
                </li>
              ))}
            </ul>
          </div>
        </section>

        {/* How it works */}
        <section aria-labelledby="steps-heading" className="bg-gray-50 py-16 md:py-24">
          <div className={container}>
            <h2
              id="steps-heading"
              className="text-center text-3xl font-bold tracking-tight"
            >
              How it works
            </h2>

            <ol className="mt-12 grid gap-8 md:grid-cols-3">
              {STEPS.map((step, i) => (
                <li key={step.title} className="flex gap-4">
                  <span
                    aria-hidden="true"
                    className="grid h-9 w-9 shrink-0 place-items-center rounded-full bg-blue-600 text-sm font-bold text-white"
                  >
                    {i + 1}
                  </span>
                  <div>
                    <h3 className="font-semibold">{step.title}</h3>
                    <p className="mt-1 text-sm leading-6 text-gray-600">{step.text}</p>
                  </div>
                </li>
              ))}
            </ol>
          </div>
        </section>

        {/* Call to action */}
        <section aria-labelledby="cta-heading" className="py-16 md:py-24">
          <div className={container}>
            <div className="rounded-2xl bg-blue-600 px-6 py-12 text-center text-white sm:px-12">
              <h2 id="cta-heading" className="text-3xl font-bold tracking-tight">
                Call to action goes here
              </h2>
              <p className="mx-auto mt-4 max-w-xl text-blue-100">
                Placeholder text. Lorem ipsum dolor sit amet, consectetur adipiscing elit.
              </p>
              <a
                href={registerHref}
                className="mt-8 inline-flex items-center justify-center rounded-md bg-white px-5 py-2.5 text-sm font-semibold text-blue-700 shadow-sm hover:bg-blue-50 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-white focus-visible:ring-offset-2 focus-visible:ring-offset-blue-600"
              >
                Create a free account
              </a>
            </div>
          </div>
        </section>
      </main>

      {/* Footer */}
      <footer className="border-t border-gray-200">
        <div
          className={`${container} flex flex-col items-center justify-between gap-3 py-8 text-sm text-gray-500 sm:flex-row`}
        >
          <p>
            &copy; {new Date().getFullYear()} {APP_NAME}. Placeholder footer text.
          </p>
          <nav aria-label="Footer" className="flex gap-4">
            <a href={loginHref} className={`rounded-md hover:text-gray-900 ${focusRing}`}>
              Sign in
            </a>
            <a href={registerHref} className={`rounded-md hover:text-gray-900 ${focusRing}`}>
              Register
            </a>
          </nav>
        </div>
      </footer>
    </div>
  );
}