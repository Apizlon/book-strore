using BookService.Application.Models;

namespace BookService.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(string id);
    Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
    Task<Order> CreateAsync(Order order);
}