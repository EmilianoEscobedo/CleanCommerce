using AutoMapper;
using FluentValidation;
using MediatR;
using Customer.Application.DTOs;
using Customer.Domain.Common;
using Customer.Domain.Repositories;

namespace Customer.Application.UseCases.Customer.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<UpdateCustomerResponse>>
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateCustomerRequestDto> _validator;

    public UpdateCustomerCommandHandler(ICustomerRepository repository, IMapper mapper, IValidator<UpdateCustomerRequestDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<UpdateCustomerResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request.Dto, cancellationToken);
        if (!validation.IsValid)
            return Result<UpdateCustomerResponse>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToList());

        var existingResult = await _repository.GetByIdAsync(request.Id);
        if (existingResult.IsFailure)
            return Result<UpdateCustomerResponse>.Failure(existingResult.Errors);

        var customer = existingResult.Value;

        var address = new Domain.Entities.Address(
            request.Dto.Address.Country,
            request.Dto.Address.City,
            request.Dto.Address.Street,
            request.Dto.Address.Number
        );

        customer.UpdateDetails(
            request.Dto.Name,
            request.Dto.Email,
            address
        );

        var updateResult = await _repository.UpdateAsync(customer);
        if (updateResult.IsFailure)
            return Result<UpdateCustomerResponse>.Failure(updateResult.Errors);

        var saveResult = await _repository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<UpdateCustomerResponse>.Failure(saveResult.Errors);

        var dto = _mapper.Map<CustomerDto>(customer);
        return Result<UpdateCustomerResponse>.Success(new UpdateCustomerResponse(dto));
    }
}
