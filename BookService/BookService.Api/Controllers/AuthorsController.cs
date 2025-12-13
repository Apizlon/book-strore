using Microsoft.AspNetCore.Mvc;
using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;

namespace BookService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorApplicationService _authorService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPermissionService _permissionService;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(
        IAuthorApplicationService authorService,
        IJwtTokenService jwtTokenService,
        IPermissionService permissionService,
        ILogger<AuthorsController> logger)
    {
        _authorService = authorService;
        _jwtTokenService = jwtTokenService;
        _permissionService = permissionService;
        _logger = logger;
    }

    [HttpGet("{authorId}")]
    public async Task<AuthorDto> GetAuthor(string authorId)
    {
        _logger.LogInformation("Getting author - {AuthorId}", authorId);
        return await _authorService.GetAuthorByIdAsync(authorId);
    }

    [HttpGet]
    public async Task<IEnumerable<AuthorDto>> GetAllAuthors()
    {
        _logger.LogInformation("Getting all authors");
        return await _authorService.GetAllAuthorsAsync();
    }

    [HttpPost]
    public async Task<AuthorDto> CreateAuthor([FromBody] CreateAuthorRequest request)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.AddAuthor))
        {
            _logger.LogWarning("User {Username} denied permission to create authors", claims.Username);
            throw new ForbiddenException("You do not have permission to create authors");
        }

        _logger.LogInformation("Creating author - {Name} by user {UserId}", request.Name, claims.UserId);
        return await _authorService.CreateAuthorAsync(request, claims.UserId);
    }

    [HttpPut("{authorId}")]
    public async Task<AuthorDto> UpdateAuthor(string authorId, [FromBody] UpdateAuthorRequest request)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.EditAuthor))
        {
            _logger.LogWarning("User {Username} denied permission to edit authors", claims.Username);
            throw new ForbiddenException("You do not have permission to edit authors");
        }

        _logger.LogInformation("Updating author - {AuthorId} by user {UserId}", authorId, claims.UserId);
        return await _authorService.UpdateAuthorAsync(authorId, request, claims.UserId);
    }

    [HttpDelete("{authorId}")]
    public async Task DeleteAuthor(string authorId)
    {
        var claims = ExtractUserClaims();

        if (!_permissionService.HasPermission(claims, Permissions.DeleteAuthor))
        {
            _logger.LogWarning("User {Username} denied permission to delete authors", claims.Username);
            throw new ForbiddenException("You do not have permission to delete authors");
        }

        _logger.LogInformation("Deleting author - {AuthorId} by user {UserId}", authorId, claims.UserId);
        await _authorService.DeleteAuthorAsync(authorId, claims.UserId);
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
