using UserService.Application.Contracts;
using UserService.Application.Models;

namespace UserService.Application.Interfaces;

public interface IUserApplicationService
{
    Task<UserResponse> GetUserByIdAsync(string userId);
    Task<UserResponse> GetUserByUsernameAsync(string username);
    Task<IEnumerable<UserResponse>> GetAllUsersAsync();
    Task<UserResponse> UpdateUserAsync(string userId, UpdateUserRequest request);
    Task<UserResponse> DeactivateUserAsync(string userId);
    Task<UserResponse> ActivateUserAsync(string userId);
    Task<UserResponse> ChangeUserRoleAsync(string userId, ChangeUserRoleRequest request);
    Task<UserResponse> CreateOrSyncUserAsync(User user);
}