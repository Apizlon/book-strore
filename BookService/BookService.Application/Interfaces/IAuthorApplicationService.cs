using BookService.Application.Contracts;

namespace BookService.Application.Interfaces;

public interface IAuthorApplicationService
{
    Task<AuthorDto> GetAuthorByIdAsync(string authorId);
    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
    Task<AuthorDto> CreateAuthorAsync(CreateAuthorRequest request, string userId);
    Task<AuthorDto> UpdateAuthorAsync(string authorId, UpdateAuthorRequest request, string userId);
    Task DeleteAuthorAsync(string authorId, string userId);
}