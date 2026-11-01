using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Domain.Enums;

namespace Backend.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        public Task AddAsync(Match match, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Match?> GetByMatchIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Match>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Match>> GetByUserIdForGameAsync(Guid userId, GameType game, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}