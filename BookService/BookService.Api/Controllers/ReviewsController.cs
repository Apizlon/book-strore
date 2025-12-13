using Microsoft.AspNetCore.Mvc;
using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;

namespace BookService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewApplicationService _reviewService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPermissionService _permissionService;
    private readonly ILogger<ReviewsController> _logger;

    public ReviewsController(
        IReviewApplicationService reviewService,
        IJwtTokenService jwtTokenService,
        IPermissionService permissionService,
        ILogger<ReviewsController> logger)
    {
        _reviewService = reviewService;
        _jwtTokenService = jwtTokenService;
        _permissionService = permissionService;
        _logger = logger;
    }

    [HttpGet("book/{bookId}")]
    public async Task<IEnumerable<ReviewDto>> GetReviewsByBook(string bookId)
    {
        _logger.LogInformation("Getting reviews for book - {BookId}", bookId);
        return await _reviewService.GetReviewsByBookAsync(bookId);
    }

    [HttpPost]
    public async Task<ReviewDto> CreateReview([FromBody] CreateReviewRequest request)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.AddReview))
        {
            _logger.LogWarning("User {Username} denied permission to add reviews", claims.Username);
            throw new ForbiddenException("You do not have permission to add reviews");
        }

        _logger.LogInformation("Creating review for book {BookId} by user {UserId}", request.BookId, claims.UserId);
        return await _reviewService.CreateReviewAsync(request, claims.UserId, claims.Username);
    }

    [HttpDelete("{reviewId}")]
    public async Task DeleteReview(string reviewId)
    {
        var claims = ExtractUserClaims();

        _logger.LogInformation("Deleting review - {ReviewId} by user {UserId}", reviewId, claims.UserId);
        await _reviewService.DeleteReviewAsync(reviewId, claims.UserId, claims.Username);
    }

    private UserClaimsDto ExtractUserClaims()
    {
        try
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                authHeader = HttpContext.Request.Headers["X-User-Token"].ToString();
            }

            if (string.IsNullOrEmpty(authHeader))
            {
                throw new BadRequestException("No authorization header");
            }

            return _jwtTokenService.ExtractClaimsFromToken(authHeader);
        }
        catch (BadRequestException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting user claims");
            throw new BadRequestException("Invalid token");
        }
    }
}
