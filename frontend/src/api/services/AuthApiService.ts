import axios from "axios";
import type { AuthResponse } from "../../types/auth/AuthResponse";
import { apiErr, type ApiPromise, type ApiResponse } from "../../helpers/api/api";
import type { IAuthAPIService } from "../interfaces/IAuthApiService";
import type { LoginRequest, RegisterRequest } from "../../types/Requests";

const BASE = import.meta.env.VITE_API_URL + "auth";

export const authApi: IAuthAPIService = {
    login: async function (request: LoginRequest): ApiPromise<AuthResponse> {
        return axios.post<ApiResponse<AuthResponse>>(`${BASE}/login`, {request})
            .then(r => r.data).catch(e => apiErr(e, "Login failed."));
    },
    register: async function (request: RegisterRequest): ApiPromise<AuthResponse> {
        return axios.post<ApiResponse<AuthResponse>>(`${BASE}/register`, {request})
            .then(r => r.data).catch(e => apiErr(e, "Register failed."));
    }
};