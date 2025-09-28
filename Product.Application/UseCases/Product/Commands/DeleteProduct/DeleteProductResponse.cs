namespace Product.Application.UseCases.Product.Commands.DeleteProduct;

public class DeleteProductResponse
{
    public bool Success { get; }

    public DeleteProductResponse(bool success)
    {
        Success = success;
    }
}