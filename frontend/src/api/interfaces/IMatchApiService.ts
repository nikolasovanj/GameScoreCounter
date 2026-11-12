import type { ApiPromise } from "../../helpers/api/api";
import type { MatchDto, PlayerMatchResultDto } from "../../models/MatchDtos";
import type { Page, PageRequest } from "../../types/Pagination";
import type { CreateMatchRequest } from "../../types/Requests";

export interface IMatchApiService {
    Create(request: CreateMatchRequest): ApiPromise<MatchDto>;
    GetById(id: string): ApiPromise<MatchDto>;
    GetForUser(userId: string, game: string, request: PageRequest): ApiPromise<Page<MatchDto>>;
    Delete(id: string): ApiPromise;
    GetUsersResults(game: string, request: PageRequest): ApiPromise<Page<PlayerMatchResultDto>>;
}