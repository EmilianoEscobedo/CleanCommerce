using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Order.Application.DTOs;
using Order.Application.Services;
using Order.Domain.Common;
using Order.Infrastructure.ExternalService;

namespace Order.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ClientSettings _settings;

    public ProductService(HttpClient httpClient, IOptions<ClientSettings> settings)
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

    public async Task<Result> UpdateProductStockAsync(int productId, int quantity)
    {
        var updateDto = new UpdateProductStockDto { Quantity = quantity };
        var response = await _httpClient.PatchAsJsonAsync($"{_settings.ProductBaseUrl}/{productId}/stock", updateDto);

        return !response.IsSuccessStatusCode
            ? Result.Failure($"Failed to update stock for product {productId}. Status: {response.StatusCode}")
            : Result.Success();
    }
}