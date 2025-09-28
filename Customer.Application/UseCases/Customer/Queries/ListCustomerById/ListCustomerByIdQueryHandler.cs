using AutoMapper;
using MediatR;
using Customer.Application.DTOs;
using Customer.Domain.Common;
using Customer.Domain.Repositories;

namespace Customer.Application.UseCases.Customer.Queries.ListCustomerById;

public class ListCustomerByIdQueryHandler : IRequestHandler<ListCustomerByIdQuery, Result<ListCustomerByIdResponse>>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public ListCustomerByIdQueryHandler(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ListCustomerByIdResponse>> Handle(ListCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id);
        if (result.IsFailure)
            return Result<ListCustomerByIdResponse>.Failure(result.Errors);

        var dto = _mapper.Map<CustomerDto>(result.Value);
        return Result<ListCustomerByIdResponse>.Success(new ListCustomerByIdResponse(dto));
    }
}