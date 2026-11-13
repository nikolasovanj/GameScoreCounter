import axios from "axios";
import { type ApiResponse, type ApiPromise, authHeader, apiErr } from "../../helpers/api/api";
import type { ScheduleDto } from "../../models/ScheduleDtos";
import type { PageRequest, Page } from "../../types/Pagination";
import type { CreateScheduleRequest, InviteRequest, RescheduleRequest } from "../../types/Requests";
import type { IScheduleService } from "../interfaces/IScheduleService";

const BASE = import.meta.env.VITE_API_URL + "schedule";

export const scheduleApi : IScheduleService = {
    Create: async function (request: CreateScheduleRequest): ApiPromise<ScheduleDto> {
        return axios.post<ApiResponse<ScheduleDto>>(`${BASE}`, request, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "Create failed"));
    },
    GetForUser: async function (request: PageRequest): ApiPromise<Page<ScheduleDto>> {
        return axios.get<ApiResponse<Page<ScheduleDto>>>(`${BASE}?page=${request.Page}&pageSize=${request.PageSize}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetForUser failed"));
    },
    GetById: async function (id: string): ApiPromise<ScheduleDto> {
        return axios.get<ApiResponse<ScheduleDto>>(`${BASE}/${id}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "GetById failed."));
    },
    InvitePlayers: async function (id: string, request: InviteRequest): ApiPromise<ScheduleDto> {
        return axios.post<ApiResponse<ScheduleDto>>(`${BASE}/${id}/invitations`, {request}, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "InvitePlayers failed"));
    },
    Respond: async function (id: string, status: string): ApiPromise<ScheduleDto> {
        return axios.put<ApiResponse<ScheduleDto>>(`${BASE}/${id}/invitation/me`,  {status: status}, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "Respond failed."));
    },
    Reschedule: async function (id: string, request: RescheduleRequest): ApiPromise<ScheduleDto> {
        return axios.put<ApiResponse<ScheduleDto>>(`${BASE}/${id}`, request, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "Reschedule failed"));
    },
    Cancel: async function (id: string): ApiPromise {
        return axios.delete<ApiResponse<null>>(`${BASE}/${id}`, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "Cancel failed."));
    },
    LinkMatch: async function (id: string, matchId: string): ApiPromise<ScheduleDto> {
        return axios.put<ApiResponse<ScheduleDto>>(`${BASE}/${id}/match`, {matchId: matchId}, {headers: authHeader()})
            .then(r => r.data)
            .catch(e => apiErr(e, "LinkMatch failed."));
    }
}