using BookService.Application.Interfaces;
using BookService.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace BookService.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly BookDbContext _dbContext;

    public OrderRepository(BookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> GetByIdAsync(string id)
    {
        return await _dbContext.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Book)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
    {
        return await _dbContext.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Book)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order> CreateAsync(Order order)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }
}