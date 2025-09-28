using AutoMapper;
using MediatR;
using Order.Application.DTOs;
using Order.Domain.Common;
using Order.Domain.Repositories;

namespace Order.Application.UseCases.Order.Queries.ListOrders;

public class ListOrdersQueryHandler : IRequestHandler<ListOrdersQuery, Result<ListOrdersResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ListOrdersResponse>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
    {
        var ordersResult = await _unitOfWork.OrderRepository.GetAllAsync();
        if (ordersResult.IsFailure)
            return Result<ListOrdersResponse>.Failure(ordersResult.Errors);

        var dtos = _mapper.Map<IEnumerable<OrderResponseDto>>(ordersResult.Value);
        return Result<ListOrdersResponse>.Success(new ListOrdersResponse { Orders = dtos });
    }
}