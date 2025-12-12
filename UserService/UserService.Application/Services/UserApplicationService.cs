using Microsoft.Extensions.Logging;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;
using UserService.Application.Models;

namespace UserService.Application.Services;

public class UserApplicationService : IUserApplicationService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserApplicationService> _logger;

    public UserApplicationService(IUserRepository userRepository, ILogger<UserApplicationService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserResponse> GetUserByIdAsync(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            _logger.LogWarning("User not found - {UserId}", userId);
            throw new InvalidOperationException("User not found");
        }

        return MapToResponse(user);
    }

    public async Task<UserResponse> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        
        if (user == null)
        {
            _logger.LogWarning("User not found - {Username}", username);
            throw new InvalidOperationException("User not found");
        }

        return MapToResponse(user);
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(MapToResponse);
    }

    public async Task<UserResponse> UpdateUserAsync(string userId, UpdateUserRequest request)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            // Check if email is already taken by another user
            var emailExists = await _userRepository.EmailExistsAsync(request.Email, userId);
            if (emailExists)
            {
                throw new InvalidOperationException("Email already in use");
            }
            user.Email = request.Email;
        }

        if (request.DateOfBirth.HasValue)
        {
            user.DateOfBirth = request.DateOfBirth;
        }

        await _userRepository.UpdateUserAsync(user);

        _logger.LogInformation("User updated - {UserId}", userId);
        return MapToResponse(user);
    }

    public async Task<UserResponse> DeactivateUserAsync(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.IsActive = false;
        await _userRepository.UpdateUserAsync(user);

        _logger.LogInformation("User deactivated - {UserId}", userId);
        return MapToResponse(user);
    }

    public async Task<UserResponse> ActivateUserAsync(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.IsActive = true;
        await _userRepository.UpdateUserAsync(user);

        _logger.LogInformation("User activated - {UserId}", userId);
        return MapToResponse(user);
    }

    public async Task<UserResponse> ChangeUserRoleAsync(string userId, ChangeUserRoleRequest request)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        user.Role = request.NewRole;
        await _userRepository.UpdateUserAsync(user);

        _logger.LogInformation("User role changed to {Role} - {UserId}", request.NewRole, userId);
        return MapToResponse(user);
    }

    public async Task<UserResponse> CreateOrSyncUserAsync(User user)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(user.Id);

        if (existingUser == null)
        {
            await _userRepository.CreateUserAsync(user);
            _logger.LogInformation("User created - {UserId}", user.Id);
        }
        else
        {
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.Role = user.Role;
            existingUser.IsActive = user.IsActive;
            await _userRepository.UpdateUserAsync(existingUser);
            _logger.LogInformation("User synced - {UserId}", user.Id);
        }

        return MapToResponse(user);
    }
    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        // Проверь дублирование username
        var existingUser = await _userRepository.GetUserByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists");
        }

        // Проверь дублирование email
        if (!string.IsNullOrEmpty(request.Email))
        {
            var emailExists = await _userRepository.EmailExistsAsync(request.Email);
            if (emailExists)
            {
                throw new InvalidOperationException("Email already in use");
            }
        }

        var user = new User
        {
            Id = request.Id,
            Username = request.Username,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            RegistrationDate = DateTime.UtcNow,
            Role = UserRole.User,
            IsActive = true
        };

        await _userRepository.CreateUserAsync(user);
        _logger.LogInformation("User created via AuthService - {UserId}", user.Id);
    
        return MapToResponse(user);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userRepository.EmailExistsAsync(email);
    }

    private static UserResponse MapToResponse(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        DateOfBirth = user.DateOfBirth,
        RegistrationDate = user.RegistrationDate,
        Role = user.Role,
        IsActive = user.IsActive
    };
}
