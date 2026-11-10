using Backend.Api.Contracts;
using Backend.Api.Extenstions;
using Backend.Application.Common;
using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Application.Interfaces.Services;
using Backend.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MatchController(IMatchService matchService) : ControllerBase
{
    private readonly IMatchService _matches = matchService;
    [HttpPost]
    public async Task<ActionResult<ApiResponse<MatchResponse>>> Create([FromBody] CreateMatchRequest request, CancellationToken ct) => 
        (await _matches.CreateAsync(request, ct)).ToApiResult();
    
    [HttpGet("id/{id}")]
    public async Task<ActionResult<ApiResponse<MatchResponse>>> GetById([FromRoute] Guid id, CancellationToken ct) => 
        (await _matches.GetByIdAsync(id, ct)).ToApiResult();

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<MatchResponse>>>> GetForUser ([FromRoute] Guid userId, [FromQuery] GameType game, [FromQuery] PageRequest request, CancellationToken ct) => 
        (await _matches.GetForUserByGameAsync(userId, game, request, ct)).ToApiResult();

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute] Guid id, CancellationToken ct) => 
        (await _matches.DeleteAsync(id, ct)).ToApiResult();
}