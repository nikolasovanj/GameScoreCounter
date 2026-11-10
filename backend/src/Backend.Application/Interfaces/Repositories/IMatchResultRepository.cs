using Backend.Application.Common;
using Backend.Application.DTOs.Responses;
using Backend.Domain.Enums;

namespace Backend.Application.Interfaces.Repositories;

public interface IMatchResultRepository
{
    Task<PagedResult<MatchResultResponse>> GetForUserByGameAsync(Guid userId, GameType game, PageRequest request, CancellationToken ct = default);
}