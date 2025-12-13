using BookService.Application.Contracts;

namespace BookService.Application.Interfaces;

public interface IOrderApplicationService
{
    Task<OrderDto> CheckoutAsync(string userId);
    Task<OrderDto> GetOrderByIdAsync(string orderId, string userId);
    Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
}