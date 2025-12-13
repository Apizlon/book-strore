namespace BookService.Application.Models;

public class Cart
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}