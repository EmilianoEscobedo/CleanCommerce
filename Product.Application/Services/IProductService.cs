using Product.Application.DTOs;
using Product.Domain.Common;

namespace Product.Application.Services;

public interface IProductService
{
    Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync();
    Task<Result<ProductDto>> GetProductByIdAsync(int id);
    Task<Result<ProductDto>> CreateProductAsync(CreateProductDto createProductDto);
    Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
    Task<Result<ProductDto>> UpdateProductStockAsync(int id, UpdateProductStockDto updateProductStockDto);
    Task<Result<bool>> DeleteProductAsync(int id);
}