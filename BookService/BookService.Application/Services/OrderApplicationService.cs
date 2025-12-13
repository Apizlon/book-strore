using BookService.Application.Contracts;
using BookService.Application.Exceptions;
using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.Extensions.Logging;

namespace BookService.Application.Services;

public class OrderApplicationService : IOrderApplicationService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ILogger<OrderApplicationService> _logger;

    public OrderApplicationService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        ILogger<OrderApplicationService> logger)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _logger = logger;
    }

    public async Task<OrderDto> CheckoutAsync(string userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null || !cart.Items.Any())
        {
            throw new BadRequestException("Cart is empty");
        }

        var order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Status = OrderStatus.Completed,
            CreatedAt = DateTime.UtcNow,
            Items = new List<OrderItem>(),
            TotalPrice = 0
        };

        decimal totalPrice = 0;

        foreach (var cartItem in cart.Items)
        {
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = order.Id,
                BookId = cartItem.BookId,
                PriceAtPurchase = cartItem.Book!.Price
            };

            order.Items.Add(orderItem);
            totalPrice += cartItem.Book.Price;
        }

        order.TotalPrice = totalPrice;

        await _orderRepository.CreateAsync(order);
        await _cartRepository.ClearCartAsync(userId);

        _logger.LogInformation("Order created - {OrderId} for user {UserId}, total: {TotalPrice}", 
            order.Id, userId, totalPrice);

        return MapToDto(order);
    }

    public async Task<OrderDto> GetOrderByIdAsync(string orderId, string userId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            throw new NotFoundException("Order not found");
        }

        if (order.UserId != userId)
        {
            throw new ForbiddenException("Unauthorized");
        }

        return MapToDto(order);
    }

    public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);
        return orders.Select(MapToDto);
    }

    private static OrderDto MapToDto(Order order)
    {
        var items = order.Items.Select(oi => new OrderItemDto
        {
            Id = oi.Id,
            BookId = oi.BookId,
            BookTitle = oi.Book?.Title ?? string.Empty,
            PriceAtPurchase = oi.PriceAtPurchase
        }).ToList();

        return new OrderDto
        {
            Id = order.Id,
            TotalPrice = order.TotalPrice,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt,
            Items = items
        };
    }
}
