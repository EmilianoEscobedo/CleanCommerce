using Product.Application.DTOs;

namespace Product.Application.UseCases.Product.Commands.UpdateProductStock;

public class UpdateProductStockResponse
{
    public ProductDto Product { get; }

    public UpdateProductStockResponse(ProductDto product)
    {
        Product = product;
    }
}