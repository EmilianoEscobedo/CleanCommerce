using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WebClient.Application.DTOs.Customer;
using WebClient.Application.Services;
using WebClient.Domain.Common;
using WebClient.Infrastructure.Settings;


namespace WebClient.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;
    private readonly ClientSettings _settings;

    public CustomerService(HttpClient httpClient, IOptions<ClientSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<Result<IEnumerable<CustomerResponseDto>>> GetAllCustomersAsync()
    {
        var response = await _httpClient.GetAsync($"{_settings.CustomerBaseUrl}");
        if (!response.IsSuccessStatusCode)
            return Result<IEnumerable<CustomerResponseDto>>.Failure(
                $"Failed to fetch customers. Status: {response.StatusCode}");

        var customers = await response.Content.ReadFromJsonAsync<IEnumerable<CustomerResponseDto>>();
        return customers is null
            ? Result<IEnumerable<CustomerResponseDto>>.Failure("Customers deserialization failed.")
            : Result<IEnumerable<CustomerResponseDto>>.Success(customers);
    }

    public async Task<Result<CustomerResponseDto>> CreateCustomerAsync(CreateCustomerRequestDto createCustomerDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_settings.CustomerBaseUrl}", createCustomerDto);
        if (!response.IsSuccessStatusCode)
            return Result<CustomerResponseDto>.Failure(
                $"Failed to create customer. Status: {response.StatusCode}");

        var created = await response.Content.ReadFromJsonAsync<CustomerResponseDto>();
        return created is null
            ? Result<CustomerResponseDto>.Failure("Customer deserialization failed.")
            : Result<CustomerResponseDto>.Success(created);
    }

    public async Task<Result<CustomerResponseDto>> UpdateCustomerAsync(int id, UpdateCustomerRequestDto updateCustomerDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_settings.CustomerBaseUrl}/{id}", updateCustomerDto);
        if (!response.IsSuccessStatusCode)
            return Result<CustomerResponseDto>.Failure(
                $"Failed to update customer {id}. Status: {response.StatusCode}");

        var updated = await response.Content.ReadFromJsonAsync<CustomerResponseDto>();
        return updated is null
            ? Result<CustomerResponseDto>.Failure("Customer deserialization failed.")
            : Result<CustomerResponseDto>.Success(updated);
    }

    public async Task<Result<bool>> DeleteCustomerAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_settings.CustomerBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return Result<bool>.Failure($"Failed to delete customer {id}. Status: {response.StatusCode}");

        return Result<bool>.Success(true);
    }
}
