namespace UserService.Application.Contracts;

public class UserClaimsDto
{
    public string UserId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
    public List<string> Permissions { get; set; } = new();
}