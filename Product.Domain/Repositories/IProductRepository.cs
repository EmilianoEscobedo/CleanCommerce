namespace Product.Domain.Repositories;

public interface IProductRepository
{
    Task<Entities.Product?> GetByIdAsync(int id);
    Task<IEnumerable<Entities.Product>> GetAllAsync();
    Task<IEnumerable<Entities.Product>> GetAllPaginatedAsync(int page, int pageSize);
    Task<int> AddAsync(Entities.Product product);
    Task UpdateAsync(Entities.Product product);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> SaveChangesAsync();
}