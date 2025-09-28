using MediatR;
using Product.Application.DTOs;
using Product.Application.UseCases.Product.Commands.CreateProduct;
using Product.Domain.Common;

public class CreateProductCommand : IRequest<Result<CreateProductResponse>>
{
    public CreateProductDto Dto { get; set; }

    public CreateProductCommand(CreateProductDto dto)
    {
        Dto = dto;
    }
}