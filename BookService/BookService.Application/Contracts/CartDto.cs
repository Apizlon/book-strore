namespace BookService.Application.Contracts;

public class AddToCartRequest
{
    public string BookId { get; set; } = null!;
}

public class CartItemDto
{
    public string Id { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public string BookTitle { get; set; } = null!;
    public decimal BookPrice { get; set; }
    public DateTime AddedAt { get; set; }
}

public class CartDto
{
    public string Id { get; set; } = null!;
    public IEnumerable<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    public decimal TotalPrice { get; set; }
    public DateTime UpdatedAt { get; set; }
}