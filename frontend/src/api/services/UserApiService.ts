import axios from "axios";
import { apiErr, type ApiResponse, authHeader, type ApiPromise } from "../../helpers/api/api";
import type { UserDto } from "../../models/UserDtos";
import type { PageRequest, Page } from "../../types/Pagination";
import type { IUserApiService } from "../interfaces/IUserApiService";

const BASE = import.meta.env.VITE_APU_URL + "user";

export const userApi : IUserApiService = {
    Delete: async function (id: string): ApiPromise {
        return axios.delete<ApiResponse<null>>(`${BASE}/delete/${id}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "Delete Failed."));
    },
    GetById: async function (id: string): ApiPromise<UserDto> {
        return axios.get<ApiResponse<UserDto>>(`${BASE}/${id}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetById Failed."));
    },
    GetByUserName: async function (username: string): ApiPromise<UserDto> {
        return axios.get<ApiResponse<UserDto>>(`${BASE}/${username}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetByUsername Failed."));
    },
    GetAll: async function (pageRequest: PageRequest): ApiPromise<Page<UserDto>> {
        return axios.get<ApiResponse<Page<UserDto>>>(`${BASE}?page=${pageRequest.Page}&pageSize=${pageRequest.PageSize}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetAll failed."));
    }
}