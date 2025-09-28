using MediatR;
using Order.Application.DTOs;
using Order.Domain.Common;

namespace Order.Application.UseCases.Order.Commands.CreateOrder;

public record CreateOrderCommand(CreateOrderRequestDto Order) : IRequest<Result<CreateOrderResponse>>;