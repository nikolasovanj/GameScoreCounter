import axios from "axios";
import { apiErr, type ApiResponse, authHeader, type ApiPromise } from "../../helpers/api/api";
import type { UserDto } from "../../models/UserDtos";
import type { PageRequest, Page } from "../../types/Pagination";
import type { IFollowApiService } from "../interfaces/IFollowApuService";

const BASE = import.meta.env.VITE_API_URL + "users";

export const followAPi: IFollowApiService = {
    follow: async function (id: string): ApiPromise {
        return axios.put<ApiResponse<null>>(`${BASE}/${id}/follow`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "Follow failed."));
    },
    unfollow: async function (id: string): ApiPromise {
        return axios.delete<ApiResponse<null>>(`${BASE}/${id}/follow`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "Unfollow failed."));
    },
    getFollowers: async function (request: PageRequest): ApiPromise<Page<UserDto>> {
        return axios.get<ApiResponse<Page<UserDto>>>(`${BASE}/followers?page=${request.Page}&pageSize=${request.PageSize}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetFollowers failed."));
    },
    getFollowing: async function (request: PageRequest): ApiPromise<Page<UserDto>> {
        return axios.get<ApiResponse<Page<UserDto>>>(`${BASE}/following?page=${request.Page}&pageSize=${request.PageSize}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetFollowing failed."));
    }
}