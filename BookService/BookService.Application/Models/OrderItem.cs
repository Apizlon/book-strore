namespace BookService.Application.Models;

public class OrderItem
{
    public string Id { get; set; } = null!;
    public string OrderId { get; set; } = null!;
    public Order Order { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public Book Book { get; set; } = null!;
    public decimal PriceAtPurchase { get; set; }
}