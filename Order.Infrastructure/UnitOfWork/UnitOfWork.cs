using Order.Domain.Repositories;
using Order.Infrastructure.Data;
using Order.Infrastructure.Repositories;

namespace Order.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrderDbContext _context;
    private IOrderRepository _orderRepository;

    public UnitOfWork(OrderDbContext context)
    {
        _context = context;
    }

    public IOrderRepository OrderRepository 
    {
        get { return _orderRepository ??= new OrderRepository(_context); }
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}