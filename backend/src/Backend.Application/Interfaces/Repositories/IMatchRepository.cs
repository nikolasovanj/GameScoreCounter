using Backend.Domain.Entities;
using Backend.Domain.Enums;

namespace Backend.Application.Interfaces.Repositories;

public interface IMatchRepository
{
    Task AddAsync (Match match, CancellationToken ct = default);
    Task<Match?> GetByMatchIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Match>> GetByUserIdForGameAsync(Guid userId, GameType game, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
