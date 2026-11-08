namespace Backend.Application.DTOs.Requests;

public record RescheduleRequest(DateTime MatchTime, string? Location);