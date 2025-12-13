namespace BookService.Application.Contracts;

public class CreateReviewRequest
{
    public string BookId { get; set; } = null!;
    public int Rating { get; set; }
    public string Text { get; set; } = null!;
}

public class ReviewDto
{
    public string Id { get; set; } = null!;
    public string BookId { get; set; } = null!;
    public string Username { get; set; } = null!;
    public int Rating { get; set; }
    public string Text { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}