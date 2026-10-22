using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de crear producto.
    /// </summary>
public record CrearProductoCmd(
        CrearProductoDto producto
    ) : IRequest<OperationResult<bool>>;
}
