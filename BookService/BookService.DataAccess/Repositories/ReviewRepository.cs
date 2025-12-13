using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace BookService.DataAccess.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly BookDbContext _dbContext;

    public ReviewRepository(BookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Review?> GetByIdAsync(string id)
    {
        return await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Review>> GetByBookIdAsync(string bookId)
    {
        return await _dbContext.Reviews
            .Where(r => r.BookId == bookId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<Review> CreateAsync(Review review)
    {
        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();
        return review;
    }

    public async Task DeleteAsync(string id)
    {
        var review = await GetByIdAsync(id);
        if (review != null)
        {
            _dbContext.Reviews.Remove(review);
            await _dbContext.SaveChangesAsync();
        }
    }
}