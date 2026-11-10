using Backend.Domain.Enums;

namespace Backend.Application.DTOs;

public record PlayerMatchResultDTO(
    Guid MatchId,
    GameType Game, 
    DateTime PlayedAt,
    bool IsWinner,
    int? TotalScore,
    IReadOnlyList<ScoreComponentDTO> Components
);