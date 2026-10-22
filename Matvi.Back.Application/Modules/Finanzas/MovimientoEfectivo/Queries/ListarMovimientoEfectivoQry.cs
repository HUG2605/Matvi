using System;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar movimiento efectivo.
    /// </summary>
public record ListarMovimientoEfectivoQry(
        byte IdCtSucursal,
        byte? Tipo,
        string? OrigenTipo,
        bool? EnCorte,
        DateTime? FechaInicio,
        DateTime? FechaFin,
        FilterGrd Filter
    ) : IRequest<DataGrd<ListarMovimientoEfectivoDto>>;
}
