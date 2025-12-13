using BookService.Application.Contracts;

namespace BookService.Application.Interfaces;

public interface IBookApplicationService
{
    Task<BookDto> GetBookByIdAsync(string bookId);
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<IEnumerable<BookDto>> GetBooksByAuthorAsync(string authorId);
    Task<IEnumerable<BookDto>> GetBooksByGenreAsync(string genre);
    Task<IEnumerable<BookDto>> GetBooksByRatingAsync(int take = 10);
    Task<BookDto> CreateBookAsync(CreateBookRequest request, string userId);
    Task<BookDto> UpdateBookAsync(string bookId, UpdateBookRequest request, string userId);
    Task DeleteBookAsync(string bookId, string userId);
}