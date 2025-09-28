using Customer.Application.DTOs;
using Customer.Domain.Common;
using MediatR;

namespace Customer.Application.UseCases.Customer.Commands.CreateCustomer;

public class CreateCustomerCommand : IRequest<Result<CreateCustomerResponse>>
{
    public CreateCustomerRequestDto Dto { get; }

    public CreateCustomerCommand(CreateCustomerRequestDto dto)
    {
        Dto = dto;
    }
}