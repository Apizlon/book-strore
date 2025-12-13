using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.Extensions.Logging;

namespace BookService.Application.Services;

public class AuthorApplicationService : IAuthorApplicationService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<AuthorApplicationService> _logger;

    public AuthorApplicationService(
        IAuthorRepository authorRepository,
        IBookRepository bookRepository,
        ILogger<AuthorApplicationService> logger)
    {
        _authorRepository = authorRepository;
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<AuthorDto> GetAuthorByIdAsync(string authorId)
    {
        var author = await _authorRepository.GetByIdAsync(authorId);
        if (author == null)
        {
            throw new NotFoundException("Author not found");
        }

        return await MapToDtoAsync(author);
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        var authors = await _authorRepository.GetAllAsync();
        var dtos = new List<AuthorDto>();

        foreach (var author in authors)
        {
            dtos.Add(await MapToDtoAsync(author));
        }

        return dtos;
    }

    public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorRequest request, string userId)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new BadRequestException("Author name is required");
        }

        var author = new Author
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Bio = request.Bio,
            CreatedAt = DateTime.UtcNow
        };

        await _authorRepository.CreateAsync(author);
        _logger.LogInformation("Author created - {AuthorId} by user {UserId}", author.Id, userId);

        return await MapToDtoAsync(author);
    }

    public async Task<AuthorDto> UpdateAuthorAsync(string authorId, UpdateAuthorRequest request, string userId)
    {
        var author = await _authorRepository.GetByIdAsync(authorId);
        if (author == null)
        {
            throw new NotFoundException("Author not found");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new BadRequestException("Author name is required");
        }

        author.Name = request.Name;
        author.Bio = request.Bio;

        await _authorRepository.UpdateAsync(author);
        _logger.LogInformation("Author updated - {AuthorId} by user {UserId}", authorId, userId);

        return await MapToDtoAsync(author);
    }

    public async Task DeleteAuthorAsync(string authorId, string userId)
    {
        var author = await _authorRepository.GetByIdAsync(authorId);
        if (author == null)
        {
            throw new NotFoundException("Author not found");
        }

        await _authorRepository.DeleteAsync(authorId);
        _logger.LogInformation("Author deleted - {AuthorId} by user {UserId}", authorId, userId);
    }

    private async Task<AuthorDto> MapToDtoAsync(Author author)
    {
        var books = await _bookRepository.GetByAuthorIdAsync(author.Id);

        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Bio = author.Bio,
            BookCount = books.Count()
        };
    }
}
