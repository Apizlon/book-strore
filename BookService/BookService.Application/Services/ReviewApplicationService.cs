using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.Extensions.Logging;

namespace BookService.Application.Services;

public class ReviewApplicationService : IReviewApplicationService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IClickHouseSender _clickHouseSender;
    private readonly ILogger<ReviewApplicationService> _logger;

    public ReviewApplicationService(
        IReviewRepository reviewRepository,
        IBookRepository bookRepository,
        IClickHouseSender clickHouseSender,
        ILogger<ReviewApplicationService> logger)
    {
        _reviewRepository = reviewRepository;
        _bookRepository = bookRepository;
        _clickHouseSender = clickHouseSender;
        _logger = logger;
    }

    public async Task<ReviewDto> CreateReviewAsync(CreateReviewRequest request, string userId, string username)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
            {
                throw new NotFoundException("Book not found");
            }

            if (request.Rating < 1 || request.Rating > 5)
            {
                throw new BadRequestException("Rating must be between 1 and 5");
            }

            if (string.IsNullOrWhiteSpace(request.Text))
            {
                throw new BadRequestException("Review text is required");
            }

            var review = new Review
            {
                Id = Guid.NewGuid().ToString(),
                BookId = request.BookId,
                Username = username,
                Rating = request.Rating,
                Text = request.Text,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.CreateAsync(review);
            _logger.LogInformation("Review created - {ReviewId} by user {UserId}", review.Id, userId);

            // ✅ Метрика в ClickHouse
            await _clickHouseSender.SendEventAsync("ReviewCreated", userId, "Success", book.Title);

            return MapToDto(review);
        }
        catch (Exception ex) when (!(ex is NotFoundException || ex is BadRequestException))
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            await _clickHouseSender.SendEventAsync("ReviewCreated", userId, "Failure", book?.Title ?? "Unknown");
            throw;
        }
    }

    public async Task<IEnumerable<ReviewDto>> GetReviewsByBookAsync(string bookId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book == null)
        {
            throw new NotFoundException("Book not found");
        }

        var reviews = await _reviewRepository.GetByBookIdAsync(bookId);
        return reviews.Select(MapToDto);
    }

    public async Task DeleteReviewAsync(string reviewId, string userId, string username)
    {
        var review = await _reviewRepository.GetByIdAsync(reviewId);
        if (review == null)
        {
            throw new NotFoundException("Review not found");
        }

        if (review.Username != username)
        {
            throw new ForbiddenException("You can only delete your own reviews");
        }

        await _reviewRepository.DeleteAsync(reviewId);
        _logger.LogInformation("Review deleted - {ReviewId} by user {UserId}", reviewId, userId);
    }

    private static ReviewDto MapToDto(Review review)
    {
        return new ReviewDto
        {
            Id = review.Id,
            BookId = review.BookId,
            Username = review.Username,
            Rating = review.Rating,
            Text = review.Text,
            CreatedAt = review.CreatedAt
        };
    }
}
