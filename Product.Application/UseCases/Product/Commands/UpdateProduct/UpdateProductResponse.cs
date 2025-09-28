using Product.Application.DTOs;

namespace Product.Application.UseCases.Product.Commands.UpdateProduct;

public class UpdateProductResponse
{
    public ProductDto Product { get; }

    public UpdateProductResponse(ProductDto product)
    {
        Product = product;
    }
}