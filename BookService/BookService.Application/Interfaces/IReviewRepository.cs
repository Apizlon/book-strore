using BookService.Application.Models;

namespace BookService.Application.Interfaces;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(string id);
    Task<IEnumerable<Review>> GetByBookIdAsync(string bookId);
    Task<Review> CreateAsync(Review review);
    Task DeleteAsync(string id);
}