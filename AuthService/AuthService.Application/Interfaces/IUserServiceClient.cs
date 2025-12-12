using AuthService.Application.Contracts;

namespace AuthService.Application.Interfaces;

public interface IUserServiceClient
{
    Task<UserServiceUserResponse?> GetUserByIdAsync(string userId);
    Task<UserServiceUserResponse?> GetUserByUsernameAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<UserServiceUserResponse> CreateUserAsync(CreateUserRequest request);
    Task<UserServiceUserResponse> UpdateUserAsync(string userId, UpdateUserRequest request);
}