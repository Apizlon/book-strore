namespace UserService.Application.Contracts;

public class UpdateUserRequest
{
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
}