using Matvi.Back.Application.Modules.Configuracion.Sucursal.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar sucursal.
    /// </summary>
public record DetallarSucursalQry(
        byte id
        ) : IRequest<DetallarSucursalDto>;
}
