using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Producto.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de eliminar producto.
    /// </summary>
public record EliminarProductoCmd(
        int id
        ) : IRequest<OperationResult<bool>>;
}
