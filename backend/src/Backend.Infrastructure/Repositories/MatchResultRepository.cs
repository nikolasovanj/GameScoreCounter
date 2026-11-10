using Backend.Application.Common;
using Backend.Application.DTOs.Responses;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Enums;

namespace Backend.Infrastructure.Repositories;

public class MatchResultRepository : IMatchResultRepository
{
    public Task<PagedResult<MatchResultResponse>> GetForUserByGameAsync(Guid userId, GameType game, PageRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}