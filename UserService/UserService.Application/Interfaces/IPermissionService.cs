using UserService.Application.Contracts;

namespace UserService.Application.Interfaces;

public interface IPermissionService
{
    bool HasPermission(UserClaimsDto claims, string requiredPermission);
    bool HasAnyPermission(UserClaimsDto claims, params string[] requiredPermissions);
    bool HasAllPermissions(UserClaimsDto claims, params string[] requiredPermissions);
}