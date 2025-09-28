using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WebClient.Application.DTOs.Order;
using WebClient.Application.Services;
using WebClient.Domain.Common;
using WebClient.Infrastructure.Settings;

namespace WebClient.Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    private readonly ClientSettings _settings;

    public OrderService(HttpClient httpClient, IOptions<ClientSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<Result<OrderResponseDto>> CreateOrderAsync(CreateOrderRequestDto orderDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_settings.OrderBaseUrl}", orderDto);
        if (!response.IsSuccessStatusCode)
            return Result<OrderResponseDto>.Failure(
                $"Failed to create order. Status: {response.StatusCode}");

        var created = await response.Content.ReadFromJsonAsync<OrderResponseDto>();
        return created is null
            ? Result<OrderResponseDto>.Failure("Order deserialization failed.")
            : Result<OrderResponseDto>.Success(created);
    }

    public async Task<Result<OrderResponseDto>> GetOrderByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{_settings.OrderBaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
            return Result<OrderResponseDto>.Failure(
                $"Failed to fetch order {id}. Status: {response.StatusCode}");

        var order = await response.Content.ReadFromJsonAsync<OrderResponseDto>();
        return order is null
            ? Result<OrderResponseDto>.Failure("Order deserialization failed.")
            : Result<OrderResponseDto>.Success(order);
    }

    public async Task<Result<IEnumerable<OrderResponseDto>>> GetAllOrdersAsync()
    {
        var response = await _httpClient.GetAsync($"{_settings.OrderBaseUrl}");
        if (!response.IsSuccessStatusCode)
            return Result<IEnumerable<OrderResponseDto>>.Failure(
                $"Failed to fetch orders. Status: {response.StatusCode}");

        var orders = await response.Content.ReadFromJsonAsync<IEnumerable<OrderResponseDto>>();
        return orders is null
            ? Result<IEnumerable<OrderResponseDto>>.Failure("Orders deserialization failed.")
            : Result<IEnumerable<OrderResponseDto>>.Success(orders);
    }
}