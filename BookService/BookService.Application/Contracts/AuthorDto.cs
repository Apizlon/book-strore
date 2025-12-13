namespace BookService.Application.Contracts;

public class CreateAuthorRequest
{
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
}

public class UpdateAuthorRequest
{
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
}

public class AuthorDto
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
    public int BookCount { get; set; }
}