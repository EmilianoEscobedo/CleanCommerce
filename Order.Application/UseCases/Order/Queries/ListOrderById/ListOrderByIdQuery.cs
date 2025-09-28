using MediatR;
using Order.Domain.Common;

namespace Order.Application.UseCases.Order.Queries.ListOrderById;

public record ListOrderByIdQuery(int Id) : IRequest<Result<ListOrderByIdResponse>>;