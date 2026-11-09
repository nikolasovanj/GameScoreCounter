using Backend.Api.Extenstions;
using Backend.Application.Interfaces.Services;
using Backend.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/users/")]
[Authorize]
public class FollowController(IFollowService followService) : ControllerBase
{
    [HttpPut("{id}/follow")]
    public async Task<ActionResult> Follow([FromRoute]Guid id, CancellationToken ct)
    {
        var follower = User.GetUserId();
        var result = await followService.FollowAsync(follower, id, ct);
        return result.ToApiResult();
    }
    [HttpDelete("{id}/follow")]
    public async Task<ActionResult> Unfollow([FromRoute] Guid id, CancellationToken ct)
    {
        var follower = User.GetUserId();
        var result = await followService.UnfollowAsync(follower, id, ct);
        return result.ToApiResult();
    }
}