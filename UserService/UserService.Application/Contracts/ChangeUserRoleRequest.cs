using UserService.Application.Models;

namespace UserService.Application.Contracts;

public class ChangeUserRoleRequest
{
    public UserRole NewRole { get; set; }
}