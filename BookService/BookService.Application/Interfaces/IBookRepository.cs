using BookService.Application.Models;

namespace BookService.Application.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(string id);
    Task<IEnumerable<Book>> GetAllAsync();
    Task<IEnumerable<Book>> GetByAuthorIdAsync(string authorId);
    Task<IEnumerable<Book>> GetByGenreAsync(BookGenre genre);
    Task<Book> CreateAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task DeleteAsync(string id);
}