using BookService.Application.Models;

namespace BookService.Application.Interfaces;

public interface IPermissionService
{
    bool HasPermission(UserClaimsDto claims, string requiredPermission);
    bool HasAnyPermission(UserClaimsDto claims, params string[] requiredPermissions);
    bool HasAllPermissions(UserClaimsDto claims, params string[] requiredPermissions);
}