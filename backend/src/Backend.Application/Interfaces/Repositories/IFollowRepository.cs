namespace Backend.Application.Interfaces.Repositories;

public interface IFollowRepository
{
    Task AddIfMissingAsync(Guid follower, Guid followee, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid follower, Guid followee, CancellationToken ct = default);
}