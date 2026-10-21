using System;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Queries
{
    public record ObtenerResumenBancarioQry(
        byte? IdCtSucursal,
        byte? IdCtCuentaBancaria,
        DateTime? FechaInicio,
        DateTime? FechaFin
    ) : IRequest<ResumenBancarioDto>;
}
