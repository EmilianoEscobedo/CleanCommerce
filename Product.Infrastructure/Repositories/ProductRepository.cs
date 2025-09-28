using Microsoft.EntityFrameworkCore;
using Product.Domain.Common;
using Product.Domain.Repositories;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repositories;
using Domain.Entities;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(ProductDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<int>> SaveChangesAsync()
    {
        try
        {
            var changes = await _context.SaveChangesAsync();
            return Result<int>.Success(changes);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Failed to save changes: {ex.Message}");
        }
    }

    public async Task<Result<Product>> AddAsync(Product product)
    {
        try
        {
            await _context.Products.AddAsync(product);
            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure($"Failed to add product: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return Result.Failure($"Product with ID {id} not found");

        product.IsDeleted = true;
        product.UpdatedDate = DateTime.UtcNow;

        _context.Products.Update(product); 

        return Result.Success();
    }

    public async Task<Result<IEnumerable<Product>>> GetAllAsync()
    {
        var products = await _context.Products.ToListAsync();
        return Result<IEnumerable<Product>>.Success(products);
    }

    public async Task<Result<Product>> GetByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return Result<Product>.Failure($"Product with ID {id} not found");

        return Result<Product>.Success(product);
    }

    public async Task<Result> UpdateAsync(Product product)
    {
        var exists = await _context.Products.AnyAsync(p => p.Id == product.Id);
        if (!exists)
            return Result.Failure($"Product with ID {product.Id} not found");

        _context.Products.Update(product);
        return Result.Success();
    }
}
