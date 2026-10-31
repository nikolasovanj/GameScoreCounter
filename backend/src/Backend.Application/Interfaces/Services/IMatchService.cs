using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Domain.Enums;

namespace Backend.Application.Interfaces.Services;

public interface IMatchService
{
    Task<MatchResponse> CreateAsync(CreateMatchRequest request, CancellationToken ct = default);
    Task<MatchResponse> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<MatchResponse>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<MatchResponse>> GetByUserIdForGameAsync(Guid userId, GameType game, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}