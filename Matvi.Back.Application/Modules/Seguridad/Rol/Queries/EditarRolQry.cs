using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar rol.
    /// </summary>
public record EditarRolQry(
        byte id
        ): IRequest<EditarRolDto>;
}
