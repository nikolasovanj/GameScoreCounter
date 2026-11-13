import { useEffect, useId, useRef, useState, type SubmitEvent } from "react";
import { DEFAULT_GAMES, type GameOption, type PlayerOption } from "../match/MatchForm";
import type { ScheduleDto } from "../../models/ScheduleDtos";
import type { CreateScheduleRequest } from "../../types/Requests";
import { scheduleApi } from "../../api/services/ScheduleService";

type ScheduleFormProps = {
  /** Players that can be invited. Leave yourself out: the organizer is added automatically. */
  players: PlayerOption[];
  defaultInvitedIds?: string[];
  games?: GameOption[];
  onSuccess: (schedule: ScheduleDto) => void;
  title?: string;
};

type FormErrors = { game?: string; matchTime?: string; location?: string };

const LOCATION_MAX = 200; // matches the column length in the EF configuration
const MAX_INVITEES = 20;

const inputBase =
  "block w-full rounded-md border bg-white px-3 py-2 text-sm text-gray-900 shadow-sm focus:outline-none focus:ring-2";
const inputOk = "border-gray-300 focus:border-blue-600 focus:ring-blue-600/30";
const inputBad = "border-red-500 focus:border-red-500 focus:ring-red-500/30";
const focusRing =
  "focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-600 focus-visible:ring-offset-2";

// <input type="datetime-local"> wants local time as "YYYY-MM-DDTHH:mm"
function toLocalInputValue(d: Date): string {
  const pad = (n: number) => String(n).padStart(2, "0");
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`;
}

// Next full hour, always at least 30 minutes from now
function defaultMatchTime(): string {
  const d = new Date(Date.now() + 30 * 60 * 1000);
  if (d.getMinutes() > 0 || d.getSeconds() > 0 || d.getMilliseconds() > 0) {
    d.setHours(d.getHours() + 1);
  }
  d.setMinutes(0, 0, 0);
  return toLocalInputValue(d);
}

function validate(game: GameOption | undefined, matchTime: string, location: string): FormErrors {
  const errors: FormErrors = {};

  if (!game) errors.game = "Choose a game.";

  const time = new Date(matchTime).getTime();
  if (!matchTime || Number.isNaN(time)) errors.matchTime = "Enter when the match is.";
  else if (time <= Date.now()) errors.matchTime = "Pick a time in the future.";

  if (location.trim().length > LOCATION_MAX)
    errors.location = `Location can be at most ${LOCATION_MAX} characters.`;

  return errors;
}

export function ScheduleForm({
  players,
  defaultInvitedIds = [],
  games = DEFAULT_GAMES,
  onSuccess,
  title = "Schedule a match",
}: ScheduleFormProps) {
  const baseId = useId();
  const titleId = `${baseId}-title`;
  const gameId = `${baseId}-game`;
  const gameErrorId = `${baseId}-game-error`;
  const timeId = `${baseId}-time`;
  const timeErrorId = `${baseId}-time-error`;
  const locationId = `${baseId}-location`;
  const locationErrorId = `${baseId}-location-error`;
  const invitedLabelId = `${baseId}-invited`;
  const pickerId = `${baseId}-picker`;

  const formRef = useRef<HTMLFormElement>(null);
  const alertRef = useRef<HTMLDivElement>(null);
  const pickerRef = useRef<HTMLSelectElement>(null);
  const mountedRef = useRef(true);

  const [gameValue, setGameValue] = useState(games[0]?.value ?? "");
  const [matchTime, setMatchTime] = useState(defaultMatchTime);
  const [location, setLocation] = useState("");
  const [invitedIds, setInvitedIds] = useState<string[]>(() => [...new Set(defaultInvitedIds)]);
  const [pickId, setPickId] = useState("");
  const [errors, setErrors] = useState<FormErrors>({});
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
  const available = players.filter((p) => !invitedIds.includes(p.id));

  const nameOf = (id: string) =>
    players.find((p) => p.id === id)?.name ?? `Player ${id.slice(0, 8)}`;

  function addPlayer(id: string) {
    if (!id || invitedIds.length >= MAX_INVITEES || invitedIds.includes(id)) return;
    setInvitedIds((prev) => [...prev, id]);
    setPickId("");
    pickerRef.current?.focus();
  }

  function removePlayer(id: string) {
    setInvitedIds((prev) => prev.filter((x) => x !== id));
    // The removed row's button is gone, so move focus somewhere sensible
    pickerRef.current?.focus();
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

    const found = validate(game, matchTime, location);
    setErrors(found);
    if (!game || found.game || found.matchTime || found.location) {
      focusFirstInvalid();
      return;
    }

    const request: CreateScheduleRequest = {
      game: game.value,
      // datetime-local has no offset, so this parses as local time; JSON turns it into UTC ("...Z")
      matchTime: new Date(matchTime),
      location: location.trim() || undefined,
      invitedPlayerIds: invitedIds,
    };

    setSubmitting(true);

    const res = await scheduleApi.Create(request);

    // The form may have unmounted while the request was in flight
    if (!mountedRef.current) return;

    setSubmitting(false);

    if (res.success && res.data) {
      onSuccess(res.data);
      return;
    }

    setFormError(res.message ?? "Create failed");
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
              onChange={(e) => {
                setGameValue(e.target.value);
                setErrors((prev) => ({ ...prev, game: undefined }));
              }}
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
            <label htmlFor={timeId} className="block text-sm font-medium text-gray-700">
              Date and time
            </label>
            <input
              id={timeId}
              type="datetime-local"
              value={matchTime}
              onChange={(e) => {
                setMatchTime(e.target.value);
                setErrors((prev) => ({ ...prev, matchTime: undefined }));
              }}
              aria-invalid={errors.matchTime ? true : undefined}
              aria-describedby={errors.matchTime ? timeErrorId : undefined}
              className={`${inputBase} ${errors.matchTime ? inputBad : inputOk}`}
            />
            {errors.matchTime && (
              <p id={timeErrorId} className="text-sm text-red-600">
                {errors.matchTime}
              </p>
            )}
          </div>
        </div>

        <div className="space-y-1.5">
          <label htmlFor={locationId} className="block text-sm font-medium text-gray-700">
            Location <span className="font-normal text-gray-500">(optional)</span>
          </label>
          <input
            id={locationId}
            type="text"
            autoComplete="off"
            value={location}
            onChange={(e) => {
              setLocation(e.target.value);
              setErrors((prev) => ({ ...prev, location: undefined }));
            }}
            aria-invalid={errors.location ? true : undefined}
            aria-describedby={errors.location ? locationErrorId : undefined}
            className={`${inputBase} ${errors.location ? inputBad : inputOk}`}
          />
          {errors.location && (
            <p id={locationErrorId} className="text-sm text-red-600">
              {errors.location}
            </p>
          )}
        </div>

        <div role="group" aria-labelledby={invitedLabelId} className="space-y-3">
          <div className="flex items-baseline justify-between">
            <h2 id={invitedLabelId} className="text-sm font-semibold text-gray-900">
              Invited players ({invitedIds.length})
            </h2>
            <span className="text-xs text-gray-500">You&apos;re added automatically</span>
          </div>

          {invitedIds.length === 0 ? (
            <p className="rounded-md border border-dashed border-gray-300 px-3 py-4 text-center text-sm text-gray-500">
              No one invited yet. You can also invite players after creating the schedule.
            </p>
          ) : (
            <ul className="divide-y divide-gray-100 rounded-lg border border-gray-200">
              {invitedIds.map((id) => {
                const name = nameOf(id);
                return (
                  <li key={id} className="flex items-center justify-between gap-3 px-4 py-2.5">
                    <span className="truncate text-sm font-medium text-gray-900">{name}</span>
                    <button
                      type="button"
                      onClick={() => removePlayer(id)}
                      aria-label={`Remove ${name}`}
                      className={`shrink-0 rounded-md px-2 py-1 text-sm font-medium text-red-600 hover:bg-red-50 disabled:opacity-60 ${focusRing}`}
                    >
                      Remove
                    </button>
                  </li>
                );
              })}
            </ul>
          )}

          <div className="flex gap-2">
            <label htmlFor={pickerId} className="sr-only">
              Player to invite
            </label>
            <select
              id={pickerId}
              ref={pickerRef}
              value={pickId}
              onChange={(e) => setPickId(e.target.value)}
              className={`${inputBase} ${inputOk}`}
            >
              <option value="">
                {available.length > 0 ? "Select a player…" : "No more players to invite"}
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
              disabled={!pickId || invitedIds.length >= MAX_INVITEES}
              className={`rounded-md border border-gray-300 bg-white px-3 py-2 text-sm font-medium text-gray-700 shadow-sm hover:bg-gray-50 disabled:cursor-not-allowed disabled:opacity-60 ${focusRing}`}
            >
              Invite
            </button>
          </div>
        </div>

        <button
          type="submit"
          aria-busy={submitting}
          className={`w-full rounded-md bg-blue-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-700 disabled:cursor-not-allowed disabled:opacity-60 ${focusRing}`}
        >
          {submitting ? "Scheduling…" : "Create schedule"}
        </button>
      </fieldset>
    </form>
  );
}