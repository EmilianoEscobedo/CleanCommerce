using WebClient.Application.DTOs.Customer;
using WebClient.Domain.Common;

namespace WebClient.Application.Services;

public interface ICustomerService
{
    Task<Result<IEnumerable<CustomerResponseDto>>> GetAllCustomersAsync();
    Task<Result<CustomerResponseDto>> CreateCustomerAsync(CreateCustomerRequestDto createCustomerDto);
    Task<Result<CustomerResponseDto>> UpdateCustomerAsync(int id, UpdateCustomerRequestDto updateCustomerDto);
    Task<Result<bool>> DeleteCustomerAsync(int id);
}