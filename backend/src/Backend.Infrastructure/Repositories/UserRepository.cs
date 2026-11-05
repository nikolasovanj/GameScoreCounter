using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var rows = await db.Users.Where(m => m.UserId.Equals(id)).ExecuteDeleteAsync(ct);
        return rows > 0;
    }

    public Task<User?> GetByUserIdAsync(Guid id, CancellationToken ct) => 
        db.Users.Where(u => u.UserId.Equals(id)).FirstOrDefaultAsync(ct);

    public Task<User?> GetByUsernameAsync(string username, CancellationToken ct) => 
        db.Users.Where(u => u.Username.Equals(username)).FirstOrDefaultAsync(ct);
}
