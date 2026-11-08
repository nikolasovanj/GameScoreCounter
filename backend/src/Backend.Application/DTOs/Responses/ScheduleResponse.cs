using Backend.Domain.Enums;

namespace Backend.Application.DTOs.Responses;

public record ScheduleResponse(
    Guid Id, 
    Guid OrganizatorId, 
    GameType Game,
    DateTime MatchTime,
    string? Location,
    ScheduleStatus Status,
    Guid? MatchId,
    IReadOnlyList<InvitationResponse> Invitations);