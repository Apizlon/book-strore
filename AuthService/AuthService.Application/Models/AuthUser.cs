namespace AuthService.Application.Models;

public class AuthUser
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}