using Matvi.Back.Application.Common.Results;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de eliminar sucursal.
    /// </summary>
public record EliminarSucursalCmd(
        byte id
        ) : IRequest<OperationResult<bool>>;
    
}
