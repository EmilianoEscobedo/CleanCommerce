using MediatR;
using Order.Domain.Common;

namespace Order.Application.UseCases.Order.Queries.ListOrders;

public record ListOrdersQuery : IRequest<Result<ListOrdersResponse>>;