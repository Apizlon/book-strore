namespace AuthService.Application.Contracts;

public class UserServiceUserResponse
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Role { get; set; } = null!;
    public bool IsActive { get; set; }
}