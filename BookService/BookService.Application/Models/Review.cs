namespace BookService.Application.Models;

public class Review
{
    public string Id { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public Book Book { get; set; } = null!;
    public string Username { get; set; } = null!;
    public int Rating { get; set; } // 1-5
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}