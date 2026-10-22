using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de editar sucursal.
    /// </summary>
public record EditarSucursalQry(
        byte id
        ) : IRequest<EditarSucursalDto>;

}
