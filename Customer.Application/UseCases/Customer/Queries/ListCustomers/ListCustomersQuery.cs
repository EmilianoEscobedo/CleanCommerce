using MediatR;
using Customer.Domain.Common;

namespace Customer.Application.UseCases.Customer.Queries.ListCustomers;

public class ListCustomersQuery : IRequest<Result<ListCustomersResponse>>
{
}