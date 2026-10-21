using System;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries
{
    public record ObtenerResumenEfectivoQry(
        byte IdCtSucursal,
        DateTime? FechaInicio,
        DateTime? FechaFin
    ) : IRequest<ResumenEfectivoDto>;
}
