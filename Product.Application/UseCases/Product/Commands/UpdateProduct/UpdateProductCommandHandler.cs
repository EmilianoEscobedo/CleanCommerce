using AutoMapper;
using MediatR;
using Product.Domain.Common;
using Product.Domain.Repositories;
using FluentValidation;
using Product.Application.DTOs;

namespace Product.Application.UseCases.Product.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<UpdateProductResponse>>
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateProductDto> _validator;

    public UpdateProductCommandHandler(IProductRepository repository, IMapper mapper, IValidator<UpdateProductDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<UpdateProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var validation = await _validator.ValidateAsync(dto, cancellationToken);
        if (!validation.IsValid)
            return Result<UpdateProductResponse>.Failure(validation.Errors.Select(e => e.ErrorMessage).ToList());

        var productResult = await _repository.GetByIdAsync(request.Id);
        if (productResult.IsFailure)
            return Result<UpdateProductResponse>.Failure(productResult.Errors);

        var product = productResult.Value;

        if (dto.Price < 0)
            return Result<UpdateProductResponse>.Failure("Price cannot be negative");

        if (string.IsNullOrWhiteSpace(dto.Name))
            return Result<UpdateProductResponse>.Failure("Name cannot be empty");

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.UpdatedDate = DateTime.UtcNow;

        var updateResult = await _repository.UpdateAsync(product);
        if (updateResult.IsFailure)
            return Result<UpdateProductResponse>.Failure(updateResult.Errors);

        var saveResult = await _repository.SaveChangesAsync();
        if (saveResult.IsFailure)
            return Result<UpdateProductResponse>.Failure(saveResult.Errors);

        return Result<UpdateProductResponse>.Success(new UpdateProductResponse(_mapper.Map<ProductDto>(product)));
    }
}
