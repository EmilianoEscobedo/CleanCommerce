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
    private readonly IValidator<CreateCustomerDto> _createValidator;
    private readonly IValidator<UpdateCustomerDto> _updateValidator;

    public CustomerService(
        ICustomerRepository customerRepository,
        IMapper mapper,
        IValidator<CreateCustomerDto> createValidator,
        IValidator<UpdateCustomerDto> updateValidator)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Result<IEnumerable<CustomerDto>>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
        return Result<IEnumerable<CustomerDto>>.Success(customerDtos);
    }

    public async Task<Result<CustomerDto>> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            return Result<CustomerDto>.Failure($"Customer with id {id} not found");
        }

        var customerDto = _mapper.Map<CustomerDto>(customer);
        return Result<CustomerDto>.Success(customerDto);
    }

    public async Task<Result<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createCustomerDto);
        if (!validationResult.IsValid)
        {
            return Result<CustomerDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var customer = new Domain.Entities.Customer(
            createCustomerDto.Name, 
            createCustomerDto.Email, 
            createCustomerDto.Address ?? string.Empty
        );

        await _customerRepository.AddAsync(customer);
        await _customerRepository.SaveChangesAsync();

        var customerDto = _mapper.Map<CustomerDto>(customer);
        return Result<CustomerDto>.Success(customerDto);
    }

    public async Task<Result<CustomerDto>> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateCustomerDto);
        if (!validationResult.IsValid)
        {
            return Result<CustomerDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var existingCustomer = await _customerRepository.GetByIdAsync(id);
        if (existingCustomer == null)
        {
            return Result<CustomerDto>.Failure($"Customer with id {id} not found");
        }

        existingCustomer.UpdateDetails(
            updateCustomerDto.Name,
            updateCustomerDto.Email,
            updateCustomerDto.Address
        );

        await _customerRepository.UpdateAsync(existingCustomer);
        await _customerRepository.SaveChangesAsync();

        var customerDto = _mapper.Map<CustomerDto>(existingCustomer);
        return Result<CustomerDto>.Success(customerDto);
    }

    public async Task<Result<bool>> DeleteCustomerAsync(int id)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(id);
        if (existingCustomer == null)
        {
            return Result<bool>.Failure($"Customer with id {id} not found");
        }

        await _customerRepository.DeleteAsync(id);
        await _customerRepository.SaveChangesAsync();

        return Result<bool>.Success(true);
    }
}