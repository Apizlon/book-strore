using Microsoft.AspNetCore.Mvc;
using UserService.Application.Attributes;
using UserService.Application.Contracts;
using UserService.Application.Exceptions;
using UserService.Application.Interfaces;
using UserService.Application.Models;

namespace UserService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserApplicationService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPermissionService _permissionService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(
        IUserApplicationService userService,
        IJwtTokenService jwtTokenService,
        IPermissionService permissionService,
        ILogger<UsersController> logger)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
        _permissionService = permissionService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<UserResponse> CreateUser([FromBody] CreateUserRequest request)
    {
        _logger.LogInformation("Creating user - {Username}", request.Username);
        return await _userService.CreateUserAsync(request);
    }

    [HttpGet("by-username/{username}")]
    public async Task<UserResponse> GetUserByUsername(string username)
    {
        _logger.LogInformation("Getting user by username - {Username}", username);
        var user = await _userService.GetUserByUsernameAsync(username);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        return user;
    }

    [HttpGet("email-exists/{email}")]
    public async Task<IActionResult> EmailExists(string email)
    {
        _logger.LogInformation("Checking email existence - {Email}", email);
        var exists = await _userService.EmailExistsAsync(email);
        return Ok(new { exists });
    }

    [HttpGet("{userId}")]
    public async Task<UserResponse> GetUser(string userId)
    {
        var claims = ExtractUserClaims();

        if (claims.UserId != userId && !_permissionService.HasPermission(claims, Permissions.ReadAllProfiles))
        {
            _logger.LogWarning("User {Username} attempted unauthorized access to user {UserId}", claims.Username,
                userId);
            throw new ForbiddenException("You do not have permission to access this user");
        }

        return await _userService.GetUserByIdAsync(userId);
    }

    [HttpGet]
    public async Task<IEnumerable<UserResponse>> GetAllUsers()
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.ReadAllProfiles))
        {
            _logger.LogWarning("User {Username} denied access to all users list", claims.Username);
            throw new ForbiddenException("You do not have permission to access all users");
        }

        return await _userService.GetAllUsersAsync();
    }

    [HttpPut("{userId}")]
    public async Task<UserResponse> UpdateUser(string userId, [FromBody] UpdateUserRequest request)
    {
        var claims = ExtractUserClaims();

        if (claims.UserId != userId && !_permissionService.HasPermission(claims, Permissions.UpdateAnyProfile))
        {
            _logger.LogWarning("User {Username} attempted unauthorized update of user {UserId}", claims.Username,
                userId);
            throw new ForbiddenException("You do not have permission to update this user");
        }

        return await _userService.UpdateUserAsync(userId, request);
    }

    [HttpPost("{userId}/deactivate")]
    [NeedPermissions(Permissions.BlockUser)]
    public async Task<UserResponse> DeactivateUser(string userId)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.BlockUser))
        {
            _logger.LogWarning("User {Username} denied permission to deactivate users", claims.Username);
            throw new ForbiddenException("You do not have permission to deactivate users");
        }

        return await _userService.DeactivateUserAsync(userId);
    }

    [HttpPost("{userId}/activate")]
    [NeedPermissions(Permissions.UnblockUser)]
    public async Task<UserResponse> ActivateUser(string userId)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.UnblockUser))
        {
            _logger.LogWarning("User {Username} denied permission to activate users", claims.Username);
            throw new ForbiddenException("You do not have permission to activate users");
        }

        return await _userService.ActivateUserAsync(userId);
    }

    [HttpPut("{userId}/role")]
    [NeedPermissions(Permissions.ManageRoles)]
    public async Task<UserResponse> ChangeUserRole(string userId, [FromBody] ChangeUserRoleRequest request)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.ManageRoles))
        {
            _logger.LogWarning("User {Username} denied permission to manage roles", claims.Username);
            throw new ForbiddenException("You do not have permission to manage roles");
        }

        return await _userService.ChangeUserRoleAsync(userId, request);
    }

    private UserClaimsDto ExtractUserClaims()
    {
        try
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                authHeader = HttpContext.Request.Headers["X-User-Token"].ToString();
            }

            if (string.IsNullOrEmpty(authHeader))
            {
                throw new BadRequestException("No authorization header");
            }

            return _jwtTokenService.ExtractClaimsFromToken(authHeader);
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting user claims");
            throw new BadRequestException("Invalid token");
        }
    }
}