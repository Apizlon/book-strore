using AuthService.Application.Contracts;

namespace AuthService.Application.Interfaces;

public interface IAuthApplicationService
{
    Task<TokenResponse> LoginAsync(LoginRequest request);
    Task<TokenResponse> RegisterAsync(RegisterRequest request);
    Task<TokenValidationResponse> ValidateTokenAsync(string token);
}