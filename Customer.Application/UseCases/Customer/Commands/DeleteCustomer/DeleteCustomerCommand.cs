using Customer.Domain.Common;
using MediatR;

namespace Customer.Application.UseCases.Customer.Commands.DeleteCustomer;

public class DeleteCustomerCommand : IRequest<Result<DeleteCustomerResponse>>
{
    public int Id { get; }

    public DeleteCustomerCommand(int id)
    {
        Id = id;
    }
}