export type ScheduleDto = {
    id: string,
    organizatorId: string,
    game: string,
    matchTime: Date,
    location?: string,
    status: string,
    matchId?: string,
    invitations: InvitationDto[]
}
export type InvitationDto = {
    playerId: string,
    status: string,
    respondedAt?: Date
} 