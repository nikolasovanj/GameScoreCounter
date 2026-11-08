namespace Backend.Application.Interfaces.Repositories;

public interface IFollowInterface
{
    Task AddIfMissingAsync(Guid me, Guid targer, CancellationToken ct = default);
    Task DeleteAsync(Guid me, Guid target, CancellationToken ct = default);
}