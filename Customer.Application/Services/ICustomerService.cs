using Customer.Application.DTOs;
using Customer.Domain.Common;

namespace Customer.Application.Services;

public interface ICustomerService
{
    Task<Result<IEnumerable<CustomerDto>>> GetAllCustomersAsync();
    Task<Result<CustomerDto>> GetCustomerByIdAsync(int id);
    Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task<Result<CustomerDto>> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto);
    Task<Result<bool>> DeleteCustomerAsync(int id);
}