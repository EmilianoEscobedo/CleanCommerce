using MediatR;
using Product.Domain.Common;

namespace Product.Application.UseCases.Product.Queries.ListProductById;

public class ListProductByIdQuery : IRequest<Result<ListProductByIdResponse>>
{
    public int Id { get; set; }

    public ListProductByIdQuery(int id)
    {
        Id = id;
    }
}