namespace Backend.Application.Interfaces.Repositories;

public interface IUserLookup
{
    Task<bool> AllExistAsync(IReadOnlyCollection<Guid> ids, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}