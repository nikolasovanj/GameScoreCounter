using Backend.Application.Interfaces.Repositories;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class UserLookup(AppDbContext db) : IUserLookup
{
    public async Task<bool> AllExistAsync(IReadOnlyCollection<Guid> ids, CancellationToken ct = default) => 
        await db.Users.CountAsync(u => ids.Contains(u.UserId), ct) == ids.Count;

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct = default) => 
        db.Users.AnyAsync(u => u.UserId.Equals(id), ct);
}