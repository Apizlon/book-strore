using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.Extensions.Logging;

namespace BookService.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(ILogger<JwtTokenService> logger)
    {
        _logger = logger;
    }

    public UserClaimsDto ExtractClaimsFromToken(string token)
    {
        try
        {
            // Remove "Bearer " prefix if present
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring("Bearer ".Length).Trim();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new BadRequestException("Invalid token format");
            }

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                ?? throw new BadRequestException("UserId claim not found");

            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                ?? throw new BadRequestException("Username claim not found");

            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                ?? throw new BadRequestException("Role claim not found");

            var permissionsString = jwtToken.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value ?? string.Empty;
            var permissions = permissionsString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            return new UserClaimsDto
            {
                UserId = userId,
                Username = username,
                Role = role,
                Permissions = permissions
            };
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting claims from token");
            throw new BadRequestException("Invalid token");
        }
    }
}
