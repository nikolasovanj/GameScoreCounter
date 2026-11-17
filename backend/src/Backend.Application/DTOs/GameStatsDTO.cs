using Backend.Domain.Enums;

namespace Backend.Application.DTOs;

public record GameStatsDTO(GameType Game, int MatchesPlayed, int MtchesWon, double? AveragePoints);