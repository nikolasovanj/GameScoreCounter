using Backend.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Data;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, PageRequest request, CancellationToken ct = default)
    {
        var total = await query.CountAsync(ct);

        var items = await query
                    .Skip(request.GetSkip())
                    .Take(request.GetSafePageSize())
                    .ToListAsync(ct);
        return new PagedResult<T>(items, request.GetSafePage(), request.GetSafePageSize(), total);
    }
}