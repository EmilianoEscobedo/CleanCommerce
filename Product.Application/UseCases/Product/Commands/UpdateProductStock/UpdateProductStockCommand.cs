using MediatR;
using Product.Application.DTOs;
using Product.Domain.Common;

namespace Product.Application.UseCases.Product.Commands.UpdateProductStock;

public class UpdateProductStockCommand : IRequest<Result<UpdateProductStockResponse>>
{
    public int Id { get; set; }
    public UpdateProductStockDto Dto { get; set; }

    public UpdateProductStockCommand(int id, UpdateProductStockDto dto)
    {
        Id = id;
        Dto = dto;
    }
}