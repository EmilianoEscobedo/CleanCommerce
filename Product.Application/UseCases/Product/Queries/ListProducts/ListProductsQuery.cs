using MediatR;
using Product.Domain.Common;

namespace Product.Application.UseCases.Product.Queries.ListProducts;

public class ListProductsQuery : IRequest<Result<ListProductsResponse>>
{
}