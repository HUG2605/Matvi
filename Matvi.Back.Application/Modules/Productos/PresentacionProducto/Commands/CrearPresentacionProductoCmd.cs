using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de crear presentacion producto.
    /// </summary>
public record CrearPresentacionProductoCmd(
        CrearPresentacionProductoDto presentacionProducto
          ) : IRequest<OperationResult<bool>>;
}
