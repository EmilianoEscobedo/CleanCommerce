using AutoMapper;
using FluentValidation;
using Customer.Application.DTOs;
using Customer.Domain.Common;
using Customer.Domain.Repositories;

namespace Customer.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCustomerRequestDto> _createValidator;
    private readonly IValidator<UpdateCustomerRequestDto> _updateValidator;

    public CustomerService(
        ICustomerRepository customerRepository,
        IMapper mapper,
        IValidator<CreateCustomerRequestDto> createValidator,
        IValidator<UpdateCustomerRequestDto> updateValidator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<CustomerDto>>> GetAllCustomersAsync()
    {
        var result = await _customerRepository.GetAllAsync();
        if (result.IsFailure)
            return Result<IEnumerable<CustomerDto>>.Failure(result.Errors);

        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(result.Value);
        return Result<IEnumerable<CustomerDto>>.Success(customerDtos);
    }

    public async Task<Result<CustomerDto>> GetCustomerByIdAsync(int id)
    {
        var result = await _customerRepository.GetByIdAsync(id);
        if (result.IsFailure)
            return Result<CustomerDto>.Failure(result.Errors);

        var customerDto = _mapper.Map<CustomerDto>(result.Value);
        return Result<CustomerDto>.Success(customerDto);
    }

    public async Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerRequestDto createCustomerDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createCustomerDto);
        if (!validationResult.IsValid)
            return Result<CustomerDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

        var address = new Domain.Entities.Address(
            createCustomerDto.Address.Country,
            createCustomerDto.Address.City,
            createCustomerDto.Address.Street,
            createCustomerDto.Address.Number
        );

        var customer = new Domain.Entities.Customer(
            createCustomerDto.Name,
            createCustomerDto.Email,
            address
        );

        var addResult = await _customerRepository.AddAsync(customer);
        if (addResult.IsFailure)
            return Result<CustomerDto>.Failure(addResult.Errors);

        var saveResult = await _customerRepository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<CustomerDto>.Failure(saveResult.Errors);

        var customerDto = _mapper.Map<CustomerDto>(customer);
        return Result<CustomerDto>.Success(customerDto);
    }


    public async Task<Result<CustomerDto>> UpdateCustomerAsync(int id, UpdateCustomerRequestDto updateCustomerDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateCustomerDto);
        if (!validationResult.IsValid)
            return Result<CustomerDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

        var existingResult = await _customerRepository.GetByIdAsync(id);
        if (existingResult.IsFailure)
            return Result<CustomerDto>.Failure(existingResult.Errors);

        var existingCustomer = existingResult.Value;

        var address = new Domain.Entities.Address(
            updateCustomerDto.Address.Country,
            updateCustomerDto.Address.City,
            updateCustomerDto.Address.Street,
            updateCustomerDto.Address.Number
        );

        existingCustomer.UpdateDetails(
            updateCustomerDto.Name,
            updateCustomerDto.Email,
            address
        );

        var updateResult = await _customerRepository.UpdateAsync(existingCustomer);
        if (updateResult.IsFailure)
            return Result<CustomerDto>.Failure(updateResult.Errors);

        var saveResult = await _customerRepository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<CustomerDto>.Failure(saveResult.Errors);

        var customerDto = _mapper.Map<CustomerDto>(existingCustomer);
        return Result<CustomerDto>.Success(customerDto);
    }

    public async Task<Result<bool>> DeleteCustomerAsync(int id)
    {
        var existingResult = await _customerRepository.GetByIdAsync(id);
        if (existingResult.IsFailure)
            return Result<bool>.Failure(existingResult.Errors);

        var deleteResult = await _customerRepository.DeleteAsync(id);
        if (deleteResult.IsFailure)
            return Result<bool>.Failure(deleteResult.Errors);

        var saveResult = await _customerRepository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<bool>.Failure(saveResult.Errors);

        return Result<bool>.Success(true);
    }
}