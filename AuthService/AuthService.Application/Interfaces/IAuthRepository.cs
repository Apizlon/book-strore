using AuthService.Application.Models;

namespace AuthService.Application.Interfaces;

public interface IAuthRepository
{
    Task<AuthUser?> GetAuthUserByIdAsync(string userId);
    Task<AuthUser?> GetAuthUserByUsernameAsync(string username);
    Task CreateAuthUserAsync(AuthUser authUser);
    Task UpdateAuthUserAsync(AuthUser authUser);
    Task DeleteAuthUserAsync(string userId);
}
