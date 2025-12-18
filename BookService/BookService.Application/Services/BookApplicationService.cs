using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.Extensions.Logging;

namespace BookService.Application.Services;

public class BookApplicationService : IBookApplicationService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IClickHouseSender _clickHouseSender;
    private readonly ILogger<BookApplicationService> _logger;

    public BookApplicationService(
        IBookRepository bookRepository,
        IAuthorRepository authorRepository,
        IClickHouseSender clickHouseSender,
        ILogger<BookApplicationService> logger)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _clickHouseSender = clickHouseSender;
        _logger = logger;
    }

    public async Task<BookDto> GetBookByIdAsync(string bookId)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                throw new NotFoundException("Book not found");
            }

            // ✅ Метрика в ClickHouse
            await _clickHouseSender.SendEventAsync("BookById", "system", "Success", book.Title);

            return MapToDto(book);
        }
        catch (Exception ex) when (!(ex is NotFoundException))
        {
            await _clickHouseSender.SendEventAsync("BookById", "system", "Failure", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(MapToDto);
    }

    public async Task<IEnumerable<BookDto>> GetBooksByAuthorAsync(string authorId)
    {
        try
        {
            var author = await _authorRepository.GetByIdAsync(authorId);
            if (author == null)
            {
                throw new NotFoundException("Author not found");
            }

            var books = await _bookRepository.GetByAuthorIdAsync(authorId);
            
            // ✅ Метрика в ClickHouse
            await _clickHouseSender.SendEventAsync("BookByAuthorId", "system", "Success", author.Name);

            return books.Select(MapToDto);
        }
        catch (Exception ex) when (!(ex is NotFoundException))
        {
            await _clickHouseSender.SendEventAsync("BookByAuthorId", "system", "Failure", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<BookDto>> GetBooksByGenreAsync(string genre)
    {
        try
        {
            if (!Enum.TryParse<BookGenre>(genre, true, out var bookGenre))
            {
                throw new BadRequestException($"Invalid genre: {genre}");
            }

            var books = await _bookRepository.GetByGenreAsync(bookGenre);
            
            // ✅ Метрика в ClickHouse
            await _clickHouseSender.SendEventAsync("BookByGenre", "system", "Success", genre);

            return books.Select(MapToDto);
        }
        catch (Exception ex) when (!(ex is BadRequestException))
        {
            await _clickHouseSender.SendEventAsync("BookByGenre", "system", "Failure", ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<BookDto>> GetBooksByRatingAsync(int take = 10)
    {
        var books = await _bookRepository.GetAllAsync();
        
        var sortedBooks = books
            .Where(b => b.Reviews.Any())
            .OrderByDescending(b => b.Reviews.Average(r => r.Rating))
            .Take(take);

        return sortedBooks.Select(MapToDto);
    }

    public async Task<BookDto> CreateBookAsync(CreateBookRequest request, string userId)
    {
        try
        {
            var author = await _authorRepository.GetByIdAsync(request.AuthorId);
            if (author == null)
            {
                throw new NotFoundException("Author not found");
            }

            if (!Enum.TryParse<BookGenre>(request.Genre, true, out var genre))
            {
                throw new BadRequestException($"Invalid genre: {request.Genre}");
            }

            if (request.Price <= 0)
            {
                throw new BadRequestException("Price must be greater than 0");
            }

            var book = new Book
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                AuthorId = request.AuthorId,
                Genre = genre,
                PublishedDate = request.PublishedDate,
                CreatedAt = DateTime.UtcNow
            };

            await _bookRepository.CreateAsync(book);
            _logger.LogInformation("Book created - {BookId} by user {UserId}", book.Id, userId);

            return MapToDto(book);
        }
        catch (Exception ex) when (!(ex is NotFoundException || ex is BadRequestException))
        {
            throw;
        }
    }

    public async Task<BookDto> UpdateBookAsync(string bookId, UpdateBookRequest request, string userId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book == null)
        {
            throw new NotFoundException("Book not found");
        }

        var author = await _authorRepository.GetByIdAsync(request.AuthorId);
        if (author == null)
        {
            throw new NotFoundException("Author not found");
        }

        if (!Enum.TryParse<BookGenre>(request.Genre, true, out var genre))
        {
            throw new BadRequestException($"Invalid genre: {request.Genre}");
        }

        if (request.Price <= 0)
        {
            throw new BadRequestException("Price must be greater than 0");
        }

        book.Title = request.Title;
        book.Description = request.Description;
        book.Price = request.Price;
        book.AuthorId = request.AuthorId;
        book.Genre = genre;
        book.PublishedDate = request.PublishedDate;

        await _bookRepository.UpdateAsync(book);
        _logger.LogInformation("Book updated - {BookId} by user {UserId}", bookId, userId);

        return MapToDto(book);
    }

    public async Task DeleteBookAsync(string bookId, string userId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book == null)
        {
            throw new NotFoundException("Book not found");
        }

        await _bookRepository.DeleteAsync(bookId);
        _logger.LogInformation("Book deleted - {BookId} by user {UserId}", bookId, userId);
    }

    private static BookDto MapToDto(Book book)
    {
        var avgRating = book.Reviews.Any() 
            ? book.Reviews.Average(r => r.Rating) 
            : 0;

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            Price = book.Price,
            AuthorId = book.AuthorId,
            AuthorName = book.Author.Name,
            Genre = book.Genre.ToString(),
            PublishedDate = book.PublishedDate,
            AverageRating = avgRating,
            ReviewCount = book.Reviews.Count
        };
    }
}
