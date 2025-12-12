using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Application.Contracts;
using AuthService.Application.Exceptions;
using AuthService.Application.Interfaces;
using AuthService.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Application.Services;

public class JwtService : IJwtService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;
    private readonly ILogger<JwtService> _logger;

    public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
    {
        _key = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
        _issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
        _audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");
        _expirationMinutes = int.Parse(configuration["Jwt:ExpirationMinutes"] ?? "60");
        _logger = logger;
    }

    public TokenResponse GenerateToken(string userId, string username, UserRole role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var permissions = Permissions.GetPermissionsForRole(role);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, username),
            new("role", role.ToString()),
            new("isActive", "true")
        };

        // Add each permission as a separate claim
        foreach (var permission in permissions)
        {
            claims.Add(new Claim("permission", permission));
        }

        // Add serialized permissions list as a claim
        claims.Add(new Claim("permissions", string.Join(",", permissions)));

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        _logger.LogInformation("JWT token generated for user {Username}", username);

        return new TokenResponse
        {
            AccessToken = tokenString,
            RefreshToken = refreshToken,
            ExpiresIn = _expirationMinutes * 60
        };
    }

    public (string userId, string username, string role, List<string> permissions) ValidateAndExtractClaims(string token)
    {
        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                ?? throw new BadRequestException("UserId claim not found");
            
            var username = principal.FindFirst(ClaimTypes.Name)?.Value 
                ?? throw new BadRequestException("Username claim not found");
            
            var role = principal.FindFirst("role")?.Value 
                ?? throw new BadRequestException("Role claim not found");

            var permissionsString = principal.FindFirst("permissions")?.Value ?? string.Empty;
            var permissions = permissionsString.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            var isActiveClaim = principal.FindFirst("isActive")?.Value ?? "false";
            if (!bool.TryParse(isActiveClaim, out var isActive) || !isActive)
            {
                throw new BadRequestException("User is not active");
            }

            return (userId, username, role, permissions);
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token validation failed");
            throw new BadRequestException("Token validation failed");
        }
    }

    public bool ValidateToken(string token)
    {
        try
        {
            ValidateAndExtractClaims(token);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
