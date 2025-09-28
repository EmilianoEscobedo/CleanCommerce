
using AutoMapper;
using MediatR;
using Product.Application.DTOs;
using Product.Domain.Common;
using Product.Domain.Repositories;

namespace Product.Application.UseCases.Product.Queries.ListProductById;

public class ListProductByIdQueryHandler : IRequestHandler<ListProductByIdQuery, Result<ListProductByIdResponse>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ListProductByIdQueryHandler(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ListProductByIdResponse>> Handle(ListProductByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.Id);
        if (result.IsFailure)
            return Result<ListProductByIdResponse>.Failure(result.Errors);

        return Result<ListProductByIdResponse>.Success(new ListProductByIdResponse(_mapper.Map<ProductDto>(result.Value)));
    }
}