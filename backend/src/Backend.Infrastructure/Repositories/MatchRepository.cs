using System.Collections.Immutable;
using Backend.Application.Common;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories
{
    public class MatchRepository(AppDbContext db) : IMatchRepository
    {
        public async Task AddAsync(Match match, CancellationToken ct = default)
        {
            db.Matches.Add(match);
            await db.SaveChangesAsync(ct);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var rows = await db.Matches.Where(m => m.MatchId.Equals(id)).ExecuteDeleteAsync(ct);
            return rows > 0;
        }

        public Task<Match?> GetByMatchIdAsync(Guid id, CancellationToken ct = default) =>
            db.Matches
              .Include(m => m.Results).ThenInclude(r => r.ScoreComponents)
              .AsSplitQuery()
              .FirstOrDefaultAsync(m => m.MatchId.Equals(id), ct);


        public async Task<IReadOnlyList<Match>> GetByUserIdForGameAsync(Guid userId, GameType game, CancellationToken ct = default) =>
                await db.Matches
                    .Include(m => m.Results).ThenInclude(r => r.ScoreComponents)
                    .AsSplitQuery()
                    .Where(m => m.Game == game && m.Results.Any(r => r.UserId.Equals(userId)))
                    .ToListAsync(ct);

        public Task<PagedResult<Match>> GetByUserIdForGameAsync(Guid userId, GameType game, PageRequest request, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}