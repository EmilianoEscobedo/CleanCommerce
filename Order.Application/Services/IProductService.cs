using Order.Application.DTOs;
using Order.Domain.Common;

namespace Order.Application.Services;

public interface IProductService
{
    Task<Result<ProductDto>> GetProductAsync(int id);
    Task<Result> UpdateProductStockAsync(int productId, int quantity);
}