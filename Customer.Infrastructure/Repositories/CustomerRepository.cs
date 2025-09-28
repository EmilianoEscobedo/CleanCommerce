using Microsoft.EntityFrameworkCore;
using Customer.Domain.Common;
using Customer.Domain.Repositories;
using Customer.Infrastructure.Data;

namespace Customer.Infrastructure.Repositories;
using Domain.Entities;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
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

    public async Task<Result<Customer>> AddAsync(Customer customer)
    {
        try
        {
            await _context.Customers.AddAsync(customer);
            return Result<Customer>.Success(customer);
        }
        catch (Exception ex)
        {
            return Result<Customer>.Failure($"Failed to add customer: {ex.Message}");
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return Result.Failure($"Customer with ID {id} not found");

        _context.Customers.Remove(customer);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<Customer>>> GetAllAsync()
    {
        var customers = await _context.Customers.ToListAsync();
        return Result<IEnumerable<Customer>>.Success(customers);
    }

    public async Task<Result<Customer>> GetByIdAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return Result<Customer>.Failure($"Customer with ID {id} not found");

        return Result<Customer>.Success(customer);
    }

    public async Task<Result> UpdateAsync(Customer customer)
    {
        var exists = await _context.Customers.AnyAsync(c => c.Id == customer.Id);
        if (!exists)
            return Result.Failure($"Customer with ID {customer.Id} not found");

        _context.Customers.Update(customer);
        return Result.Success();
    }
}
