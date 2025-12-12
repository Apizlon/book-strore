using AuthService.Application.Contracts;
using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthApplicationService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthApplicationService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet("health")]
    public object Health()
    {
        return new { status = "healthy", service = "AuthService" };
    }

    [HttpPost("login")]
    public async Task<TokenResponse> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login attempt for user {Username}", request.Username);
        return await _authService.LoginAsync(request);
    }

    [HttpPost("register")]
    public async Task<TokenResponse> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Registration attempt for user {Username}", request.Username);
        return await _authService.RegisterAsync(request);
    }

    [HttpPost("validate")]
    public async Task<TokenValidationResponse> ValidateToken()
    {
        var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            throw new Application.Exceptions.BadRequestException("No authorization header");
        }

        var token = authHeader.Substring("Bearer ".Length);
        return await _authService.ValidateTokenAsync(token);
    }
}