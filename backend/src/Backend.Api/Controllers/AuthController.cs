using Backend.Api.Contracts;
using Backend.Api.Extenstions;
using Backend.Application.DTOs.Requests;
using Backend.Application.DTOs.Responses;
using Backend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _auth = authService;
        
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request, CancellationToken ct) => 
            (await _auth.Login(request, ct)).ToApiResult();
        

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request, CancellationToken ct) => 
            (await _auth.Register(request, ct)).ToApiResult();
        
    }
}
