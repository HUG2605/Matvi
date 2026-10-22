using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de eliminar presentacion producto.
    /// </summary>
public record EliminarPresentacionProductoCmd(
        int id
        ) : IRequest<OperationResult<bool>>;
}
