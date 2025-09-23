using Product.Application.DTOs;
using WebClient.Application.DTOs.Product;
using WebClient.Domain.Common;

namespace WebClient.Application.Services;

public interface IProductService
{
    Task<Result<IEnumerable<ProductResponseDto>>> GetAllProductsAsync();
    Task<Result<ProductResponseDto>> GetProductByIdAsync(int id);
    Task<Result<ProductResponseDto>> CreateProductAsync(CreateProductRequestDto request);
    Task<Result<ProductResponseDto>> UpdateProductAsync(int id, UpdateProductRequestDto request);
    Task<Result<bool>> DeleteProductAsync(int id);
}