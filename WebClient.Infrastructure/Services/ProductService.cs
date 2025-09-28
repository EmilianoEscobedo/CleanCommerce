using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Product.Application.DTOs;
using WebClient.Application.DTOs.Product;
using WebClient.Application.Services;
using WebClient.Domain.Common;
using WebClient.Infrastructure.Settings;

namespace WebClient.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ClientSettings _settings;

    public ProductService(HttpClient httpClient, IOptions<ClientSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<Result<IEnumerable<ProductResponseDto>>> GetAllProductsAsync()
    {
        var response = await _httpClient.GetAsync($"{_settings.ProductBaseUrl}");
        if (!response.IsSuccessStatusCode)
            return Result<IEnumerable<ProductResponseDto>>.Failure(
                $"Failed to fetch products. Status: {response.StatusCode}");
        var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductResponseDto>>();
        return products is null
            ? Result<IEnumerable<ProductResponseDto>>.Failure("Products deserialization failed.")
            : Result<IEnumerable<ProductResponseDto>>.Success(products);
    }

    public async Task<Result<ProductResponseDto>> GetProductByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_settings.ProductBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return Result<ProductResponseDto>.Failure(
                $"Failed to fetch product {id}. Status: {response.StatusCode}");

        var product = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        return product is null
            ? Result<ProductResponseDto>.Failure("Product deserialization failed.")
            : Result<ProductResponseDto>.Success(product);
    }

    public async Task<Result<ProductResponseDto>> CreateProductAsync(CreateProductRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_settings.ProductBaseUrl}", request);
        if (!response.IsSuccessStatusCode)
            return Result<ProductResponseDto>.Failure(
                $"Failed to create product. Status: {response.StatusCode}");

        var created = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        return created is null
            ? Result<ProductResponseDto>.Failure("Product deserialization failed.")
            : Result<ProductResponseDto>.Success(created);
    }

    public async Task<Result<ProductResponseDto>> UpdateProductAsync(int id, UpdateProductRequestDto request)
    {
        var response = await _httpClient.PutAsJsonAsync($"{_settings.ProductBaseUrl}/{id}", request);
        if (!response.IsSuccessStatusCode)
            return Result<ProductResponseDto>.Failure(
                $"Failed to update product {id}. Status: {response.StatusCode}");

        var updated = await response.Content.ReadFromJsonAsync<ProductResponseDto>();
        return updated is null
            ? Result<ProductResponseDto>.Failure("Product deserialization failed.")
            : Result<ProductResponseDto>.Success(updated);
    }

    public async Task<Result<bool>> DeleteProductAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_settings.ProductBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return Result<bool>.Failure($"Failed to delete product {id}. Status: {response.StatusCode}");

        return Result<bool>.Success(true);
    }
}