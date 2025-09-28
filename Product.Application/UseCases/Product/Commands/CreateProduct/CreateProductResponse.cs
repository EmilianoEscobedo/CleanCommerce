using Product.Application.DTOs;

namespace Product.Application.UseCases.Product.Commands.CreateProduct;

public class CreateProductResponse
{
    public ProductDto Product { get; }

    public CreateProductResponse(ProductDto product)
    {
        Product = product;
    }
}