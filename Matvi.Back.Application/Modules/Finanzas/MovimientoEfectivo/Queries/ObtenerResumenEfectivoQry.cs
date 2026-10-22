using System;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de obtener resumen efectivo.
    /// </summary>
public record ObtenerResumenEfectivoQry(
        byte IdCtSucursal,
        DateTime? FechaInicio,
        DateTime? FechaFin
    ) : IRequest<ResumenEfectivoDto>;
}
