using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de eliminar seccion.
    /// </summary>
public record EliminarSeccionCmd(
        byte id
        ) : IRequest<OperationResult<bool>>;
}
