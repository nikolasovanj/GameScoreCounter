import type { ApiPromise } from "../../helpers/api/api";
import type { ScheduleDto } from "../../models/ScheduleDtos";
import type { Page, PageRequest } from "../../types/Pagination";
import type { CreateScheduleRequest, InviteRequest, RescheduleRequest } from "../../types/Requests";

export interface IScheduleService {
    Create(request: CreateScheduleRequest): ApiPromise<ScheduleDto>;
    GetForUser(request: PageRequest): ApiPromise<Page<ScheduleDto>>;
    GetById(id: string): ApiPromise<ScheduleDto>;
    InvitePlayers(id: string, request: InviteRequest): ApiPromise<ScheduleDto>;
    Respond(id: string, status: string): ApiPromise<ScheduleDto>;
    Reschedule(id: string, request: RescheduleRequest): ApiPromise<ScheduleDto>;
    Cancel(id: string): ApiPromise;
    LinkMatch(id: string, matchId: string): ApiPromise<ScheduleDto>;
}