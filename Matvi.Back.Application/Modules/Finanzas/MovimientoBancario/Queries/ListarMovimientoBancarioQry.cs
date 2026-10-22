using System;
using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de listar movimiento bancario.
    /// </summary>
public record ListarMovimientoBancarioQry(
        byte? IdCtSucursal,
        byte? IdCtCuentaBancaria,
        byte? Tipo,
        bool? Conciliado,
        DateTime? FechaInicio,
        DateTime? FechaFin,
        FilterGrd Filter
    ) : IRequest<DataGrd<ListarMovimientoBancarioDto>>;
}
