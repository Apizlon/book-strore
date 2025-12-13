namespace AuthService.Application.Contracts;

public class UserResponse
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool IsActive { get; set; }
    public List<string> Permissions { get; set; } = new();
}