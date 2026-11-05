using Backend.Domain.Entities;

namespace Backend.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUserIdAsync(Guid id, CancellationToken ct);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
