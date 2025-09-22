using Order.Domain.Common;

namespace Order.Domain.Repositories;
using Entities;


public interface IOrderRepository
{
    Task<Result<Order>> GetByIdAsync(int id);
    Task<Result<IEnumerable<Order>>> GetAllAsync();
    Task<Result<Order>> AddAsync(Order order);
    Task<Result<bool>> UpdateAsync(Order order);
    Task<Result<bool>> DeleteAsync(int id);
}