namespace BookService.Application.Models;

public class Order
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}