using AutoMapper;
using MediatR;
using Customer.Application.DTOs;
using Customer.Domain.Common;
using Customer.Domain.Repositories;

namespace Customer.Application.UseCases.Customer.Queries.ListCustomers;

public class ListCustomersQueryHandler : IRequestHandler<ListCustomersQuery, Result<ListCustomersResponse>>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public ListCustomersQueryHandler(ICustomerRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ListCustomersResponse>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync();
        if (result.IsFailure)
            return Result<ListCustomersResponse>.Failure(result.Errors);

        var dtos = _mapper.Map<IEnumerable<CustomerDto>>(result.Value);
        return Result<ListCustomersResponse>.Success(new ListCustomersResponse(dtos));
    }
}