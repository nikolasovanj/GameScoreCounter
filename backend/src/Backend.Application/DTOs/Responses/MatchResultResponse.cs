namespace Backend.Application.DTOs.Responses;

public record MatchResultResponse(
    Guid PlayerId,
    bool IsWinner,
    int? TotalScore,
    IReadOnlyList<ScoreComponentResponse> Components
);
