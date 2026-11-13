import { useEffect, useId, useRef, useState, type SubmitEvent } from "react";
import type { AuthResponse } from "../../types/auth/AuthResponse";
import { authApi } from "../../api/services/AuthApiService";

type LoginFormProps = {
  /** Called with the server response after a successful login. Store the token and navigate here. */
  onSuccess: (auth: AuthResponse) => void;
  title?: string;
  /** Shows a "Register" link when provided. */
  registerHref?: string;
};

type FieldErrors = { username?: string; password?: string };

const inputBase =
  "block w-full rounded-md border bg-white px-3 py-2 text-sm text-gray-900 shadow-sm " +
  "focus:outline-none focus:ring-2 read-only:bg-gray-50";
const inputOk = "border-gray-300 focus:border-blue-600 focus:ring-blue-600/30";
const inputBad = "border-red-500 focus:border-red-500 focus:ring-red-500/30";

function validate(username: string, password: string): FieldErrors {
  const errors: FieldErrors = {};
  if (!username.trim()) errors.username = "Enter your username.";
  if (!password) errors.password = "Enter your password.";
  return errors;
}

export function LoginForm({
  onSuccess,
  title = "Sign in",
  registerHref,
}: LoginFormProps) {
  const titleId = useId();
  const usernameId = useId();
  const passwordId = useId();
  const usernameErrorId = useId();
  const passwordErrorId = useId();

  const usernameRef = useRef<HTMLInputElement>(null);
  const passwordRef = useRef<HTMLInputElement>(null);
  const mountedRef = useRef(true);

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [fieldErrors, setFieldErrors] = useState<FieldErrors>({});
  const [formError, setFormError] = useState<string | null>(null);

  // Setting it true in the effect keeps this correct under React StrictMode's double-mount
  useEffect(() => {
    mountedRef.current = true;
    return () => {
      mountedRef.current = false;
    };
  }, []);

  async function handleSubmit(e: SubmitEvent<HTMLFormElement>) {
    e.preventDefault();
    if (submitting) return;

    setFormError(null);

    const errors = validate(username, password);
    setFieldErrors(errors);
    if (errors.username) {
      usernameRef.current?.focus();
      return;
    }
    if (errors.password) {
      passwordRef.current?.focus();
      return;
    }

    setSubmitting(true);

    const res = await authApi.login({ username: username.trim(), password });

    // The form may have unmounted while the request was in flight
    if (!mountedRef.current) return;

    setSubmitting(false);

    if (res.success && res.data?.token) {
      onSuccess(res.data);
      return;
    }

    setFormError(res.message ?? "Login failed.");
    // Inputs are readOnly (not disabled) while submitting, so focus can return here
    passwordRef.current?.focus();
  }

  return (
    <form
      onSubmit={handleSubmit}
      noValidate
      aria-labelledby={titleId}
      className="w-full max-w-sm space-y-5 rounded-xl border border-gray-200 bg-white p-6 shadow-sm"
    >
      <h1 id={titleId} className="text-xl font-semibold text-gray-900">
        {title}
      </h1>

      {formError && (
        <div
          role="alert"
          className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700"
        >
          {formError}
        </div>
      )}

      <div className="space-y-1.5">
        <label htmlFor={usernameId} className="block text-sm font-medium text-gray-700">
          Username
        </label>
        <input
          id={usernameId}
          ref={usernameRef}
          type="text"
          name="username"
          autoComplete="username"
          autoCapitalize="none"
          autoCorrect="off"
          spellCheck={false}
          value={username}
          readOnly={submitting}
          onChange={(e) => {
            setUsername(e.target.value);
            if (fieldErrors.username) setFieldErrors((p) => ({ ...p, username: undefined }));
          }}
          aria-invalid={fieldErrors.username ? true : undefined}
          aria-describedby={fieldErrors.username ? usernameErrorId : undefined}
          className={`${inputBase} ${fieldErrors.username ? inputBad : inputOk}`}
        />
        {fieldErrors.username && (
          <p id={usernameErrorId} className="text-sm text-red-600">
            {fieldErrors.username}
          </p>
        )}
      </div>

      <div className="space-y-1.5">
        <label htmlFor={passwordId} className="block text-sm font-medium text-gray-700">
          Password
        </label>
        <div className="relative">
          <input
            id={passwordId}
            ref={passwordRef}
            type={showPassword ? "text" : "password"}
            name="password"
            autoComplete="current-password"
            value={password}
            readOnly={submitting}
            onChange={(e) => {
              setPassword(e.target.value);
              if (fieldErrors.password) setFieldErrors((p) => ({ ...p, password: undefined }));
            }}
            aria-invalid={fieldErrors.password ? true : undefined}
            aria-describedby={fieldErrors.password ? passwordErrorId : undefined}
            className={`${inputBase} pr-16 ${fieldErrors.password ? inputBad : inputOk}`}
          />
          <button
            type="button"
            onClick={() => setShowPassword((s) => !s)}
            aria-controls={passwordId}
            aria-label={showPassword ? "Hide password" : "Show password"}
            className="absolute inset-y-0 right-0 rounded-md px-3 text-xs font-medium text-gray-600 hover:text-gray-900 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-600"
          >
            {showPassword ? "Hide" : "Show"}
          </button>
        </div>
        {fieldErrors.password && (
          <p id={passwordErrorId} className="text-sm text-red-600">
            {fieldErrors.password}
          </p>
        )}
      </div>

      <button
        type="submit"
        disabled={submitting}
        aria-busy={submitting}
        className="w-full rounded-md bg-blue-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-700 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-600 focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-60"
      >
        {submitting ? "Signing in…" : "Sign in"}
      </button>

      {registerHref && (
        <p className="text-center text-sm text-gray-600">
          Don&apos;t have an account?{" "}
          <a href={registerHref} className="font-medium text-blue-600 hover:underline">
            Register
          </a>
        </p>
      )}
    </form>
  );
}