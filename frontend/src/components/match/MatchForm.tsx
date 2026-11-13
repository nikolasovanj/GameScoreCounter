import { useEffect, useId, useRef, useState, type SubmitEvent } from "react";
import type { MatchDto } from "../../models/MatchDtos";
import type { CreateMatchRequest, MatchResultRequest } from "../../types/Requests";
import { matchApi } from "../../api/services/MatchApiService";
export type PlayerOption = { id: string; name: string };

/** win = winner only (Chess), score = one total (Darts), rounds = per-round points that sum up (Rummy) */
export type ScoringMode = "win" | "score" | "rounds";

export type GameOption = {
  /** Must match the backend GameType name exactly. */
  value: string;
  label: string;
  scoring: ScoringMode;
  minPlayers?: number;
};

// Example values from earlier in this project: keep in sync with your GameType enum and scoring rules
export const DEFAULT_GAMES: GameOption[] = [
  { value: "Chess", label: "Chess", scoring: "win", minPlayers: 2 },
  { value: "Darts", label: "Darts", scoring: "score", minPlayers: 2 },
  { value: "Rummy", label: "Rummy", scoring: "rounds", minPlayers: 2 },
];

type MatchFormProps = {
  /** Players that can be added to the match. */
  players: PlayerOption[];
  /** Players added up front, e.g. the current user. */
  defaultPlayerIds?: string[];
  games?: GameOption[];
  onSuccess: (match: MatchDto) => void;
  title?: string;
};

type Entry = { playerId: string; isWinner: boolean; score: string; rounds: string[] };
type EntryErrors = { score?: string; rounds?: string; badRounds: number[] };
type FormErrors = {
  game?: string;
  playedAt?: string;
  players?: string;
  entries: Record<string, EntryErrors>;
};

// Keeps the stored sum of rounds far below the backend's 32-bit int limit
const MAX_POINTS = 1_000_000;
const MAX_ROUNDS = 100;
const MAX_PLAYERS = 20;
const INT = /^-?\d+$/;

const inputBase =
  "block w-full rounded-md border bg-white px-3 py-2 text-sm text-gray-900 shadow-sm focus:outline-none focus:ring-2";
const inputOk = "border-gray-300 focus:border-blue-600 focus:ring-blue-600/30";
const inputBad = "border-red-500 focus:border-red-500 focus:ring-red-500/30";
const focusRing =
  "focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-600 focus-visible:ring-offset-2";
const secondaryButton =
  `rounded-md border border-gray-300 bg-white px-3 py-2 text-sm font-medium text-gray-700 shadow-sm hover:bg-gray-50 disabled:cursor-not-allowed disabled:opacity-60 ${focusRing}`;

const emptyEntry = (playerId: string, mode: ScoringMode): Entry => ({
  playerId,
  isWinner: false,
  score: "",
  rounds: mode === "rounds" ? [""] : [],
});

// <input type="datetime-local"> wants local time as "YYYY-MM-DDTHH:mm"
function toLocalInputValue(d: Date): string {
  const pad = (n: number) => String(n).padStart(2, "0");
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
}

function isValidPoints(raw: string): boolean {
  const s = raw.trim();
  return INT.test(s) && Math.abs(Number(s)) <= MAX_POINTS;
}

function hasErrors(e: FormErrors): boolean {
  return (
    Boolean(e.game || e.playedAt || e.players) ||
    Object.values(e.entries).some((x) => x.score || x.rounds)
  );
}

function validate(game: GameOption | undefined, playedAt: string, entries: Entry[]): FormErrors {
  const errors: FormErrors = { entries: {} };

  if (!game) errors.game = "Choose a game.";

  if (!playedAt || Number.isNaN(new Date(playedAt).getTime()))
    errors.playedAt = "Enter when the match was played.";

  const min = game?.minPlayers ?? 1;
  if (entries.length < min)
    errors.players = min === 1 ? "Add at least one player." : `Add at least ${min} players.`;

  if (!game) return errors;

  for (const entry of entries) {
    const entryErrors: EntryErrors = { badRounds: [] };

    if (game.scoring === "score") {
      const raw = entry.score.trim();
      if (!INT.test(raw)) entryErrors.score = "Enter a whole number.";
      else if (Number(raw) < 0) entryErrors.score = "Score can't be negative.";
      else if (Number(raw) > MAX_POINTS)
        entryErrors.score = `Score can't be more than ${MAX_POINTS.toLocaleString("en-US")}.`;
    }

    if (game.scoring === "rounds") {
      if (entry.rounds.length === 0) {
        entryErrors.rounds = "Add at least one round.";
      } else {
        entry.rounds.forEach((r, i) => {
          if (!isValidPoints(r)) entryErrors.badRounds.push(i);
        });
        if (entryErrors.badRounds.length > 0)
          entryErrors.rounds = `Every round needs a whole number between -${MAX_POINTS.toLocaleString("en-US")} and ${MAX_POINTS.toLocaleString("en-US")}.`;
      }
    }

    if (entryErrors.score || entryErrors.rounds) errors.entries[entry.playerId] = entryErrors;
  }

  return errors;
}

// Mirrors the backend scoring rules: win-only sends no score, "score" sends totalScore,
// "rounds" sends components and NO totalScore (the server calculates it and rejects one).
function buildRequest(game: GameOption, playedAt: string, entries: Entry[]): CreateMatchRequest {
  const results: MatchResultRequest[] = entries.map((e) => {
    const base = { playerId: e.playerId, isWinner: e.isWinner };
    switch (game.scoring) {
      case "score":
        return { ...base, totalScore: Number(e.score.trim()) };
      case "rounds":
        return {
          ...base,
          components: e.rounds.map((points, i) => ({ order: i + 1, points: Number(points.trim()) })),
        };
      default:
        return base;
    }
  });

  // datetime-local has no offset, so this parses as local time; JSON turns it into UTC ("...Z")
  return { game: game.value, playedAt: new Date(playedAt), results };
}

export function MatchForm({
  players,
  defaultPlayerIds = [],
  games = DEFAULT_GAMES,
  onSuccess,
  title = "Record match",
}: MatchFormProps) {
  const baseId = useId();
  const titleId = `${baseId}-title`;
  const gameId = `${baseId}-game`;
  const gameErrorId = `${baseId}-game-error`;
  const playedAtId = `${baseId}-played-at`;
  const playedAtErrorId = `${baseId}-played-at-error`;
  const playersLabelId = `${baseId}-players`;
  const pickerId = `${baseId}-picker`;
  const playersErrorId = `${baseId}-players-error`;

  const formRef = useRef<HTMLFormElement>(null);
  const alertRef = useRef<HTMLDivElement>(null);
  const mountedRef = useRef(true);

  const [gameValue, setGameValue] = useState(games[0]?.value ?? "");
  const [playedAt, setPlayedAt] = useState(() => toLocalInputValue(new Date()));
  const [entries, setEntries] = useState<Entry[]>(() => {
    const mode = games[0]?.scoring ?? "win";
    return [...new Set(defaultPlayerIds)].map((id) => emptyEntry(id, mode));
  });
  const [pickId, setPickId] = useState("");
  const [errors, setErrors] = useState<FormErrors>({ entries: {} });
  const [formError, setFormError] = useState<string | null>(null);
  const [submitting, setSubmitting] = useState(false);

  // Setting it true in the effect keeps this correct under React StrictMode's double-mount
  useEffect(() => {
    mountedRef.current = true;
    return () => {
      mountedRef.current = false;
    };
  }, []);

  const game = games.find((g) => g.value === gameValue);
  const mode: ScoringMode = game?.scoring ?? "win";
  const minPlayers = game?.minPlayers ?? 1;
  const available = players.filter((p) => !entries.some((e) => e.playerId === p.id));

  const nameOf = (id: string) =>
    players.find((p) => p.id === id)?.name ?? `Player ${id.slice(0, 8)}`;

  function clearEntryError(playerId: string) {
    setErrors((prev) => {
      if (!prev.entries[playerId]) return prev;
      const next = { ...prev.entries };
      delete next[playerId];
      return { ...prev, entries: next };
    });
  }

  function updateEntry(playerId: string, patch: Partial<Entry>) {
    setEntries((prev) => prev.map((e) => (e.playerId === playerId ? { ...e, ...patch } : e)));
  }

  function changeGame(value: string) {
    const next = games.find((g) => g.value === value);
    setGameValue(value);
    setErrors((prev) => ({ ...prev, game: undefined, players: undefined, entries: {} }));

    // A different scoring shape means the entered scores no longer apply
    if ((next?.scoring ?? "win") !== mode) {
      const nextMode = next?.scoring ?? "win";
      setEntries((prev) =>
        prev.map((e) => ({ ...e, score: "", rounds: nextMode === "rounds" ? [""] : [] })),
      );
    }
  }

  function addPlayer(id: string) {
    if (!id || entries.length >= MAX_PLAYERS || entries.some((e) => e.playerId === id)) return;
    setEntries((prev) => [...prev, emptyEntry(id, mode)]);
    setPickId("");
    setErrors((prev) => ({ ...prev, players: undefined }));
  }

  function removePlayer(id: string) {
    setEntries((prev) => prev.filter((e) => e.playerId !== id));
    clearEntryError(id);
  }

  function setRound(playerId: string, index: number, value: string) {
    setEntries((prev) =>
      prev.map((e) =>
        e.playerId === playerId
          ? { ...e, rounds: e.rounds.map((r, i) => (i === index ? value : r)) }
          : e,
      ),
    );
    clearEntryError(playerId);
  }

  function addRound(playerId: string) {
    setEntries((prev) =>
      prev.map((e) =>
        e.playerId === playerId && e.rounds.length < MAX_ROUNDS
          ? { ...e, rounds: [...e.rounds, ""] }
          : e,
      ),
    );
  }

  function removeRound(playerId: string, index: number) {
    setEntries((prev) =>
      prev.map((e) =>
        e.playerId === playerId && e.rounds.length > 1
          ? { ...e, rounds: e.rounds.filter((_, i) => i !== index) }
          : e,
      ),
    );
    clearEntryError(playerId);
  }

  function focusFirstInvalid() {
    requestAnimationFrame(() => {
      formRef.current?.querySelector<HTMLElement>('[aria-invalid="true"]')?.focus();
    });
  }

  async function handleSubmit(e: SubmitEvent<HTMLFormElement>) {
    e.preventDefault();
    if (submitting) return;

    setFormError(null);

    const found = validate(game, playedAt, entries);
    setErrors(found);
    if (!game || hasErrors(found)) {
      focusFirstInvalid();
      return;
    }

    setSubmitting(true);

    const res = await matchApi.Create(buildRequest(game, playedAt, entries));

    // The form may have unmounted while the request was in flight
    if (!mountedRef.current) return;

    setSubmitting(false);

    if (res.success && res.data) {
      onSuccess(res.data);
      return;
    }

    setFormError(res.message ?? "Create failed.");
    requestAnimationFrame(() => alertRef.current?.focus());
  }

  return (
    <form
      ref={formRef}
      onSubmit={handleSubmit}
      noValidate
      aria-labelledby={titleId}
      className="w-full max-w-2xl rounded-xl border border-gray-200 bg-white p-6 shadow-sm"
    >
      {/* A disabled fieldset disables every control inside it while the request is running */}
      <fieldset disabled={submitting} className="min-w-0 space-y-6">
        <h1 id={titleId} className="text-xl font-semibold text-gray-900">
          {title}
        </h1>

        {formError && (
          <div
            ref={alertRef}
            tabIndex={-1}
            role="alert"
            className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700 focus:outline-none"
          >
            {formError}
          </div>
        )}

        <div className="grid gap-4 sm:grid-cols-2">
          <div className="space-y-1.5">
            <label htmlFor={gameId} className="block text-sm font-medium text-gray-700">
              Game
            </label>
            <select
              id={gameId}
              value={gameValue}
              onChange={(e) => changeGame(e.target.value)}
              aria-invalid={errors.game ? true : undefined}
              aria-describedby={errors.game ? gameErrorId : undefined}
              className={`${inputBase} ${errors.game ? inputBad : inputOk}`}
            >
              {games.map((g) => (
                <option key={g.value} value={g.value}>
                  {g.label}
                </option>
              ))}
            </select>
            {errors.game && (
              <p id={gameErrorId} className="text-sm text-red-600">
                {errors.game}
              </p>
            )}
          </div>

          <div className="space-y-1.5">
            <label htmlFor={playedAtId} className="block text-sm font-medium text-gray-700">
              Played at
            </label>
            <input
              id={playedAtId}
              type="datetime-local"
              value={playedAt}
              onChange={(e) => {
                setPlayedAt(e.target.value);
                setErrors((prev) => ({ ...prev, playedAt: undefined }));
              }}
              aria-invalid={errors.playedAt ? true : undefined}
              aria-describedby={errors.playedAt ? playedAtErrorId : undefined}
              className={`${inputBase} ${errors.playedAt ? inputBad : inputOk}`}
            />
            {errors.playedAt && (
              <p id={playedAtErrorId} className="text-sm text-red-600">
                {errors.playedAt}
              </p>
            )}
          </div>
        </div>

        <div role="group" aria-labelledby={playersLabelId} className="space-y-3">
          <div className="flex items-baseline justify-between">
            <h2 id={playersLabelId} className="text-sm font-semibold text-gray-900">
              Players ({entries.length})
            </h2>
            {minPlayers > 1 && (
              <span className="text-xs text-gray-500">Minimum {minPlayers}</span>
            )}
          </div>

          {entries.length === 0 && (
            <p className="rounded-md border border-dashed border-gray-300 px-3 py-4 text-center text-sm text-gray-500">
              No players yet. Pick one below to add them to the match.
            </p>
          )}

          <ul className="space-y-3">
            {entries.map((entry) => {
              const name = nameOf(entry.playerId);
              const entryErrors = errors.entries[entry.playerId];
              const scoreId = `${baseId}-${entry.playerId}-score`;
              const scoreErrorId = `${scoreId}-error`;
              const roundsErrorId = `${baseId}-${entry.playerId}-rounds-error`;

              const roundsTotal = entry.rounds.reduce(
                (sum, r) => (isValidPoints(r) ? sum + Number(r.trim()) : sum),
                0,
              );

              return (
                <li key={entry.playerId} className="rounded-lg border border-gray-200 p-4">
                  <div className="flex items-center justify-between gap-3">
                    <span className="truncate text-sm font-semibold text-gray-900">{name}</span>
                    <div className="flex shrink-0 items-center gap-4">
                      <label className="flex items-center gap-2 text-sm text-gray-700">
                        <input
                          type="checkbox"
                          checked={entry.isWinner}
                          onChange={(e) => updateEntry(entry.playerId, { isWinner: e.target.checked })}
                          className="h-4 w-4 rounded border-gray-300 text-blue-600 focus:ring-blue-600"
                        />
                        Winner
                      </label>
                      <button
                        type="button"
                        onClick={() => removePlayer(entry.playerId)}
                        aria-label={`Remove ${name}`}
                        className={`rounded-md px-2 py-1 text-sm font-medium text-red-600 hover:bg-red-50 disabled:opacity-60 ${focusRing}`}
                      >
                        Remove
                      </button>
                    </div>
                  </div>

                  {mode === "score" && (
                    <div className="mt-3 space-y-1.5">
                      <label htmlFor={scoreId} className="block text-sm font-medium text-gray-700">
                        Total score
                      </label>
                      <input
                        id={scoreId}
                        type="text"
                        inputMode="numeric"
                        autoComplete="off"
                        value={entry.score}
                        onChange={(e) => {
                          updateEntry(entry.playerId, { score: e.target.value });
                          clearEntryError(entry.playerId);
                        }}
                        aria-invalid={entryErrors?.score ? true : undefined}
                        aria-describedby={entryErrors?.score ? scoreErrorId : undefined}
                        className={`${inputBase} w-40 ${entryErrors?.score ? inputBad : inputOk}`}
                      />
                      {entryErrors?.score && (
                        <p id={scoreErrorId} className="text-sm text-red-600">
                          {entryErrors.score}
                        </p>
                      )}
                    </div>
                  )}

                  {mode === "rounds" && (
                    <div className="mt-3 space-y-2">
                      <ul className="space-y-2">
                        {entry.rounds.map((points, i) => {
                          const roundId = `${baseId}-${entry.playerId}-round-${i}`;
                          return (
                            <li key={i} className="flex items-center gap-3">
                              <label
                                htmlFor={roundId}
                                className="w-20 shrink-0 text-sm font-medium text-gray-700"
                              >
                                Round {i + 1}
                              </label>
                              <input
                                id={roundId}
                                type="text"
                                inputMode="numeric"
                                autoComplete="off"
                                value={points}
                                onChange={(e) => setRound(entry.playerId, i, e.target.value)}
                                aria-invalid={entryErrors?.badRounds.includes(i) ? true : undefined}
                                aria-describedby={entryErrors?.rounds ? roundsErrorId : undefined}
                                className={`${inputBase} w-32 ${entryErrors?.badRounds.includes(i) ? inputBad : inputOk}`}
                              />
                              <button
                                type="button"
                                onClick={() => removeRound(entry.playerId, i)}
                                disabled={entry.rounds.length <= 1}
                                aria-label={`Remove round ${i + 1} for ${name}`}
                                className={`rounded-md px-2 py-1 text-sm text-gray-600 hover:bg-gray-100 disabled:cursor-not-allowed disabled:opacity-40 ${focusRing}`}
                              >
                                ✕
                              </button>
                            </li>
                          );
                        })}
                      </ul>

                      {entryErrors?.rounds && (
                        <p id={roundsErrorId} className="text-sm text-red-600">
                          {entryErrors.rounds}
                        </p>
                      )}

                      <div className="flex items-center justify-between">
                        <button
                          type="button"
                          onClick={() => addRound(entry.playerId)}
                          disabled={entry.rounds.length >= MAX_ROUNDS}
                          className={`rounded-md px-2 py-1 text-sm font-medium text-blue-600 hover:bg-blue-50 disabled:cursor-not-allowed disabled:opacity-60 ${focusRing}`}
                        >
                          + Add round
                        </button>
                        <span className="text-sm text-gray-600">
                          Total: <span className="font-semibold tabular-nums">{roundsTotal}</span>
                        </span>
                      </div>
                    </div>
                  )}
                </li>
              );
            })}
          </ul>

          <div className="flex gap-2">
            <label htmlFor={pickerId} className="sr-only">
              Player to add
            </label>
            <select
              id={pickerId}
              value={pickId}
              onChange={(e) => setPickId(e.target.value)}
              aria-invalid={errors.players ? true : undefined}
              aria-describedby={errors.players ? playersErrorId : undefined}
              className={`${inputBase} ${errors.players ? inputBad : inputOk}`}
            >
              <option value="">
                {available.length > 0 ? "Select a player…" : "No more players to add"}
              </option>
              {available.map((p) => (
                <option key={p.id} value={p.id}>
                  {p.name}
                </option>
              ))}
            </select>
            <button
              type="button"
              onClick={() => addPlayer(pickId)}
              disabled={!pickId || entries.length >= MAX_PLAYERS}
              className={secondaryButton}
            >
              Add
            </button>
          </div>
          {errors.players && (
            <p id={playersErrorId} className="text-sm text-red-600">
              {errors.players}
            </p>
          )}
        </div>

        <button
          type="submit"
          aria-busy={submitting}
          className={`w-full rounded-md bg-blue-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-60 ${focusRing}`}
        >
          {submitting ? "Saving…" : "Save match"}
        </button>
      </fieldset>
    </form>
  );
}