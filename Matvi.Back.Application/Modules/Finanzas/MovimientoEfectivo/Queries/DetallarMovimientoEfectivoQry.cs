using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de detallar movimiento efectivo.
    /// </summary>
public record DetallarMovimientoEfectivoQry(
        string TipoOrigen,
        int Id
    ) : IRequest<DetallarMovimientoEfectivoDto?>;
}
