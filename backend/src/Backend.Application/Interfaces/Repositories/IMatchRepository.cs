using Backend.Application.Common;
using Backend.Application.DTOs;
using Backend.Domain.Entities;
using Backend.Domain.Enums;

namespace Backend.Application.Interfaces.Repositories;

public interface IMatchRepository
{
    Task AddAsync (Match match, CancellationToken ct = default);
    Task<Match?> GetByMatchIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<Match>> GetByUserIdForGameAsync(Guid userId, GameType game, PageRequest page, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<PlayerMatchResultDTO>> PlayerResultByGameAsync(Guid userId, GameType game, PageRequest page, CancellationToken ct = default);
}
