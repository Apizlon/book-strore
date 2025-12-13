using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.Extensions.Logging;

namespace BookService.Application.Services;

public class CartApplicationService : ICartApplicationService
{
    private readonly ICartRepository _cartRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<CartApplicationService> _logger;

    public CartApplicationService(
        ICartRepository cartRepository,
        IBookRepository bookRepository,
        ILogger<CartApplicationService> logger)
    {
        _cartRepository = cartRepository;
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<CartDto> GetCartAsync(string userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        
        if (cart == null)
        {
            // Create new cart if doesn't exist
            cart = new Cart
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            cart = await _cartRepository.CreateAsync(cart);
        }

        return MapToDto(cart);
    }

    public async Task<CartDto> AddToCartAsync(string userId, AddToCartRequest request)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId);
        if (book == null)
        {
            throw new NotFoundException("Book not found");
        }

        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            cart = await _cartRepository.CreateAsync(cart);
        }

        // Check if book already in cart
        if (cart.Items.Any(i => i.BookId == request.BookId))
        {
            throw new BadRequestException("Book already in cart");
        }

        var cartItem = new CartItem
        {
            Id = Guid.NewGuid().ToString(),
            CartId = cart.Id,
            BookId = request.BookId,
            AddedAt = DateTime.UtcNow
        };

        cart.Items.Add(cartItem);
        cart.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateAsync(cart);
        _logger.LogInformation("Book added to cart - BookId: {BookId}, UserId: {UserId}", request.BookId, userId);

        return MapToDto(cart);
    }

    public async Task<CartDto> RemoveFromCartAsync(string userId, string cartItemId)
    {
        var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
        if (cartItem == null)
        {
            throw new NotFoundException("Cart item not found");
        }

        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null || cartItem.CartId != cart.Id)
        {
            throw new ForbiddenException("Unauthorized");
        }

        cart.Items.Remove(cartItem);
        cart.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateAsync(cart);
        _logger.LogInformation("Item removed from cart - CartItemId: {CartItemId}, UserId: {UserId}", cartItemId, userId);

        return MapToDto(cart);
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            return;
        }

        cart.Items.Clear();
        cart.UpdatedAt = DateTime.UtcNow;

        await _cartRepository.UpdateAsync(cart);
        _logger.LogInformation("Cart cleared - UserId: {UserId}", userId);
    }

    private static CartDto MapToDto(Cart cart)
    {
        var items = cart.Items.Select(ci => new CartItemDto
        {
            Id = ci.Id,
            BookId = ci.BookId,
            BookTitle = ci.Book?.Title ?? string.Empty,
            BookPrice = ci.Book?.Price ?? 0,
            AddedAt = ci.AddedAt
        }).ToList();

        var totalPrice = items.Sum(i => i.BookPrice);

        return new CartDto
        {
            Id = cart.Id,
            Items = items,
            TotalPrice = totalPrice,
            UpdatedAt = cart.UpdatedAt
        };
    }
}
