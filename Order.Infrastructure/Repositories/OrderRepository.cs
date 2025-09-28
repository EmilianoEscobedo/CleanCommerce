using Microsoft.EntityFrameworkCore;
using Order.Domain.Common;
using Order.Domain.Repositories;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Repositories;
using Domain.Entities;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Order>> GetByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return Result<Order>.Failure($"Order with ID {id} not found");

        return Result<Order>.Success(order);
    }

    public async Task<Result<IEnumerable<Order>>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .ToListAsync();

        return Result<IEnumerable<Order>>.Success(orders);
    }

    public async Task<Result<Order>> AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        return Result<Order>.Success(order);
    }

    public async Task<Result<bool>> UpdateAsync(Order order)
    {
        var exists = await _context.Orders.AnyAsync(o => o.Id == order.Id);
        if (!exists)
            return Result<bool>.Failure($"Order with ID {order.Id} not found");

        _context.Entry(order).State = EntityState.Modified;
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return Result<bool>.Failure($"Order with ID {id} not found");

        _context.Orders.Remove(order);
        return Result<bool>.Success(true);
    }
}