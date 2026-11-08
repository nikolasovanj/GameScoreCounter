using Backend.Domain.Enums;

namespace Backend.Application.DTOs.Requests;

public record CreateScheduleRequest(GameType Game, DateTime MatchTime, string? Location, List<Guid>? InvitedPlayers);