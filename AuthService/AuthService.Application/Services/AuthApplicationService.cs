using AuthService.Application.Contracts;
using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Services;

public class AuthApplicationService : IAuthApplicationService
{
    private readonly IAuthRepository _authRepository;
    private readonly IJwtService _jwtService;
    private readonly IUserServiceClient _userServiceClient;
    private readonly ILogger<AuthApplicationService> _logger;

    public AuthApplicationService(
        IAuthRepository authRepository,
        IJwtService jwtService,
        IUserServiceClient userServiceClient,
        ILogger<AuthApplicationService> logger)
    {
        _authRepository = authRepository;
        _jwtService = jwtService;
        _userServiceClient = userServiceClient;
        _logger = logger;
    }

    public async Task<TokenResponse> LoginAsync(LoginRequest request)
    {
        var authUser = await _authRepository.GetAuthUserByUsernameAsync(request.Username);

        if (authUser == null)
        {
            _logger.LogWarning("Login failed: User not found - {Username}", request.Username);
            throw new BadRequestException("Invalid credentials");
        }

        if (!VerifyPassword(request.Password, authUser.PasswordHash))
        {
            _logger.LogWarning("Login failed: Invalid password - {Username}", request.Username);
            throw new BadRequestException("Invalid credentials");
        }

        if (!authUser.IsActive)
        {
            _logger.LogWarning("Login failed: User is inactive - {Username}", request.Username);
            throw new BadRequestException("User account is deactivated");
        }

        _logger.LogInformation("User logged in successfully - {Username}", request.Username);
        return _jwtService.GenerateToken(authUser.Id, authUser.Username, authUser.Role);
    }

    public async Task<TokenResponse> RegisterAsync(RegisterRequest request)
    {
        // Check if user already exists in auth service
        var existingAuthUser = await _authRepository.GetAuthUserByUsernameAsync(request.Username);
        if (existingAuthUser != null)
        {
            _logger.LogWarning("Registration failed: Username already exists - {Username}", request.Username);
            throw new BadRequestException("Username already exists");
        }

        // Check if user exists in UserService by username
        var existingUserByUsername = await _userServiceClient.GetUserByUsernameAsync(request.Username);
        if (existingUserByUsername != null)
        {
            _logger.LogWarning("Registration failed: Username already exists in UserService - {Username}", request.Username);
            throw new BadRequestException("Username already exists");
        }

        // Check if email already exists in UserService (if provided)
        if (!string.IsNullOrEmpty(request.Email))
        {
            var emailExists = await _userServiceClient.EmailExistsAsync(request.Email);
            if (emailExists)
            {
                _logger.LogWarning("Registration failed: Email already exists - {Email}", request.Email);
                throw new BadRequestException("Email already exists");
            }
        }

        // Create auth user
        var userId = Guid.NewGuid().ToString();
        var authUser = new AuthUser
        {
            Id = userId,
            Username = request.Username,
            PasswordHash = HashPassword(request.Password),
            IsActive = true,
            Role = UserRole.User,
            CreatedAt = DateTime.UtcNow
        };

        await _authRepository.CreateAuthUserAsync(authUser);
        _logger.LogInformation("Auth user created - {Username}", request.Username);

        // Create user profile in UserService
        try
        {
            var createUserRequest = new CreateUserRequest
            {
                Id = userId,
                Username = request.Username,
                Email = request.Email,
                DateOfBirth = request.DateOfBirth
            };

            await _userServiceClient.CreateUserAsync(createUserRequest);
            _logger.LogInformation("User profile created in UserService - {Username}", request.Username);
        }
        catch (Exception ex)
        {
            // If UserService fails, we should rollback the auth user creation
            _logger.LogError(ex, "Failed to create user profile in UserService, rolling back auth user");
            await _authRepository.DeleteAuthUserAsync(userId);
            throw new InvalidOperationException("Failed to complete registration");
        }

        _logger.LogInformation("User registered successfully - {Username}", request.Username);
        return _jwtService.GenerateToken(authUser.Id, authUser.Username, authUser.Role);
    }

    public async Task<TokenValidationResponse> ValidateTokenAsync(string token)
    {
        try
        {
            var (userId, username, role, permissions) = _jwtService.ValidateAndExtractClaims(token);

            // Double-check user is still active in AuthService
            var authUser = await _authRepository.GetAuthUserByIdAsync(userId);
            
            if (authUser == null || !authUser.IsActive)
            {
                return new TokenValidationResponse { IsValid = false };
            }

            return new TokenValidationResponse
            {
                IsValid = true,
                UserId = userId,
                Username = username,
                Role = role,
                Permissions = permissions
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token validation failed");
            return new TokenValidationResponse { IsValid = false };
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}
