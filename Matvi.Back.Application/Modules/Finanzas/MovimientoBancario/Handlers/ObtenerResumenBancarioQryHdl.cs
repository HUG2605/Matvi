using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Handlers
{
    class ObtenerResumenBancarioQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ObtenerResumenBancarioQry, ResumenBancarioDto>
    {
        public async Task<ResumenBancarioDto> Handle(
            ObtenerResumenBancarioQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<OpMovimientoBancarioEtt>().AsNoTracking();

            if (request.IdCtSucursal.HasValue && request.IdCtSucursal.Value > 0)
            {
                var sucId = request.IdCtSucursal.Value;
                query = query.Where(x =>
                    (x.Nav_IdOpCobro != null && x.Nav_IdOpCobro.IdCtSucursal == sucId) ||
                    (x.Nav_IdOpGasto != null && x.Nav_IdOpGasto.IdCtSucursal == sucId) ||
                    (x.Nav_IdOpPago != null && x.Nav_IdOpPago.IdCtSucursal == sucId)
                );
            }

            if (request.IdCtCuentaBancaria.HasValue && request.IdCtCuentaBancaria.Value > 0)
            {
                query = query.Where(x => x.IdCtCuentaBancaria == request.IdCtCuentaBancaria.Value);
            }

            if (request.FechaInicio.HasValue)
            {
                query = query.Where(x => x.Fecha >= request.FechaInicio.Value);
            }
            if (request.FechaFin.HasValue)
            {
                var fin = request.FechaFin.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.Fecha <= fin);
            }

            var totalIngresos = await query.Where(x => x.Tipo == 1).SumAsync(x => (decimal?)x.Monto, cancellationToken) ?? 0m;
            var totalEgresos = await query.Where(x => x.Tipo == 2).SumAsync(x => (decimal?)x.Monto, cancellationToken) ?? 0m;
            var pendientesCount = await query.Where(x => !x.Conciliado).CountAsync(cancellationToken);
            var pendientesMonto = await query.Where(x => !x.Conciliado).SumAsync(x => (decimal?)x.Monto, cancellationToken) ?? 0m;
            var totalMovimientos = await query.CountAsync(cancellationToken);

            return new ResumenBancarioDto
            {
                TotalIngresos = totalIngresos,
                TotalEgresos = totalEgresos,
                TotalPendientesConciliar = pendientesCount,
                MontoPendienteConciliar = pendientesMonto,
                TotalMovimientos = totalMovimientos
            };
        }
    }
}
