using Order.Application.DTOs;
using Order.Domain.Common;

namespace Order.Application.Services;

public interface ICustomerService
{
    Task<Result<CustomerResponseDto>> GetCustomerAsync(int id);
}