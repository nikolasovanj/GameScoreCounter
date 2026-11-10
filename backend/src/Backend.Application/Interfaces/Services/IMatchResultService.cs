using Backend.Application.Common;
using Backend.Application.DTOs.Responses;
using Backend.Domain.Enums;
using Backend.Domain.Result;

namespace Backend.Application.Interfaces.Services;

public interface IMatchResultService
{    
    Task<Result<PagedResult<MatchResultResponse>>> GetByUserIdForGameAsync(Guid userId, GameType game, PageRequest request, CancellationToken ct = default);
}