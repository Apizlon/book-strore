using Microsoft.AspNetCore.Mvc;
using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;

namespace BookService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderApplicationService _orderService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(
        IOrderApplicationService orderService,
        IJwtTokenService jwtTokenService,
        ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    [HttpPost("checkout")]
    public async Task<OrderDto> Checkout()
    {
        var claims = ExtractUserClaims();
        _logger.LogInformation("Checkout for user {UserId}", claims.UserId);
        return await _orderService.CheckoutAsync(claims.UserId);
    }

    [HttpGet("{orderId}")]
    public async Task<OrderDto> GetOrder(string orderId)
    {
        var claims = ExtractUserClaims();
        _logger.LogInformation("Getting order {OrderId} for user {UserId}", orderId, claims.UserId);
        return await _orderService.GetOrderByIdAsync(orderId, claims.UserId);
    }

    [HttpGet]
    public async Task<IEnumerable<OrderDto>> GetUserOrders()
    {
        var claims = ExtractUserClaims();
        _logger.LogInformation("Getting orders for user {UserId}", claims.UserId);
        return await _orderService.GetUserOrdersAsync(claims.UserId);
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
