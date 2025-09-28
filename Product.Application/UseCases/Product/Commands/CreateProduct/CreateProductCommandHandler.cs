using AutoMapper;
using FluentValidation;
using MediatR;
using Product.Application.DTOs;
using Product.Domain.Common;
using Product.Domain.Repositories;

namespace Product.Application.UseCases.Product.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProductDto> _validator;

    public CreateProductCommandHandler(IProductRepository repository, IMapper mapper, IValidator<CreateProductDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var validation = await _validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return Result<CreateProductResponse>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToList());

        if (dto.Price < 0)
            return Result<CreateProductResponse>.Failure("Price cannot be negative");

        if (dto.StockQuantity < 0)
            return Result<CreateProductResponse>.Failure("Stock quantity cannot be negative");

        var product = new Domain.Entities.Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            CreatedDate = DateTime.UtcNow
        };

        var addResult = await _repository.AddAsync(product);
        if (addResult.IsFailure)
            return Result<CreateProductResponse>.Failure(addResult.Errors);

        var saveResult = await _repository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<CreateProductResponse>.Failure(saveResult.Errors);

        return Result<CreateProductResponse>.Success(new CreateProductResponse(_mapper.Map<ProductDto>(product)));
    }
}
