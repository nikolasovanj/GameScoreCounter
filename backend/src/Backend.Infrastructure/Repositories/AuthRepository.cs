using Backend.Application.DTOs.Requests;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class AuthRepository(AppDbContext db) : IAuthRepository
{
    public Task<User?> GetExisting(string username, CancellationToken ct = default) => db.Users.Where(u => u.Username.Equals(username)).FirstOrDefaultAsync(ct);

    public async Task<User?> Register(User user, CancellationToken ct = default)
    {
        var saved = await db.Users.AddAsync(user, ct);
        await db.SaveChangesAsync(ct);
        return saved.Entity;
    }
}
