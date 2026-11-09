using Backend.Application.Interfaces.Repositories;
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
}