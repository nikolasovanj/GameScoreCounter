using Backend.Domain.Enums;

namespace Backend.Application.DTOs.Responses;

public record MatchResponse(
    Guid Id,
    GameType Game,
    DateTime PlayedAt,
    IReadOnlyList<MatchResultResponse> Results
);
