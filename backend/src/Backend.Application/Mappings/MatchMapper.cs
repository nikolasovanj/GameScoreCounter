
using Backend.Application.DTOs.Responses;
using Backend.Domain.Entities;

namespace Backend.Application.Mappings;

public static class MatchMapper
{
    public static MatchResponse ToResponse(this Match match) => new(
            match.MatchId,
            match.Game,
            match.PlayedAt,
            [.. match.Results.Select(r => r.ToResponse())]
        );
}