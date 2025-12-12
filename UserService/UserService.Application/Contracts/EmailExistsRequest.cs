namespace UserService.Application.Contracts;

public class EmailExistsRequest
{
    public string Email { get; set; } = string.Empty;
}