using Microsoft.EntityFrameworkCore;
using Product.Domain.Repositories;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<int> AddAsync(Domain.Entities.Product product)
    {
        await _context.Products.AddAsync(product);
        return product.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Domain.Entities.Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Product>> GetAllPaginatedAsync(int page, int pageSize)
    {
        return await _context.Products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Domain.Entities.Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public Task UpdateAsync(Domain.Entities.Product product)
    {
        _context.Products.Update(product);
        return Task.CompletedTask;
    }
}