namespace Backend.Application.DTOs.Responses;

public record ScoreComponentResponse(
    int Order,
    string? Label,
    int Points
);
