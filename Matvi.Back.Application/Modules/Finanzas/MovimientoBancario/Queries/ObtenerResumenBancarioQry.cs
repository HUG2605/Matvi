using System;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Queries
{
        /// <summary>
    /// Consulta CQRS para obtener información de obtener resumen bancario.
    /// </summary>
public record ObtenerResumenBancarioQry(
        byte? IdCtSucursal,
        byte? IdCtCuentaBancaria,
        DateTime? FechaInicio,
        DateTime? FechaFin
    ) : IRequest<ResumenBancarioDto>;
}
