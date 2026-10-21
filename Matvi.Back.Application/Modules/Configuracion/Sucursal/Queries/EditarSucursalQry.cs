using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries
{
    public record EditarSucursalQry(
        byte id
        ) : IRequest<EditarSucursalDto>;

}
