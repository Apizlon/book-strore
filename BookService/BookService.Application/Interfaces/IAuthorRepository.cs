using BookService.Application.Models;

namespace BookService.Application.Interfaces;

public interface IAuthorRepository
{
    Task<Author?> GetByIdAsync(string id);
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author> CreateAsync(Author author);
    Task<Author> UpdateAsync(Author author);
    Task DeleteAsync(string id);
}