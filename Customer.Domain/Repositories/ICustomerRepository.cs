using Customer.Domain.Common;

namespace Customer.Domain.Repositories;
using Entities;

public interface ICustomerRepository
{
    Task<Result<Customer>> GetByIdAsync(int id);
    Task<Result<IEnumerable<Customer>>> GetAllAsync();
    Task<Result<Customer>> AddAsync(Customer customer);
    Task<Result> UpdateAsync(Customer customer);
    Task<Result> DeleteAsync(int id);
    Task<Result<int>> SaveChangesAsync();
}