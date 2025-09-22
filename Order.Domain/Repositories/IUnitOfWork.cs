namespace Order.Domain.Repositories;

public interface IUnitOfWork
{
    IOrderRepository OrderRepository { get; }
    Task<bool> SaveChangesAsync();
}