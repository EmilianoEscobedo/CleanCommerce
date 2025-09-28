using MediatR;
using Product.Domain.Common;

namespace Product.Application.UseCases.Product.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest<Result<DeleteProductResponse>>
{
    public int Id { get; set; }

    public DeleteProductCommand(int id)
    {
        Id = id;
    }
}