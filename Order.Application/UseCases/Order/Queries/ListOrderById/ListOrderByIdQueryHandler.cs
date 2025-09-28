using AutoMapper;
using MediatR;
using Order.Application.DTOs;
using Order.Domain.Common;
using Order.Domain.Repositories;

namespace Order.Application.UseCases.Order.Queries.ListOrderById;

public class ListOrderByIdQueryHandler : IRequestHandler<ListOrderByIdQuery, Result<ListOrderByIdResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ListOrderByIdResponse>> Handle(ListOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var orderResult = await _unitOfWork.OrderRepository.GetByIdAsync(request.Id);
        if (orderResult.IsFailure)
            return Result<ListOrderByIdResponse>.Failure(orderResult.Errors);

        var dto = _mapper.Map<OrderResponseDto>(orderResult.Value);
        return Result<ListOrderByIdResponse>.Success(new ListOrderByIdResponse { Order = dto });
    }
}