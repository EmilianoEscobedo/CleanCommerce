using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Order.Application.DTOs;
using Order.Application.Services;
using Order.Domain.Common;
using Order.Infrastructure.Settings;

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

    public async Task<Result<ProductResponseDto>> GetProductAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_settings.ProductBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return Result<ProductResponseDto>.Failure($"Failed to fetch product {id}. Status: {response.StatusCode}");

        var product = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        return product is null 
            ? Result<ProductResponseDto>.Failure("Product deserialization failed.") 
            : Result<ProductResponseDto>.Success(product);
    }

    public async Task<Result> UpdateProductStockAsync(int productId, int quantity)
    {
        var updateDto = new UpdateProductStockRequestDto() { Quantity = quantity };
        var response = await _httpClient.PatchAsJsonAsync($"{_settings.ProductBaseUrl}/{productId}/stock", updateDto);

        return !response.IsSuccessStatusCode
            ? Result.Failure($"Failed to update stock for product {productId}. Status: {response.StatusCode}")
            : Result.Success();
    }
}