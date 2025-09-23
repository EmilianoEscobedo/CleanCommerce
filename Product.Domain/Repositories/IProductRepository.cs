using Product.Domain.Common;

namespace Product.Domain.Repositories;
using Entities;

public interface IProductRepository
{
    Task<Result<Product>> GetByIdAsync(int id);
    Task<Result<IEnumerable<Product>>> GetAllAsync();
    Task<Result<Product>> AddAsync(Product product);
    Task<Result> UpdateAsync(Product product);
    Task<Result> DeleteAsync(int id);
    Task<Result<int>> SaveChangesAsync();
}