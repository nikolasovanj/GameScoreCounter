import axios from "axios";
import { apiErr, type ApiResponse, authHeader, type ApiPromise } from "../../helpers/api/api";
import type { MatchDto, PlayerMatchResultDto } from "../../models/MatchDtos";
import type { PageRequest, Page } from "../../types/Pagination";
import type { CreateMatchRequest } from "../../types/Requests";
import type { IMatchApiService } from "../interfaces/IMatchApiService";

const BASE = import.meta.env.VITE_API_URL + "match";

export const matchApi: IMatchApiService = {
    Create: async function (request: CreateMatchRequest): ApiPromise<MatchDto> {
        return axios.post<ApiResponse<MatchDto>>(`${BASE}`,
            request, 
            {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "Create failed."));
    },
    GetById: async function (id: string): ApiPromise<MatchDto> {
        return axios.get<ApiResponse<MatchDto>>(`${BASE}/${id}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetById failed."));
    },
    GetForUser: async function (userId: string, game: string, request: PageRequest): ApiPromise<Page<MatchDto>> {
        return axios.get<ApiResponse<Page<MatchDto>>>(`${BASE}/user/${userId}?game=${game}&page=${request.Page}&pageSize=${request.PageSize}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetForUser failed."));
    },
    Delete: async function (id: string): ApiPromise {
        return axios.delete(`${BASE}/${id}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "Delete failed."));
    },
    GetUsersResults: async function (game: string, request: PageRequest): ApiPromise<Page<PlayerMatchResultDto>> {
        return axios.get<ApiResponse<Page<PlayerMatchResultDto>>>(`${BASE}/me?game=${game}&page=${request.Page}&pageSize=${request.PageSize}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetUsersResults failed."));
    }
}