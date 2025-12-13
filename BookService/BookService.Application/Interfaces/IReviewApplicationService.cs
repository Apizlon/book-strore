using BookService.Application.Contracts;

namespace BookService.Application.Interfaces;

public interface IReviewApplicationService
{
    Task<ReviewDto> CreateReviewAsync(CreateReviewRequest request, string userId, string username);
    Task<IEnumerable<ReviewDto>> GetReviewsByBookAsync(string bookId);
    Task DeleteReviewAsync(string reviewId, string userId, string username);
}