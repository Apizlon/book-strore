namespace BookService.Application.Contracts;

public class OrderItemDto
{
    public string Id { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public string BookTitle { get; set; } = null!;
    public decimal PriceAtPurchase { get; set; }
}

public class OrderDto
{
    public string Id { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public IEnumerable<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
}