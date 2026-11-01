using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities;

namespace Backend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByUserIdAsync(Guid id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
