using Customer.Application.DTOs;
using Customer.Domain.Common;

namespace Customer.Application.Services;

public interface ICustomerService
{
    Task<Result<IEnumerable<CustomerDto>>> GetAllCustomersAsync();
    Task<Result<CustomerDto>> GetCustomerByIdAsync(int id);
    Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerRequestDto createCustomerDto);
    Task<Result<CustomerDto>> UpdateCustomerAsync(int id, UpdateCustomerRequestDto updateCustomerDto);
    Task<Result<bool>> DeleteCustomerAsync(int id);
}