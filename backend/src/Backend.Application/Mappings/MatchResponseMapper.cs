using Backend.Application.DTOs.Responses;
using Backend.Domain.Entities;

namespace Backend.Application.Mappings;

public static class MatchResponseMapper
{   
    public static MatchResultResponse ToResponse(this MatchResult result) => new(
        result.UserId,
        result.IsWinner,
        result.TotalScore,
        [.. result.ScoreComponents.Select(c => c.ToResponse())]
    );
}