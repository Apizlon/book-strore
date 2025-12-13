namespace BookService.Application.Models;

public class Author
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
}