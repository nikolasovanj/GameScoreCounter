using Backend.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Data;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, PageRequest request, CancellationToken ct = default)
    {
        var total = await query.CountAsync(ct);

        var items = await query
                    .Skip(request.Skip)
                    .Take(request.SafePageSize)
                    .ToListAsync(ct);
        return new PagedResult<T>(items, request.SafePage, request.SafePageSize, total);
    }
}