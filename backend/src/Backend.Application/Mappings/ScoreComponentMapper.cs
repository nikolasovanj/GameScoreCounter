using Backend.Application.DTOs.Responses;
using Backend.Domain.Entities;

namespace Backend.Application.Mappings;

public static class ScoreComponentMapper
{
    public static ScoreComponentResponse ToResponse(this ScoreComponent component) => new ( 
        component.Order,
        component.Label,
        component.Points
    );
}