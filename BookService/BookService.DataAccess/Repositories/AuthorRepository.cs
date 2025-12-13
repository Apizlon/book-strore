using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace BookService.DataAccess.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly BookDbContext _dbContext;

    public AuthorRepository(BookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Author?> GetByIdAsync(string id)
    {
        return await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _dbContext.Authors
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<Author> CreateAsync(Author author)
    {
        _dbContext.Authors.Add(author);
        await _dbContext.SaveChangesAsync();
        return author;
    }

    public async Task<Author> UpdateAsync(Author author)
    {
        _dbContext.Authors.Update(author);
        await _dbContext.SaveChangesAsync();
        return author;
    }

    public async Task DeleteAsync(string id)
    {
        var author = await GetByIdAsync(id);
        if (author != null)
        {
            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync();
        }
    }
}