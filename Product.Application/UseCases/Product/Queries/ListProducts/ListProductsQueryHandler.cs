using AutoMapper;
using MediatR;
using Product.Application.DTOs;
using Product.Domain.Common;
using Product.Domain.Repositories;

namespace Product.Application.UseCases.Product.Queries.ListProducts;

public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, Result<ListProductsResponse>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ListProductsQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ListProductsResponse>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync();

        if (result.IsFailure)
        {
            return Result<ListProductsResponse>.Failure(result.Errors);
        }

        var dtos = _mapper.Map<IEnumerable<ProductDto>>(result.Value);
        var response = new ListProductsResponse(dtos);

        return Result<ListProductsResponse>.Success(response);
    }
}