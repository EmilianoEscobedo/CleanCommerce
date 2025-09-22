using Microsoft.EntityFrameworkCore;
using Security.Domain.Commons;
using Security.Domain.Entities;
using Security.Domain.Repositories;
using Security.Infrastructure.Data;

namespace Security.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SecurityDbContext _context;

    public UserRepository(SecurityDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> GetByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user is null 
            ? Result<User>.Failure("User not found") 
            : Result<User>.Success(user);
    }

    public async Task<Result<User>> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        var saveResult = await SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<User>.Failure(saveResult.Errors);

        return Result<User>.Success(user);
    }

    public async Task<Result<bool>> EmailExistsAsync(string email)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == email);
        return Result<bool>.Success(exists);
    }

    private async Task<Result<bool>> SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new List<string> { ex.Message });
        }
    }
}