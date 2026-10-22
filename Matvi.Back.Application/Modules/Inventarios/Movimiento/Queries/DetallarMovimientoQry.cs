using Matvi.Back.Application.Modules.Inventarios.Movimiento.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Inventarios.Movimiento.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar movimiento.
    /// </summary>
public record DetallarMovimientoQry(
        int id
        ) : IRequest<DetalleMovimientoDto>;

}