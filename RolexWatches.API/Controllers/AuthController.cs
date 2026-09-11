using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RolexWatches.Application.Common;
using RolexWatches.Application.Dto;
using RolexWatches.Application.ServiceInterface;

namespace RolexWatches.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            // Model validation errors are caught globally by ExceptionMiddleware
            // + [ApiController]'s automatic 400 response — no manual check needed here.
            var result = await authService.RegisterAsync(dto);
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Registration successful."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await authService.LoginAsync(dto);
            return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Login successful."));
        }
    }
}
