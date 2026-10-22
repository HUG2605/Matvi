using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de editar seccion.
    /// </summary>
public record EditarSeccionCmd (
        EditarSeccionDto seccion
        ) : IRequest<OperationResult<bool>>;    
}
