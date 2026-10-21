using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Commands
{
    public record EditarProductoCmd(
        EditarProductoDto producto
        ) : IRequest<OperationResult<bool>>;
}
