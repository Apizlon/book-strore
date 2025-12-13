using BookService.Application.Interfaces;
using BookService.Application.Models;

namespace BookService.Application.Services;

public class PermissionService : IPermissionService
{
    public bool HasPermission(UserClaimsDto claims, string requiredPermission)
    {
        return claims.Permissions.Contains(requiredPermission);
    }

    public bool HasAnyPermission(UserClaimsDto claims, params string[] requiredPermissions)
    {
        return requiredPermissions.Any(p => claims.Permissions.Contains(p));
    }

    public bool HasAllPermissions(UserClaimsDto claims, params string[] requiredPermissions)
    {
        return requiredPermissions.All(p => claims.Permissions.Contains(p));
    }
}