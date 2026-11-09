using Backend.Api.Extenstions;
using Backend.Application;
using Backend.Application.Interfaces.Services;
using Backend.Application.Services;
using Backend.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _users = userService;
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] Guid id, CancellationToken ct) => 
        (await _users.DeleteAsync(id, ct)).ToApiResult();

    [HttpGet("id/{id}")]
    public async Task<ActionResult<ApiResponse<UserDTO>>> GetById([FromRoute]Guid id, CancellationToken ct) => 
        (await _users.GetByUserIdAsync(id, ct)).ToApiResult();

    [HttpGet("username/{username}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<UserDTO>>> GetByUsername([FromRoute] string username, CancellationToken ct) => 
        (await _users.GetByUsenameAsync(username, ct)).ToApiResult();
}
