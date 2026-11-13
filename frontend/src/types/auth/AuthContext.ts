import type { AuthResponse } from "./AuthResponse";
import type { AuthUser } from "./AuthUser";

export type AuthContextType = {
  user: AuthUser | null;
  token: string | null;
  login: (token: AuthResponse) => void;
  logout: () => void;
  isAuthenticated: boolean;
  isLoading: boolean;
};