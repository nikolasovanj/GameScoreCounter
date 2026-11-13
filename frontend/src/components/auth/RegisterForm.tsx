import { useEffect, useId, useRef, useState, type SubmitEvent } from "react";
import type { AuthResponse } from "../../types/auth/AuthResponse";
import { authApi } from "../../api/services/AuthApiService";

type RegisterFormProps = {
  /** Called with the server response after a successful registration. Store the token and navigate here. */
  onSuccess: (auth: AuthResponse) => void;
  title?: string;
  /** Shows a "Sign in" link when provided. */
  loginHref?: string;
};

type FieldErrors = {
  username?: string;
  password?: string;
  passwordRepeat?: string;
};

// Keep these in line with the rules your backend enforces
const USERNAME_MIN = 3;
const USERNAME_MAX = 32;
const PASSWORD_MIN = 8;
const PASSWORD_MAX = 128;

const inputBase =
  "block w-full rounded-md border bg-white px-3 py-2 text-sm text-gray-900 shadow-sm " +
  "focus:outline-none focus:ring-2 read-only:bg-gray-50";
const inputOk = "border-gray-300 focus:border-blue-600 focus:ring-blue-600/30";
const inputBad = "border-red-500 focus:border-red-500 focus:ring-red-500/30";

function validate(username: string, password: string, passwordRepeat: string): FieldErrors {
  const errors: FieldErrors = {};
  const name = username.trim();

  if (!name) errors.username = "Choose a username.";
  else if (name.length < USERNAME_MIN)
    errors.username = `Username must be at least ${USERNAME_MIN} characters.`;
  else if (name.length > USERNAME_MAX)
    errors.username = `Username must be at most ${USERNAME_MAX} characters.`;

  if (!password) errors.password = "Choose a password.";
  else if (password.length < PASSWORD_MIN)
    errors.password = `Password must be at least ${PASSWORD_MIN} characters.`;
  else if (password.length > PASSWORD_MAX)
    errors.password = `Password must be at most ${PASSWORD_MAX} characters.`;

  if (!passwordRepeat) errors.passwordRepeat = "Repeat your password.";
  else if (password && passwordRepeat !== password)
    errors.passwordRepeat = "Passwords don't match.";

  return errors;
}

export function RegisterForm({
  onSuccess,
  title = "Create account",
  loginHref,
}: RegisterFormProps) {
  const titleId = useId();
  const usernameId = useId();
  const passwordId = useId();
  const repeatId = useId();
  const usernameErrorId = useId();
  const passwordErrorId = useId();
  const repeatErrorId = useId();
  const passwordHintId = useId();

  const usernameRef = useRef<HTMLInputElement>(null);
  const passwordRef = useRef<HTMLInputElement>(null);
  const repeatRef = useRef<HTMLInputElement>(null);
  const mountedRef = useRef(true);

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [passwordRepeat, setPasswordRepeat] = useState("");
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

  function clearError(field: keyof FieldErrors) {
    if (fieldErrors[field]) setFieldErrors((prev) => ({ ...prev, [field]: undefined }));
  }

  async function handleSubmit(e: SubmitEvent<HTMLFormElement>) {
    e.preventDefault();
    if (submitting) return;

    setFormError(null);

    const errors = validate(username, password, passwordRepeat);
    setFieldErrors(errors);
    if (errors.username) {
      usernameRef.current?.focus();
      return;
    }
    if (errors.password) {
      passwordRef.current?.focus();
      return;
    }
    if (errors.passwordRepeat) {
      repeatRef.current?.focus();
      return;
    }

    setSubmitting(true);

    const res = await authApi.register({
      username: username.trim(),
      password,
      passwordRepeat,
    });

    // The form may have unmounted while the request was in flight
    if (!mountedRef.current) return;

    setSubmitting(false);

    if (res.success && res.data?.token) {
      onSuccess(res.data);
      return;
    }

    // The server message is usually about the username (e.g. already taken)
    setFormError(res.message ?? "Register failed.");
    // Inputs are readOnly (not disabled) while submitting, so focus can return here
    usernameRef.current?.focus();
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
            clearError("username");
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
            autoComplete="new-password"
            value={password}
            readOnly={submitting}
            onChange={(e) => {
              setPassword(e.target.value);
              clearError("password");
              // A mismatch error no longer applies once the first password changes
              clearError("passwordRepeat");
            }}
            aria-invalid={fieldErrors.password ? true : undefined}
            aria-describedby={fieldErrors.password ? passwordErrorId : passwordHintId}
            className={`${inputBase} pr-16 ${fieldErrors.password ? inputBad : inputOk}`}
          />
          <button
            type="button"
            onClick={() => setShowPassword((s) => !s)}
            aria-controls={`${passwordId} ${repeatId}`}
            aria-label={showPassword ? "Hide passwords" : "Show passwords"}
            className="absolute inset-y-0 right-0 rounded-md px-3 text-xs font-medium text-gray-600 hover:text-gray-900 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-600"
          >
            {showPassword ? "Hide" : "Show"}
          </button>
        </div>
        {fieldErrors.password ? (
          <p id={passwordErrorId} className="text-sm text-red-600">
            {fieldErrors.password}
          </p>
        ) : (
          <p id={passwordHintId} className="text-xs text-gray-500">
            At least {PASSWORD_MIN} characters.
          </p>
        )}
      </div>

      <div className="space-y-1.5">
        <label htmlFor={repeatId} className="block text-sm font-medium text-gray-700">
          Repeat password
        </label>
        <input
          id={repeatId}
          ref={repeatRef}
          type={showPassword ? "text" : "password"}
          name="passwordRepeat"
          autoComplete="new-password"
          value={passwordRepeat}
          readOnly={submitting}
          onChange={(e) => {
            setPasswordRepeat(e.target.value);
            clearError("passwordRepeat");
          }}
          aria-invalid={fieldErrors.passwordRepeat ? true : undefined}
          aria-describedby={fieldErrors.passwordRepeat ? repeatErrorId : undefined}
          className={`${inputBase} ${fieldErrors.passwordRepeat ? inputBad : inputOk}`}
        />
        {fieldErrors.passwordRepeat && (
          <p id={repeatErrorId} className="text-sm text-red-600">
            {fieldErrors.passwordRepeat}
          </p>
        )}
      </div>

      <button
        type="submit"
        disabled={submitting}
        aria-busy={submitting}
        className="w-full rounded-md bg-blue-600 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-700 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-blue-600 focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-60"
      >
        {submitting ? "Creating account…" : "Create account"}
      </button>

      {loginHref && (
        <p className="text-center text-sm text-gray-600">
          Already have an account?{" "}
          <a href={loginHref} className="font-medium text-blue-600 hover:underline">
            Sign in
          </a>
        </p>
      )}
    </form>
  );
}