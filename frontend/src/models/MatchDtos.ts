export type MatchDto = {
    id: string,
    game: string,
    playedAt: Date,
    results: MatchResultDto[]
}
export type MatchResultDto = {
    playerId: string,
    isWinner: boolean,
    totalScore?: number,
    components: ScoreComponentDto[]
}
export type ScoreComponentDto = {
    order: number,
    label?: string,
    points: number
}
export type PlayerMatchResultDto = {
    matchId: string,
    game: string,
    playedAt: Date,
    isWinner: boolean,
    totalScore?: number,
    components: ScoreComponentDto[]
}