using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Queries
{
    public record EditarRolQry(
        byte id
        ): IRequest<EditarRolDto>;
}
