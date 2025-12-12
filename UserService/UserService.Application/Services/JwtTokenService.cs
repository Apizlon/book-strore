using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;

namespace UserService.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly ILogger<JwtTokenService> _logger;
    private const string NameIdentifierClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    private const string NameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

    public JwtTokenService(ILogger<JwtTokenService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Extract user claims from JWT token payload
    /// </summary>
    public UserClaimsDto ExtractClaimsFromToken(string token)
    {
        try
        {
            // Remove "Bearer " prefix if present
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring("Bearer ".Length);
            }

            // JWT structure: header.payload.signature
            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                throw new InvalidOperationException("Invalid token format");
            }

            // Decode the payload (second part)
            var payload = parts[1];

            // Add padding if necessary
            var padded = payload.Length % 4 == 0 
                ? payload 
                : payload + new string('=', 4 - payload.Length % 4);

            var decodedBytes = Convert.FromBase64String(padded);
            var jsonPayload = Encoding.UTF8.GetString(decodedBytes);

            _logger.LogInformation("Decoded JWT payload: {Payload}", jsonPayload);

            // Parse JSON to extract claims
            var claims = ExtractClaimsFromJson(jsonPayload);

            return claims;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting claims from token");
            throw new InvalidOperationException("Failed to extract claims from token", ex);
        }
    }

    private static UserClaimsDto ExtractClaimsFromJson(string json)
    {
        try
        {
            using (var doc = JsonDocument.Parse(json))
            {
                var root = doc.RootElement;

                // Try different claim name variants
                string? userId = null;
                if (root.TryGetProperty(NameIdentifierClaim, out var uidElement))
                {
                    userId = uidElement.GetString();
                }
                else if (root.TryGetProperty("sub", out var subElement))
                {
                    userId = subElement.GetString();
                }
                else if (root.TryGetProperty("nameid", out var nameidElement))
                {
                    userId = nameidElement.GetString();
                }

                string? username = null;
                if (root.TryGetProperty(NameClaim, out var unElement))
                {
                    username = unElement.GetString();
                }
                else if (root.TryGetProperty("unique_name", out var uniqueElement))
                {
                    username = uniqueElement.GetString();
                }
                else if (root.TryGetProperty("name", out var nameElement))
                {
                    username = nameElement.GetString();
                }

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username))
                {
                    throw new InvalidOperationException($"UserId or Username claim not found. Available claims: {string.Join(", ", root.EnumerateObject().Select(p => p.Name))}");
                }

                // Extract role
                string? role = null;
                if (root.TryGetProperty("role", out var roleElement))
                {
                    role = roleElement.GetString();
                }

                // Extract permissions
                var permissions = new List<string>();
                if (root.TryGetProperty("permission", out var permElement))
                {
                    if (permElement.ValueKind == JsonValueKind.Array)
                    {
                        permissions = permElement.EnumerateArray()
                            .Select(p => p.GetString() ?? "")
                            .Where(p => !string.IsNullOrEmpty(p))
                            .ToList();
                    }
                }
                else if (root.TryGetProperty("permissions", out var permsElement))
                {
                    var permsStr = permsElement.GetString();
                    if (!string.IsNullOrEmpty(permsStr))
                    {
                        permissions = permsStr.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim())
                            .ToList();
                    }
                }

                return new UserClaimsDto
                {
                    UserId = userId,
                    Username = username,
                    Role = role ?? "User",
                    Permissions = permissions
                };
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to parse JWT payload: {ex.Message}", ex);
        }
    }
}