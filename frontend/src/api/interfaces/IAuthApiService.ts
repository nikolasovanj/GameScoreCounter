import type { LoginRequest, RegisterRequest } from "../../types/Requests";
import type { ApiPromise } from "../../helpers/api/api";
import type { AuthResponse } from "../../types/auth/AuthResponse";

export interface IAuthAPIService {
  login(request: LoginRequest): ApiPromise<AuthResponse>;
  register(request: RegisterRequest): ApiPromise<AuthResponse>;
}