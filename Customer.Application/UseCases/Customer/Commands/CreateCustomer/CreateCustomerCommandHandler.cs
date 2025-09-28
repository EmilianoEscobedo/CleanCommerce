using AutoMapper;
using FluentValidation;
using MediatR;
using Customer.Application.DTOs;
using Customer.Domain.Common;
using Customer.Domain.Repositories;

namespace Customer.Application.UseCases.Customer.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CreateCustomerResponse>>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCustomerRequestDto> _validator;

    public CreateCustomerCommandHandler(ICustomerRepository repository, IMapper mapper, IValidator<CreateCustomerRequestDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<CreateCustomerResponse>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request.Dto, cancellationToken);
        if (!validation.IsValid)
            return Result<CreateCustomerResponse>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToList());

        var address = new Domain.Entities.Address(
            request.Dto.Address.Country,
            request.Dto.Address.City,
            request.Dto.Address.Street,
            request.Dto.Address.Number
        );

        var customer = new Domain.Entities.Customer(
            request.Dto.Name,
            request.Dto.Email,
            address
        );

        var addResult = await _repository.AddAsync(customer);
        if (addResult.IsFailure)
            return Result<CreateCustomerResponse>.Failure(addResult.Errors);

        var saveResult = await _repository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<CreateCustomerResponse>.Failure(saveResult.Errors);

        var dto = _mapper.Map<CustomerDto>(customer);
        return Result<CreateCustomerResponse>.Success(new CreateCustomerResponse(dto));
    }
}