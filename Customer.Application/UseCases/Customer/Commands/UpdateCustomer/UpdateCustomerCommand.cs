using Customer.Application.DTOs;
using Customer.Domain.Common;
using MediatR;

namespace Customer.Application.UseCases.Customer.Commands.UpdateCustomer;

public class UpdateCustomerCommand : IRequest<Result<UpdateCustomerResponse>>
{
    public int Id { get; }
    public UpdateCustomerRequestDto Dto { get; }

    public UpdateCustomerCommand(int id, UpdateCustomerRequestDto dto)
    {
        Id = id;
        Dto = dto;
    }
}