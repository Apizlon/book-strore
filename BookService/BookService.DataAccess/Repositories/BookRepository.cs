using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace BookService.DataAccess.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookDbContext _dbContext;

    public BookRepository(BookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Book?> GetByIdAsync(string id)
    {
        return await _dbContext.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _dbContext.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByAuthorIdAsync(string authorId)
    {
        return await _dbContext.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .Where(b => b.AuthorId == authorId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetByGenreAsync(BookGenre genre)
    {
        return await _dbContext.Books
            .Include(b => b.Author)
            .Include(b => b.Reviews)
            .Where(b => b.Genre == genre)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<Book> CreateAsync(Book book)
    {
        _dbContext.Books.Add(book);
        await _dbContext.SaveChangesAsync();
        return book;
    }

    public async Task<Book> UpdateAsync(Book book)
    {
        _dbContext.Books.Update(book);
        await _dbContext.SaveChangesAsync();
        return book;
    }

    public async Task DeleteAsync(string id)
    {
        var book = await GetByIdAsync(id);
        if (book != null)
        {
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }
    }
}
