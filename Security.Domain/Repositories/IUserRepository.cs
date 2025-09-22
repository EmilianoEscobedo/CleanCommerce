using Security.Domain.Commons;
using Security.Domain.Entities;

namespace Security.Domain.Repositories;

public interface IUserRepository
{
    Task<Result<User>> GetByEmailAsync(string email);
    Task<Result<User>> AddAsync(User user);
    Task<Result<bool>> EmailExistsAsync(string email);
}