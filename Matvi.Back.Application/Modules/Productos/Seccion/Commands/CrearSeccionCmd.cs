using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de crear seccion.
    /// </summary>
public record CrearSeccionCmd(
        CrearSeccionDto seccion
    ) : IRequest<OperationResult<bool>>;
}
