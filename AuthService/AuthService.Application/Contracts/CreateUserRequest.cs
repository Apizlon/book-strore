namespace AuthService.Application.Contracts;

public class CreateUserRequest
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
