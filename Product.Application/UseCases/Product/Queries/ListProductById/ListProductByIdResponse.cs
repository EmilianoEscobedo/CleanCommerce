using Product.Application.DTOs;

namespace Product.Application.UseCases.Product.Queries.ListProductById;

public class ListProductByIdResponse
{
    public ProductDto Product { get; }

    public ListProductByIdResponse(ProductDto product)
    {
        Product = product;
    }
}