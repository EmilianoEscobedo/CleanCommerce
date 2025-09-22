using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Order.Application.DTOs;
using Order.Application.Services;
using Order.Domain.Common;
using ProductDto = Order.Application.DTOs.ProductDto;

namespace Order.Infrastructure.ExternalService;

public class ExternalServicesClient : IExternalServicesClient
{
    private readonly HttpClient _httpClient;
    private readonly ExternalServiceSettings _settings;

    public ExternalServicesClient(HttpClient httpClient, IOptions<ExternalServiceSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<Result<ProductDto>> GetProductAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_settings.ProductBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return Result<ProductDto>.Failure($"Failed to fetch product {id}. Status: {response.StatusCode}");

        var product = await response.Content.ReadFromJsonAsync<ProductDto>();
        return product is null 
            ? Result<ProductDto>.Failure("Product deserialization failed.") 
            : Result<ProductDto>.Success(product);
    }

    public async Task<Result<CustomerDto>> GetCustomerAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_settings.CustomerBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return Result<CustomerDto>.Failure($"Failed to fetch customer {id}. Status: {response.StatusCode}");

        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
        return customer is null 
            ? Result<CustomerDto>.Failure("Customer deserialization failed.") 
            : Result<CustomerDto>.Success(customer);
    }

    public async Task<Result> UpdateProductStockAsync(int productId, int quantity)
    {
        var updateDto = new UpdateProductStockDto { Quantity = quantity };
        var response = await _httpClient.PatchAsJsonAsync($"{_settings.ProductBaseUrl}/{productId}/stock", updateDto);

        return !response.IsSuccessStatusCode
            ? Result.Failure($"Failed to update stock for product {productId}. Status: {response.StatusCode}")
            : Result.Success();
    }
}
