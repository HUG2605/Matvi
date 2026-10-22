using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Commands
{
        /// <summary>
    /// Comando CQRS para la operación de editar rol.
    /// </summary>
public record EditarRolCmd(
        EditarRolDto rol
        ) : IRequest<OperationResult<bool>>;
    
}
