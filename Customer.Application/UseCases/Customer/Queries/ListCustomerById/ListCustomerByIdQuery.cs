using MediatR;
using Customer.Domain.Common;

namespace Customer.Application.UseCases.Customer.Queries.ListCustomerById;

public class ListCustomerByIdQuery : IRequest<Result<ListCustomerByIdResponse>>
{
    public int Id { get; }

    public ListCustomerByIdQuery(int id)
    {
        Id = id;
    }
}