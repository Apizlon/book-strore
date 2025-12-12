using AuthService.Application.Contracts;
using AuthService.Application.Models;

namespace AuthService.Application.Interfaces;

public interface IJwtService
{
    TokenResponse GenerateToken(string userId, string username, UserRole role);
    (string userId, string username, string role, List<string> permissions) ValidateAndExtractClaims(string token);
    bool ValidateToken(string token);
}