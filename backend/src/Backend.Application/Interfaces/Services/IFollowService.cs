using Backend.Application.Common;
using Backend.Application.DTOs;
using Backend.Domain.Result;

namespace Backend.Application.Interfaces.Services;

public interface IFollowService
{
    Task<Result> FollowAsync(Guid me, Guid target, CancellationToken ct = default);
    Task<Result> UnfollowAsync(Guid me, Guid target, CancellationToken ct = default);    
    Task<Result<PagedResult<UserDTO>>> GetFollowersAsync(Guid userId, PageRequest request, CancellationToken ct = default);
    Task<Result<PagedResult<UserDTO>>> GetFollowingAsync(Guid userId, PageRequest request, CancellationToken ct = default);
}