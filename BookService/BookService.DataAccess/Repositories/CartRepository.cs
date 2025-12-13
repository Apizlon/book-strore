using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace BookService.DataAccess.Repositories;

public class CartRepository : ICartRepository
{
    private readonly BookDbContext _dbContext;

    public CartRepository(BookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Cart?> GetByUserIdAsync(string userId)
    {
        return await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(ci => ci.Book)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        _dbContext.Carts.Add(cart);
        await _dbContext.SaveChangesAsync();
        return cart;
    }

    public async Task<Cart> UpdateAsync(Cart cart)
    {
        _dbContext.Carts.Update(cart);
        await _dbContext.SaveChangesAsync();
        return cart;
    }

    public async Task DeleteAsync(string id)
    {
        var cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.Id == id);
        if (cart != null)
        {
            _dbContext.Carts.Remove(cart);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<CartItem?> GetCartItemByIdAsync(string cartItemId)
    {
        return await _dbContext.CartItems
            .Include(ci => ci.Book)
            .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await GetByUserIdAsync(userId);
        if (cart != null)
        {
            cart.Items.Clear();
            cart.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(cart);
        }
    }
}