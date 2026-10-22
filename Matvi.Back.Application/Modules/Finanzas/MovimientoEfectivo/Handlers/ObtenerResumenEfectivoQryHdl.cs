using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Dtos;
using Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoEfectivo.Handlers
{
    class ObtenerResumenEfectivoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ObtenerResumenEfectivoQry, ResumenEfectivoDto>
    {
                /// <inheritdoc/>
public async Task<ResumenEfectivoDto> Handle(
            ObtenerResumenEfectivoQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var sucId = request.IdCtSucursal;
            DateTime? fin = request.FechaFin.HasValue ? request.FechaFin.Value.Date.AddDays(1).AddTicks(-1) : null;

            // Cobros
            var qCobros = db.Set<OpCobroEtt>().AsNoTracking().Where(c => c.IdCtSucursal == sucId && c.IdCtMedioPago == 1);
            if (request.FechaInicio.HasValue) qCobros = qCobros.Where(c => c.Fecha >= request.FechaInicio.Value);
            if (fin.HasValue) qCobros = qCobros.Where(c => c.Fecha <= fin.Value);


            // Gastos
            var qGastos = db.Set<OpGastoEtt>().AsNoTracking().Where(g => g.IdCtSucursal == sucId && g.IdCtMedioPago == 1);
            if (request.FechaInicio.HasValue) qGastos = qGastos.Where(g => g.FechaPago >= request.FechaInicio.Value);
            if (fin.HasValue) qGastos = qGastos.Where(g => g.FechaPago <= fin.Value);

            // Pagos
            var qPagos = db.Set<OpPagoEtt>().AsNoTracking().Where(p => p.IdCtSucursal == sucId && p.IdCtMedioPago == 1);
            if (request.FechaInicio.HasValue) qPagos = qPagos.Where(p => p.Fecha >= request.FechaInicio.Value);
            if (fin.HasValue) qPagos = qPagos.Where(p => p.Fecha <= fin.Value);

            var cobrosMonto = await qCobros.SumAsync(x => (decimal?)x.MontoTotal, cancellationToken) ?? 0m;
            var totalIngresos = cobrosMonto;

            var gastosMonto = await qGastos.SumAsync(x => (decimal?)x.Importe, cancellationToken) ?? 0m;
            var pagosMonto = await qPagos.SumAsync(x => (decimal?)x.MontoTotal, cancellationToken) ?? 0m;
            var totalEgresos = gastosMonto + pagosMonto;

            var cobrosSinCorteCount = await qCobros.Where(x => x.IdOpCorteCaja == null).CountAsync(cancellationToken);
            var gastosSinCorteCount = await qGastos.Where(x => x.IdOpCorteCaja == null).CountAsync(cancellationToken);
            var pagosSinCorteCount = await qPagos.Where(x => x.IdOpCorteCaja == null).CountAsync(cancellationToken);
            var totalSinCorte = cobrosSinCorteCount + gastosSinCorteCount + pagosSinCorteCount;

            var cobrosSinCorteMonto = await qCobros.Where(x => x.IdOpCorteCaja == null).SumAsync(x => (decimal?)x.MontoTotal, cancellationToken) ?? 0m;
            var gastosSinCorteMonto = await qGastos.Where(x => x.IdOpCorteCaja == null).SumAsync(x => (decimal?)x.Importe, cancellationToken) ?? 0m;
            var pagosSinCorteMonto = await qPagos.Where(x => x.IdOpCorteCaja == null).SumAsync(x => (decimal?)x.MontoTotal, cancellationToken) ?? 0m;
            var montoSinCorte = cobrosSinCorteMonto - (gastosSinCorteMonto + pagosSinCorteMonto);

            var totalMovimientos = (await qCobros.CountAsync(cancellationToken)) +
                                   (await qGastos.CountAsync(cancellationToken)) +
                                   (await qPagos.CountAsync(cancellationToken));

            return new ResumenEfectivoDto
            {
                TotalIngresos = totalIngresos,
                TotalEgresos = totalEgresos,
                TotalSinCorte = totalSinCorte,
                MontoSinCorte = montoSinCorte,
                TotalMovimientos = totalMovimientos
            };
        }
    }
}
