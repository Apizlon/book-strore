using Microsoft.Extensions.Logging;
using UserService.Application.Contracts;
using UserService.Application.Interfaces;

namespace UserService.Application.Services;

public class PermissionService : IPermissionService
{
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(ILogger<PermissionService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Check if user has a specific permission
    /// </summary>
    public bool HasPermission(UserClaimsDto claims, string requiredPermission)
    {
        if (claims == null || claims.Permissions == null || claims.Permissions.Count == 0)
        {
            _logger.LogWarning("User {Username} has no permissions", claims?.Username ?? "Unknown");
            return false;
        }

        var hasPermission = claims.Permissions.Contains(requiredPermission, StringComparer.OrdinalIgnoreCase);

        if (!hasPermission)
        {
            _logger.LogWarning("User {Username} denied access - missing permission {Permission}", 
                claims.Username, requiredPermission);
        }

        return hasPermission;
    }

    /// <summary>
    /// Check if user has any of the required permissions
    /// </summary>
    public bool HasAnyPermission(UserClaimsDto claims, params string[] requiredPermissions)
    {
        if (claims == null || claims.Permissions == null || requiredPermissions == null || requiredPermissions.Length == 0)
        {
            return false;
        }

        return requiredPermissions.Any(p => claims.Permissions.Contains(p, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Check if user has all required permissions
    /// </summary>
    public bool HasAllPermissions(UserClaimsDto claims, params string[] requiredPermissions)
    {
        if (claims == null || claims.Permissions == null || requiredPermissions == null || requiredPermissions.Length == 0)
        {
            return false;
        }

        return requiredPermissions.All(p => claims.Permissions.Contains(p, StringComparer.OrdinalIgnoreCase));
    }
}