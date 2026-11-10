using Backend.Application.Common;
using Backend.Domain.Entities;

namespace Backend.Application.Interfaces.Repositories;

public interface IFollowRepository
{
    Task AddIfMissingAsync(Guid follower, Guid followee, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid follower, Guid followee, CancellationToken ct = default);
    Task<PagedResult<User>> GetFollowersAsync(Guid userId, PageRequest request, CancellationToken ct = default);
    Task<PagedResult<User>> GetFollowingAsync(Guid userId, PageRequest request, CancellationToken ct = default);
}