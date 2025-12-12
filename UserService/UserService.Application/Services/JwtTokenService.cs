using System.Text;
using Microsoft.Extensions.Logging;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;

namespace UserService.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly ILogger<JwtTokenService> _logger;

    public JwtTokenService(ILogger<JwtTokenService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Extract user claims from Base64 encoded JWT token payload
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
        // Simple JSON parsing for JWT claims
        var userId = ExtractJsonValue(json, "sub") ?? ExtractJsonValue(json, "nameid");
        var username = ExtractJsonValue(json, "unique_name") ?? ExtractJsonValue(json, "name");
        var role = ExtractJsonValue(json, "role");
        var permissionsStr = ExtractJsonValue(json, "permissions");

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(username))
        {
            throw new InvalidOperationException("UserId or Username claim not found");
        }

        var permissions = string.IsNullOrEmpty(permissionsStr)
            ? new List<string>()
            : permissionsStr.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .ToList();

        return new UserClaimsDto
        {
            UserId = userId,
            Username = username,
            Role = role ?? "User",
            Permissions = permissions
        };
    }

    private static string? ExtractJsonValue(string json, string key)
    {
        // Look for "key":"value" pattern
        var pattern = $"\\\"{key}\\\":\\\"([^\\\"]+)\\\"";
        var match = System.Text.RegularExpressions.Regex.Match(json, pattern);

        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        // Try without quotes (for boolean/number values)
        pattern = $"\\\"{key}\\\":([^,}}]+)";
        match = System.Text.RegularExpressions.Regex.Match(json, pattern);

        return match.Success ? match.Groups[1].Value : null;
    }
}