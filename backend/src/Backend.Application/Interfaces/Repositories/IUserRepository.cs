using Backend.Application.Common;
using Backend.Domain.Entities;

namespace Backend.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUserIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<User>> GetAllAsync(PageRequest request, CancellationToken ct = default);
}
