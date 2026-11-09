using Backend.Application.DTOs.Responses;
using Backend.Domain.Entities;

namespace Backend.Application.Mappings;

public static class ScheduleMapper
{
    public static ScheduleResponse ToResponse(this Schedule s) => new(
        s.ScheduleId,
        s.OrganizerId,
        s.Game,
        s.MatchTime,
        s.Location,
        s.Status,
        s.MatchId,
        [.. s.Invitations
            .OrderBy(i => i.PlayerId == s.OrganizerId ? 0 : 1)
            .Select(i => new InvitationResponse(i.PlayerId, i.Status, i.RespondedAt))]
    );
}