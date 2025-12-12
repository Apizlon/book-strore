namespace AuthService.Application.Contracts;

public class TokenValidationResponse
{
    public bool IsValid { get; set; }
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
    public List<string> Permissions { get; set; } = new();
}