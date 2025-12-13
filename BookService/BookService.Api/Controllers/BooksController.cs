using Microsoft.AspNetCore.Mvc;
using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;

namespace BookService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookApplicationService _bookService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPermissionService _permissionService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(
        IBookApplicationService bookService,
        IJwtTokenService jwtTokenService,
        IPermissionService permissionService,
        ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _jwtTokenService = jwtTokenService;
        _permissionService = permissionService;
        _logger = logger;
    }

    [HttpGet("{bookId}")]
    public async Task<BookDto> GetBook(string bookId)
    {
        _logger.LogInformation("Getting book - {BookId}", bookId);
        return await _bookService.GetBookByIdAsync(bookId);
    }

    [HttpGet]
    public async Task<IEnumerable<BookDto>> GetAllBooks()
    {
        _logger.LogInformation("Getting all books");
        return await _bookService.GetAllBooksAsync();
    }

    [HttpGet("by-author/{authorId}")]
    public async Task<IEnumerable<BookDto>> GetBooksByAuthor(string authorId)
    {
        _logger.LogInformation("Getting books by author - {AuthorId}", authorId);
        return await _bookService.GetBooksByAuthorAsync(authorId);
    }

    [HttpGet("by-genre/{genre}")]
    public async Task<IEnumerable<BookDto>> GetBooksByGenre(string genre)
    {
        _logger.LogInformation("Getting books by genre - {Genre}", genre);
        return await _bookService.GetBooksByGenreAsync(genre);
    }

    [HttpGet("top-rated")]
    public async Task<IEnumerable<BookDto>> GetTopRatedBooks([FromQuery] int take = 10)
    {
        _logger.LogInformation("Getting top rated books - Take: {Take}", take);
        return await _bookService.GetBooksByRatingAsync(take);
    }

    [HttpPost]
    public async Task<BookDto> CreateBook([FromBody] CreateBookRequest request)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.AddBook))
        {
            _logger.LogWarning("User {Username} denied permission to create books", claims.Username);
            throw new ForbiddenException("You do not have permission to create books");
        }

        _logger.LogInformation("Creating book - {Title} by user {UserId}", request.Title, claims.UserId);
        return await _bookService.CreateBookAsync(request, claims.UserId);
    }

    [HttpPut("{bookId}")]
    public async Task<BookDto> UpdateBook(string bookId, [FromBody] UpdateBookRequest request)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.EditBook))
        {
            _logger.LogWarning("User {Username} denied permission to edit books", claims.Username);
            throw new ForbiddenException("You do not have permission to edit books");
        }

        _logger.LogInformation("Updating book - {BookId} by user {UserId}", bookId, claims.UserId);
        return await _bookService.UpdateBookAsync(bookId, request, claims.UserId);
    }

    [HttpDelete("{bookId}")]
    public async Task DeleteBook(string bookId)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.DeleteBook))
        {
            _logger.LogWarning("User {Username} denied permission to delete books", claims.Username);
            throw new ForbiddenException("You do not have permission to delete books");
        }

        _logger.LogInformation("Deleting book - {BookId} by user {UserId}", bookId, claims.UserId);
        await _bookService.DeleteBookAsync(bookId, claims.UserId);
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
