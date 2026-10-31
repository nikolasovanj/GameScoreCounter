using Backend.Domain.Enums;

namespace Backend.Application.DTOs.Requests;

public record CreateMatchRequest(GameType Game, DateTime PlayedAt, List<MatchResultRequest> Results);
