using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de editar sucursal.
    /// </summary>
public record EditarSucursalCmd (
             EditarSucursalDto sucursal
        ) : IRequest<OperationResult<bool>>;

}
