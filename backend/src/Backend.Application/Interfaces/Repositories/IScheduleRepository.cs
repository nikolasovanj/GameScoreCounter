using Backend.Application.Common;
using Backend.Domain.Entities;
using Backend.Domain.Result;

namespace Backend.Application.Interfaces.Repositories;

public interface IScheduleRepository
{
    Task AddAsync(Schedule schedule, CancellationToken ct = default);
    Task<Schedule?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<Schedule>> ListForPlayerAsync(Guid playerId, DateTime fromUtc, PageRequest request, CancellationToken ct = default);
    Task<bool> SaveChangesAsync(CancellationToken ct);
}