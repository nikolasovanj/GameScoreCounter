using Backend.Application.DTOs.Responses;
using Backend.Domain.Enums;
using Backend.Domain.Result;

namespace Backend.Application.Interfaces.Services;

public interface IMatchResultService
{    
    Task<Result<IReadOnlyList<MatchResultResponse>>> GetByUserIdForGameAsync(Guid userId, GameType game, CancellationToken ct = default);
}