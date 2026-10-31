namespace Backend.Application.DTOs.Requests;

public record class MatchResultRequest(
    Guid PlayerId,
    bool IsWinner,
    int? TotalScore,
    List<ScoreComponentDTO>? Components
);
