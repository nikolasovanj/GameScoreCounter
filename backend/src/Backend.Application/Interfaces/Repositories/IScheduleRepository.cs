using Backend.Domain.Entities;

namespace Backend.Application.Interfaces.Repositories;

public interface IScheduleRepository
{
    Task AddAsync(Schedule schedule, CancellationToken ct = default);
    Task<Schedule?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Schedule>> ListForPlayerAsync(Guid playerId, DateTime fromUtc, CancellationToken ct = default);
}