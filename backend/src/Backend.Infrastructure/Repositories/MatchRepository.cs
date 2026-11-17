using System.Collections.Immutable;
using Backend.Application.Common;
using Backend.Application.DTOs;
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


        public Task<PagedResult<Match>> GetByUserIdForGameAsync(Guid userId, GameType game, PageRequest page, CancellationToken ct = default) =>
                db.Matches
                    .Include(m => m.Results).ThenInclude(r => r.ScoreComponents)
                    .AsSplitQuery()
                    .Where(m => m.Game == game && m.Results.Any(r => r.UserId.Equals(userId)))
                    .ToPagedResultAsync(page, ct);

        public async Task<List<GameStatsDTO>> GetPlayerStatsAsync(Guid userId, CancellationToken ct = default)
        {
            var rows = await db.MatchResults.AsNoTracking()
                                .Where(r => r.UserId.Equals(userId))
                                .Join(db.Matches, r => r.MatchId, m => m.MatchId, (r, m) => new {m.Game, r.IsWinner, r.TotalScore})
                                .GroupBy(x => x.Game)
                                .Select(g => new
                                {
                                    Game = g.Key,
                                    Played = g.Count(),
                                    Won = g.Count(x => x.IsWinner),
                                    Average = g.Average(x => (double?) x.TotalScore)
                                })
                                .ToListAsync(ct);

            return [.. rows.OrderBy(r => r.Game.ToString())
                            .Select(r => new GameStatsDTO(r.Game, r.Played, r.Won, r.Average))];
            
        }

        public Task<PagedResult<PlayerMatchResultDTO>> PlayerResultByGameAsync(Guid userId, GameType game, PageRequest page, CancellationToken ct = default) =>
            db.MatchResults.AsNoTracking()
                .Where(r => r.UserId.Equals(userId))
                .Join(db.Matches.Where(m => m.Game == game), 
                r => r.MatchId,
                m => m.MatchId,
                (r, m) => new { Result = r, Match = m})
                .OrderByDescending(x => x.Match.PlayedAt)
                .ThenBy(x => x.Match.MatchId)
                .Select(x => new PlayerMatchResultDTO(
                    x.Match.MatchId,
                    x.Match.Game,
                    x.Match.PlayedAt,
                    x.Result.IsWinner,
                    x.Result.TotalScore,
                    x.Result.ScoreComponents
                        .OrderBy(c => c.Order)
                        .Select(c => new ScoreComponentDTO(c.Order, c.Label, c.Points))
                        .ToList()
                )).ToPagedResultAsync(page, ct);
    }
}