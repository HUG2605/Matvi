using System;
using System.Collections.Generic;
using System.Text;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Handlers
{
        /// <summary>
    /// Manejador de consulta encargado de resolver los datos para detallar corte caja.
    /// </summary>
public class DetallarCorteCajaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<DetallarCorteCajaQry, DetallarCorteCajaDto?>
    {
                /// <inheritdoc/>
public async Task<DetallarCorteCajaDto?> Handle(
            DetallarCorteCajaQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<OpCorteCajaEtt>()
                .Where(c => c.Id == request.Id)
                .Select(c => new DetallarCorteCajaDto
                {
                    Id = c.Id,
                    CtSucu_Clave = c.Nav_IdCtSucursal.Clave,
                    Numero = c.Numero,
                    CtSucu_Nombre = c.Nav_IdCtSucursal.Nombre,
                    CtUsua_Nombre = c.Nav_IdCtUsuario.Nombre + " " +
                        c.Nav_IdCtUsuario.PrimerApellido + " " +
                        c.Nav_IdCtUsuario.SegundoApellido,
                    FechaApertura = c.FechaApertura,
                    FechaCierre = c.FechaCierre,
                    FondoInicial = c.FondoInicial,
                    TotalCobros = c.TotalCobros,
                    TotalCobrosEfectivo = c.TotalCobrosEfectivo,
                    TotalCobrosBancario = c.TotalCobrosBancario,
                    TotalEntregasRepartidor = c.TotalEntregasRepartidor,
                    TotalGastos = c.TotalGastos,
                    TotalGastosEfectivo = c.TotalGastosEfectivo,
                    TotalGastosBancario = c.TotalGastosBancario,
                    TotalPagos = c.TotalPagos,
                    TotalPagosEfectivo = c.TotalPagosEfectivo,
                    TotalPagosBancario = c.TotalPagosBancario,
                    FondoFinalSistema = c.FondoFinalSistema,
                    FondoFinalContado = c.FondoFinalContado,
                    Diferencia = c.Diferencia,
                    TotalVentas = c.TotalVentas,
                    CostoVentasReal = c.CostoVentasReal,
                    CostoVentasRegularizado = c.CostoVentasRegularizado,
                    VentasSinCostear = c.VentasSinCostear,
                    UtilidadBruta = c.UtilidadBruta,
                    UtilidadNeta = c.UtilidadNeta,
                    PorcentajeVentasSinCostear = c.PorcentajeVentasSinCostear,
                    PorcentajeCoberturaGastos = c.PorcentajeCoberturaGastos,
                    GastosAbsorbidosDeCapital = c.GastosAbsorbidosDeCapital,
                    Notas = c.Notas
                });

            var corte = await query.SingleOrDefaultAsync(cancellationToken);
            if (corte == null)
                return null;

            // Mientras la caja está abierta los totales se calculan en vivo a partir
            // de los movimientos ligados; al cerrar se leen de las columnas alamcenadas.
            if (corte.FechaCierre == null)
            {
                await CargarTotalesEnVivo(corte, db, cancellationToken);
            }

            return corte;
        }

        private static async Task CargarTotalesEnVivo(
            DetallarCorteCajaDto corte,
            MatviQryDbCtx db,
            CancellationToken cancellationToken)
        {
            var idCaja = corte.Id;

            var cobros = await db.Set<OpCobroEtt>()
                .Where(c => c.IdOpCorteCaja == idCaja)
                .Select(c => new { c.Id, c.MontoTotal })
                .ToListAsync(cancellationToken);

            var cobrosIds = cobros.Select(c => c.Id).ToList();
            var cobrosBancarios = await db.Set<OpMovimientoBancarioEtt>()
                .Where(m => m.IdOpCobro != null && cobrosIds.Contains(m.IdOpCobro.Value))
                .Select(m => m.IdOpCobro!.Value)
                .ToListAsync(cancellationToken);
            var cobrosBancariosSet = cobrosBancarios.ToHashSet();

            corte.TotalCobros = cobros.Sum(c => c.MontoTotal);
            corte.TotalCobrosEfectivo = cobros
                .Where(c => !cobrosBancariosSet.Contains(c.Id))
                .Sum(c => c.MontoTotal);
            corte.TotalCobrosBancario = cobros
                .Where(c => cobrosBancariosSet.Contains(c.Id))
                .Sum(c => c.MontoTotal);

            var gastos = await db.Set<OpGastoEtt>()
                .Where(g => g.IdOpCorteCaja == idCaja)
                .Select(g => new { g.Id, g.Importe })
                .ToListAsync(cancellationToken);

            var gastosIds = gastos.Select(g => g.Id).ToList();
            var gastosBancarios = await db.Set<OpMovimientoBancarioEtt>()
                .Where(m => m.IdOpGasto != null && gastosIds.Contains(m.IdOpGasto.Value))
                .Select(m => m.IdOpGasto!.Value)
                .ToListAsync(cancellationToken);
            var gastosBancariosSet = gastosBancarios.ToHashSet();

            corte.TotalGastos = gastos.Sum(g => g.Importe);
            corte.TotalGastosEfectivo = gastos
                .Where(g => !gastosBancariosSet.Contains(g.Id))
                .Sum(g => g.Importe);
            corte.TotalGastosBancario = gastos
                .Where(g => gastosBancariosSet.Contains(g.Id))
                .Sum(g => g.Importe);

            var pagos = await db.Set<OpPagoEtt>()
                .Where(p => p.IdOpCorteCaja == idCaja)
                .Select(p => new { p.Id, p.MontoTotal })
                .ToListAsync(cancellationToken);

            var pagosIds = pagos.Select(p => p.Id).ToList();
            var pagosBancarios = await db.Set<OpMovimientoBancarioEtt>()
                .Where(m => m.IdOpPago != null && pagosIds.Contains(m.IdOpPago.Value))
                .Select(m => m.IdOpPago!.Value)
                .ToListAsync(cancellationToken);
            var pagosBancariosSet = pagosBancarios.ToHashSet();

            corte.TotalPagos = pagos.Sum(p => p.MontoTotal);
            corte.TotalPagosEfectivo = pagos
                .Where(p => !pagosBancariosSet.Contains(p.Id))
                .Sum(p => p.MontoTotal);
            corte.TotalPagosBancario = pagos
                .Where(p => pagosBancariosSet.Contains(p.Id))
                .Sum(p => p.MontoTotal);

      

            var detallesCobro = await db.Set<OpCobroDetalleEtt>()
                .Where(cd => cobrosIds.Contains(cd.IdOpCobro))
                .ToListAsync(cancellationToken);

            var totalVentas = detallesCobro.Count > 0
                ? detallesCobro.Sum(cd => cd.MontoAplicado)
                : cobros.Sum(c => c.MontoTotal);

            var pedidosIds = detallesCobro.Select(cd => cd.IdOpPedido).Distinct().ToList();
            var cobrosPorPedido = detallesCobro
                .GroupBy(cd => cd.IdOpPedido)
                .ToDictionary(g => g.Key, g => g.Sum(cd => cd.MontoAplicado));
            var pedidosTotales = await db.Set<OpPedidoEtt>()
                .Where(p => pedidosIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.Total, cancellationToken);

            var lineasDetalle = await db.Set<OpPedidoDetalleEtt>()
                .Where(pd => pedidosIds.Contains(pd.IdOpPedido))
                .ToListAsync(cancellationToken);

            var idsSalida = lineasDetalle
                .Where(pd => pd.IdOpMovimientoInventario_Salida != null)
                .Select(pd => pd.IdOpMovimientoInventario_Salida!.Value)
                .Distinct()
                .ToList();

            var dictCostos = await db.Set<OpMovimientoInventarioEtt>()
                .Where(m => idsSalida.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id, m => m.CostoTotal, cancellationToken);

            decimal costoVentasReal = 0;
            decimal ventasSinCostear = 0;

            foreach (var linea in lineasDetalle)
            {
                var totalPedido = pedidosTotales.GetValueOrDefault(linea.IdOpPedido);
                var fraccion = totalPedido > 0
                    ? Math.Min(1m, cobrosPorPedido.GetValueOrDefault(linea.IdOpPedido) / totalPedido)
                    : 0m;

                if (linea.IdOpMovimientoInventario_Salida != null &&
                    dictCostos.TryGetValue(linea.IdOpMovimientoInventario_Salida.Value, out var costo) &&
                    costo.HasValue)
                {
                    costoVentasReal += costo.Value * fraccion;
                }
                else
                {
                    ventasSinCostear += linea.Importe * fraccion;
                }
            }

            var utilidadBruta = totalVentas - costoVentasReal;
            var utilidadNeta = utilidadBruta - (corte.TotalGastos ?? 0m);

            corte.TotalVentas = totalVentas;
            corte.CostoVentasReal = costoVentasReal;
            corte.CostoVentasRegularizado = costoVentasReal;
            corte.VentasSinCostear = ventasSinCostear;
            corte.PorcentajeVentasSinCostear = totalVentas > 0
                ? Math.Round((ventasSinCostear / totalVentas) * 100, 2)
                : 0m;
            corte.UtilidadBruta = utilidadBruta;
            corte.UtilidadNeta = utilidadNeta;
            corte.PorcentajeCoberturaGastos = (corte.TotalGastos ?? 0m) > 0
                ? (utilidadBruta > 0 ? Math.Round(Math.Min(100, (utilidadBruta / corte.TotalGastos!.Value) * 100), 2) : 0m)
                : 100m;
            corte.GastosAbsorbidosDeCapital = utilidadNeta < 0 ? Math.Abs(utilidadNeta) : 0m;
        }
    }
}