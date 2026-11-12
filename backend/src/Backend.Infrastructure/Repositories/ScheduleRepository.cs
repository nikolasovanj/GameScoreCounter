using Backend.Application.Common;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Domain.Result;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Backend.Infrastructure.Repositories;

public class ScheduleRepository(AppDbContext db) : IScheduleRepository
{
    public async Task AddAsync(Schedule schedule, CancellationToken ct = default)
    {
        db.Schedules.Add(schedule);
        await db.SaveChangesAsync(ct);
    }

    public Task<Schedule?> GetByIdAsync(Guid id, CancellationToken ct = default) => 
        db.Schedules.Include(s => s.Invitations).FirstOrDefaultAsync(s => s.ScheduleId.Equals(id), ct);

    public async Task<PagedResult<Schedule>> ListForPlayerAsync(Guid playerId, DateTime fromUtc, PageRequest request, CancellationToken ct = default) => 
        await db.Schedules.AsNoTracking()
          .Include(s => s.Invitations)
          .Where(s => s.Status == Domain.Enums.ScheduleStatus.SCHEDULED
                    && s.MatchTime >= fromUtc
                    && s.Invitations.Any(i => i.PlayerId.Equals(playerId)))
          .OrderBy(s => s.MatchTime)
          .ToPagedResultAsync(request, ct);

    public async Task<bool> SaveChangesAsync(CancellationToken ct)
    {
        try
        {
            await db.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException {SqlState: PostgresErrorCodes.UniqueViolation})
        {
            return false;
        }
    }
}