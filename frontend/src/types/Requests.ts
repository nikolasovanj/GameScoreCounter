import type { ScoreComponentDto } from "../models/MatchDtos";

export type LoginRequest = { username: string, password: string };

export type RegisterRequest = { username: string, password: string, passwordRepeat: string};

export type CreateMatchRequest = {game: string, playedAt: Date, results: MatchResultRequest[]}

export type MatchResultRequest = {playerId: string, isWinner: boolean, totalScore?: number, components?: ScoreComponentDto[]}

export type CreateScheduleRequest = {game: string, matchTime: Date, location?: string, invitedPlayerIds?: string[]}

export type InviteRequest = {playerIds: string[]}
export type RescheduleRequest = {matchTime: Date, location?: string}