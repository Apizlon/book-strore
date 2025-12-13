using BookService.Application.Contracts;

namespace BookService.Application.Interfaces;

public interface ICartApplicationService
{
    Task<CartDto> GetCartAsync(string userId);
    Task<CartDto> AddToCartAsync(string userId, AddToCartRequest request);
    Task<CartDto> RemoveFromCartAsync(string userId, string cartItemId);
    Task ClearCartAsync(string userId);
}