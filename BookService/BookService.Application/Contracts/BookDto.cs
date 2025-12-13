namespace BookService.Application.Contracts;

public class CreateBookRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string AuthorId { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public DateTime PublishedDate { get; set; }
}

public class UpdateBookRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string AuthorId { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public DateTime PublishedDate { get; set; }
}

public class BookDto
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string AuthorId { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public DateTime PublishedDate { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
}