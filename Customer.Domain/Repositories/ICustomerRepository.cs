namespace Customer.Domain.Repositories;

public interface ICustomerRepository
{
    Task<Entities.Customer?> GetByIdAsync(int id);
    Task<IEnumerable<Entities.Customer>> GetAllAsync();
    Task<IEnumerable<Entities.Customer>> GetAllPaginatedAsync(int page, int pageSize);
    Task<int> AddAsync(Entities.Customer customer);
    Task UpdateAsync(Entities.Customer customer);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<int> SaveChangesAsync();
}