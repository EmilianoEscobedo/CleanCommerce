using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Order.Application.DTOs;
using Order.Application.Services;
using Order.Domain.Common;
using Order.Infrastructure.Settings;

namespace Order.Infrastructure.Services;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;
    private readonly ClientSettings _settings;

    public CustomerService(HttpClient httpClient, IOptions<ClientSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<Result<CustomerResponseDto>> GetCustomerAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_settings.CustomerBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return Result<CustomerResponseDto>.Failure($"Failed to fetch customer {id}. Status: {response.StatusCode}");

        var customer = await response.Content.ReadFromJsonAsync<CustomerResponseDto>();
        return customer is null 
            ? Result<CustomerResponseDto>.Failure("Customer deserialization failed.") 
            : Result<CustomerResponseDto>.Success(customer);
    }
}