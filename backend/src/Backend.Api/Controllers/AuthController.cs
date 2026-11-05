using Backend.Api.Extenstions;
using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Application.Interfaces.Services;
using Backend.Domain.Result.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _auth = authService;
        
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var loginResult = await _auth.Login(request, ct);
            return loginResult.ToApiResult();
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            var registerResult = await _auth.Register(request, ct);
            return registerResult.ToApiResult();
        }
    }
}
