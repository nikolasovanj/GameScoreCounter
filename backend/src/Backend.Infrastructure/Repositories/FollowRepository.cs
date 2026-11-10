using Backend.Application.Common;
using Backend.Application.Interfaces.Repositories;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class FollowRepository(AppDbContext db) : IFollowRepository
{
    public async Task AddIfMissingAsync(Guid follower, Guid followee, CancellationToken ct = default)
    {
        await db.Database.ExecuteSqlInterpolatedAsync($""" 
            INSERT INTO follows ("follower_id", "followee_id", "created_at")
            VALUES ({follower}, {followee}, {DateTime.UtcNow})
            ON CONFLICT ("follower_id", "followee_id") DO NOTHING
            """ , ct);
    }

    public async Task<bool> DeleteAsync(Guid follower, Guid followee, CancellationToken ct = default)
    {
        int rows = await db.Follows.Where(f => f.FollowerId.Equals(follower) && f.FolloweeId.Equals(followee)).ExecuteDeleteAsync(ct);
        return rows > 0;
    }

    public Task<PagedResult<User>> GetFollowersAsync(Guid userId, PageRequest page, CancellationToken ct = default) => 
        db.Follows
            .Where(f => f.FolloweeId.Equals(userId))
            .OrderByDescending(f => f.CreatedAt).ThenBy(f => f.FollowerId)
            .Join(db.Users, f => f.FollowerId, u => u.UserId, (f, u) => u)
            .ToPagedResultAsync(page, ct);

    public Task<PagedResult<User>> GetFollowingAsync(Guid userId, PageRequest page, CancellationToken ct = default) =>
        db.Follows
            .Where(f => f.FollowerId.Equals(userId))
            .OrderByDescending(f => f.CreatedAt).ThenBy(f => f.FolloweeId)
            .Join(db.Users, f => f.FolloweeId, u => u.UserId, (f, u) => u)
            .ToPagedResultAsync(page, ct);
}