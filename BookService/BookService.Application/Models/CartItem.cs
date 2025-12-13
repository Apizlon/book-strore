namespace BookService.Application.Models;

public class CartItem
{
    public string Id { get; set; } = null!;
    public string CartId { get; set; } = null!;
    public Cart Cart { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public Book Book { get; set; } = null!;
    public DateTime AddedAt { get; set; }
}