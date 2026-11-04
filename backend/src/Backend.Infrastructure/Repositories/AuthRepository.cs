using Backend.Application.DTOs.Requests;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class AuthRepository(AppDbContext db) : IAuthRepository
{
    public Task<User?> Login(LoginRequest request, CancellationToken ct = default) =>
        db.Users.FirstOrDefaultAsync(u => u.Username.Equals(request.Username) && u.PasswordHash.Equals(request.Password), ct);
    

    public async Task<User?> Register(User user, CancellationToken ct = default)
    {
        var saved = db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return saved.Entity;
    }
}
