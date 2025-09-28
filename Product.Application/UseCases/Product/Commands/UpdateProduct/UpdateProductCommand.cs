using MediatR;
using Product.Application.DTOs;
using Product.Domain.Common;

namespace Product.Application.UseCases.Product.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest<Result<UpdateProductResponse>>
{
    public int Id { get; set; }
    public UpdateProductDto Dto { get; set; }

    public UpdateProductCommand(int id, UpdateProductDto dto)
    {
        Id = id;
        Dto = dto;
    }
}