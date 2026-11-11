import axios from "axios";
import { readItem } from "../local_storage/local_storage";

export type ApiResponse<T> = { success: boolean; data?: T; message?: string };
export type ApiPromise<T = null> = Promise<ApiResponse<T>>

export const authHeader = () => {
  const token = readItem("authToken");
  return token ? { Authorization: `Bearer ${token}` } : {};
};

type ApiError = { response?: { data?: { message?: string } } };

export const apiErr = <T>(e: ApiError | null | undefined, fallback: string): ApiResponse<T> => ({
  success: false,
  message: axios.isAxiosError(e)
    ? e?.response?.data?.message ?? fallback
    : fallback,
});