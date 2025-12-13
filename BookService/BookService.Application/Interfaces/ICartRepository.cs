using BookService.Application.Models;

namespace BookService.Application.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(string userId);
    Task<Cart> CreateAsync(Cart cart);
    Task<Cart> UpdateAsync(Cart cart);
    Task DeleteAsync(string id);
    Task<CartItem?> GetCartItemByIdAsync(string cartItemId);
    Task ClearCartAsync(string userId);
}