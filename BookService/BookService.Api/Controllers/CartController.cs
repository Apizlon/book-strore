using Microsoft.AspNetCore.Mvc;
using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;

namespace BookService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartApplicationService _cartService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<CartController> _logger;

    public CartController(
        ICartApplicationService cartService,
        IJwtTokenService jwtTokenService,
        ILogger<CartController> logger)
    {
        _cartService = cartService;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<CartDto> GetCart()
    {
        var claims = ExtractUserClaims();
        _logger.LogInformation("Getting cart for user {UserId}", claims.UserId);
        return await _cartService.GetCartAsync(claims.UserId);
    }

    [HttpPost("add")]
    public async Task<CartDto> AddToCart([FromBody] AddToCartRequest request)
    {
        var claims = ExtractUserClaims();
        _logger.LogInformation("Adding book {BookId} to cart for user {UserId}", request.BookId, claims.UserId);
        return await _cartService.AddToCartAsync(claims.UserId, request);
    }

    [HttpDelete("item/{cartItemId}")]
    public async Task<CartDto> RemoveFromCart(string cartItemId)
    {
        var claims = ExtractUserClaims();
        _logger.LogInformation("Removing item {CartItemId} from cart for user {UserId}", cartItemId, claims.UserId);
        return await _cartService.RemoveFromCartAsync(claims.UserId, cartItemId);
    }

    [HttpPost("clear")]
    public async Task ClearCart()
    {
        var claims = ExtractUserClaims();
        _logger.LogInformation("Clearing cart for user {UserId}", claims.UserId);
        await _cartService.ClearCartAsync(claims.UserId);
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
