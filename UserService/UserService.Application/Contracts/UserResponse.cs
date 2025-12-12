using UserService.Application.Models;

namespace UserService.Application.Contracts;

public class UserResponse
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime RegistrationDate { get; set; }
    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
}