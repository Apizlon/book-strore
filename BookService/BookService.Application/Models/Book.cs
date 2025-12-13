namespace BookService.Application.Models;

public class Book
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string AuthorId { get; set; } = null!;
    public Author Author { get; set; } = null!;
    public BookGenre Genre { get; set; }
    public DateTime PublishedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}