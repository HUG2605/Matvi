using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Commands;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Dtos;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;


namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Handlers
{
    [Transaction]
    class CerrarCorteCajaCmdHdl(
        IUnitOfWork _unitOfWork
    ) : IRequestHandler<CerrarCorteCajaCmd, OperationResult<CerrarCorteCajaResDto>>
    {
        /// <summary>
        /// 1. Validar caja existente, abierta y del usuario/sucursal actual
        /// 2. Ligado de respaldo: asignar IdOpCorteCaja a los cobros/gastos/pagos
        ///    del usuario dentro del periodo que aún no pertenezcan a una caja
        /// 3. Calcular totales y FondoFinalSistema = FondoInicial + Cobros - Gastos - Pagos
        /// 4. Diferencia = FondoFinalContado - FondoFinalSistema
        /// 5. Guardar FechaCierre y totales
        /// </summary>
        public async Task<OperationResult<CerrarCorteCajaResDto>> Handle(
            CerrarCorteCajaCmd request,
            CancellationToken cancellationToken)
        {
            var corteDto = request.corte;
            var fechaAhora = DateTime.Now;

            #region VALIDAR CAJA

            var caja = _unitOfWork.Query<OpCorteCajaEtt>()
                .FirstOrDefault(c => c.Id == corteDto.Id);

            if (caja == null)
                return OperationResult<CerrarCorteCajaResDto>.Error("La caja no existe.");

            if (caja.IdCtSucursal != corteDto.CtSucu_Id ||
                caja.IdCtUsuario != corteDto.CtUsua_Id)
                return OperationResult<CerrarCorteCajaResDto>.Error(
                    "La caja no pertenece al usuario o a la sucursal actual.");

            if (caja.FechaCierre != null)
                return OperationResult<CerrarCorteCajaResDto>.Warning(null!,
                    "La caja ya está cerrada.");

            #endregion

            #region LIGAR MOVIMIENTOS DEL PERIODO A LA CAJA

            var cobrosPeriodo = _unitOfWork.Query<OpCobroEtt>()
                .Where(c =>
                    c.IdCtSucursal == caja.IdCtSucursal &&
                    c.IdCtUsuarioRecibe == caja.IdCtUsuario &&
                    c.Fecha >= caja.FechaApertura &&
                    c.Fecha <= fechaAhora)
                .ToList();

            foreach (var cobro in cobrosPeriodo)
            {
                if (cobro.IdOpCorteCaja == null)
                {
                    cobro.IdOpCorteCaja = caja.Id;
                    _unitOfWork.Update(cobro);
                }
            }

            var gastosPeriodo = _unitOfWork.Query<OpGastoEtt>()
                .Where(c =>
                    c.IdCtSucursal == caja.IdCtSucursal &&
                    c.IdCtUsuario == caja.IdCtUsuario &&
                    c.FechaPago >= caja.FechaApertura &&
                    c.FechaPago <= fechaAhora)
                .ToList();

            foreach (var gasto in gastosPeriodo)
            {
                if (gasto.IdOpCorteCaja == null)
                {
                    gasto.IdOpCorteCaja = caja.Id;
                    _unitOfWork.Update(gasto);
                }
            }

            var pagosPeriodo = _unitOfWork.Query<OpPagoEtt>()
                .Where(c =>
                    c.IdCtSucursal == caja.IdCtSucursal &&
                    c.IdCtUsuarioPaga == caja.IdCtUsuario &&
                    c.Fecha >= caja.FechaApertura &&
                    c.Fecha <= fechaAhora)
                .ToList();

            foreach (var pago in pagosPeriodo)
            {
                if (pago.IdOpCorteCaja == null)
                {
                    pago.IdOpCorteCaja = caja.Id;
                    _unitOfWork.Update(pago);
                }
            }


            #endregion

            #region CALCULAR TOTALES Y DESGLOSE (EFECTIVO VS BANCARIO)

            // Cobros
            var cobrosIds = cobrosPeriodo.Where(c => c.IdOpCorteCaja == caja.Id).Select(c => c.Id).ToList();
            var cobrosBancariosIds = _unitOfWork.Query<OpMovimientoBancarioEtt>()
                .Where(m => m.IdOpCobro != null && cobrosIds.Contains(m.IdOpCobro.Value))
                .Select(m => m.IdOpCobro!.Value)
                .ToHashSet();

            decimal totalCobrosEfectivo = 0;
            decimal totalCobrosBancario = 0;
            foreach (var c in cobrosPeriodo.Where(c => c.IdOpCorteCaja == caja.Id))
            {
                if (cobrosBancariosIds.Contains(c.Id))
                    totalCobrosBancario += c.MontoTotal;
                else
                    totalCobrosEfectivo += c.MontoTotal;
            }
            var totalCobros = totalCobrosEfectivo + totalCobrosBancario;

            // Gastos
            var gastosIds = gastosPeriodo.Where(g => g.IdOpCorteCaja == caja.Id).Select(g => g.Id).ToList();
            var gastosBancariosIds = _unitOfWork.Query<OpMovimientoBancarioEtt>()
                .Where(m => m.IdOpGasto != null && gastosIds.Contains(m.IdOpGasto.Value))
                .Select(m => m.IdOpGasto!.Value)
                .ToHashSet();

            decimal totalGastosEfectivo = 0;
            decimal totalGastosBancario = 0;
            foreach (var g in gastosPeriodo.Where(g => g.IdOpCorteCaja == caja.Id))
            {
                if (gastosBancariosIds.Contains(g.Id))
                    totalGastosBancario += g.Importe;
                else
                    totalGastosEfectivo += g.Importe;
            }
            var totalGastos = totalGastosEfectivo + totalGastosBancario;

            // Pagos
            var pagosIds = pagosPeriodo.Where(p => p.IdOpCorteCaja == caja.Id).Select(p => p.Id).ToList();
            var pagosBancariosIds = _unitOfWork.Query<OpMovimientoBancarioEtt>()
                .Where(m => m.IdOpPago != null && pagosIds.Contains(m.IdOpPago.Value))
                .Select(m => m.IdOpPago!.Value)
                .ToHashSet();

            decimal totalPagosEfectivo = 0;
            decimal totalPagosBancario = 0;
            foreach (var p in pagosPeriodo.Where(p => p.IdOpCorteCaja == caja.Id))
            {
                if (pagosBancariosIds.Contains(p.Id))
                    totalPagosBancario += p.MontoTotal;
                else
                    totalPagosEfectivo += p.MontoTotal;
            }
            var totalPagos = totalPagosEfectivo + totalPagosBancario;

            // Fondo final en caja física: FondoInicial + Cobros(efectivo) - Gastos(efectivo) - Pagos(efectivo)
            var fondoFinalSistema = caja.FondoInicial + totalCobrosEfectivo - totalGastosEfectivo - totalPagosEfectivo;
            var diferencia = corteDto.FondoFinalContado - fondoFinalSistema;

            #endregion

            #region CALCULAR VENTAS Y UTILIDADES

            // La utilidad se reconoce de forma proporcional al pago: si un pedido
            // se vende a crédito, cada corte reconoce "monto cobrado en el corte /
            // total del pedido" de sus ventas y de su costo. Así el costo nunca se
            // duplica entre cortes y la utilidad acumulada coincide con la real.

            var detallesCobro = _unitOfWork.Query<OpCobroDetalleEtt>()
                .Where(cd => cobrosIds.Contains(cd.IdOpCobro))
                .ToList();

            var pedidosIds = detallesCobro.Select(cd => cd.IdOpPedido).Distinct().ToList();

            decimal totalVentas = detallesCobro.Count > 0 ? detallesCobro.Sum(cd => cd.MontoAplicado) : totalCobros;

            var cobrosPorPedido = detallesCobro
                .GroupBy(cd => cd.IdOpPedido)
                .ToDictionary(g => g.Key, g => g.Sum(cd => cd.MontoAplicado));

            var pedidosTotales = _unitOfWork.Query<OpPedidoEtt>()
                .Where(p => pedidosIds.Contains(p.Id))
                .ToDictionary(p => p.Id, p => p.Total);

            var lineasDetalle = _unitOfWork.Query<OpPedidoDetalleEtt>()
                .Where(pd => pedidosIds.Contains(pd.IdOpPedido))
                .ToList();

            var idsSalida = lineasDetalle
                .Where(pd => pd.IdOpMovimientoInventario_Salida != null)
                .Select(pd => pd.IdOpMovimientoInventario_Salida!.Value)
                .Distinct()
                .ToList();

            var movimientosSalida = _unitOfWork.Query<OpMovimientoInventarioEtt>()
                .Where(m => idsSalida.Contains(m.Id))
                .ToList();

            var dictCostos = movimientosSalida.ToDictionary(m => m.Id, m => m.CostoTotal);

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

            decimal porcentajeVentasSinCostear = (totalVentas > 0)
                ? Math.Round((ventasSinCostear / totalVentas) * 100, 2)
                : 0;

            decimal utilidadBruta = totalVentas - costoVentasReal;
            decimal utilidadNeta = utilidadBruta - totalGastos;

            // Cobertura de gastos: qué % del total de gastos es absorbido por el
            // margen de la mercancía. Si los gastos superan la utilidad bruta, el
            // excedente se toma del costo de mercancía / capital del negocio.
            decimal porcentajeCoberturaGastos = totalGastos > 0
                ? (utilidadBruta > 0 ? Math.Round(Math.Min(100, (utilidadBruta / totalGastos) * 100), 2) : 0m)
                : 100m;
            decimal gastosAbsorbidosDeCapital = utilidadNeta < 0 ? Math.Abs(utilidadNeta) : 0m;

            #endregion

            #region ACTUALIZAR LA CAJA

            caja.FechaCierre = fechaAhora;
            caja.FondoFinalContado = corteDto.FondoFinalContado;
            caja.TotalCobros = totalCobros;
            caja.TotalCobrosEfectivo = totalCobrosEfectivo;
            caja.TotalCobrosBancario = totalCobrosBancario;
            caja.TotalEntregasRepartidor = 0;
            caja.TotalGastos = totalGastos;
            caja.TotalGastosEfectivo = totalGastosEfectivo;
            caja.TotalGastosBancario = totalGastosBancario;
            caja.TotalPagos = totalPagos;
            caja.TotalPagosEfectivo = totalPagosEfectivo;
            caja.TotalPagosBancario = totalPagosBancario;
            caja.FondoFinalSistema = fondoFinalSistema;
            caja.Diferencia = diferencia;
            caja.TotalVentas = totalVentas;
            caja.CostoVentasReal = costoVentasReal;
            caja.CostoVentasRegularizado = costoVentasReal;
            caja.VentasSinCostear = ventasSinCostear;
            caja.PorcentajeVentasSinCostear = porcentajeVentasSinCostear;
            caja.UtilidadBruta = utilidadBruta;
            caja.UtilidadNeta = utilidadNeta;
            caja.PorcentajeCoberturaGastos = porcentajeCoberturaGastos;
            caja.GastosAbsorbidosDeCapital = gastosAbsorbidosDeCapital;
            caja.Notas = corteDto.Notas;

            _unitOfWork.Update(caja);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            return OperationResult<CerrarCorteCajaResDto>.Success(
                new CerrarCorteCajaResDto
                {
                    TotalCobros = totalCobros,
                    TotalEntregasRepartidor = 0,
                    TotalGastos = totalGastos,
                    TotalPagos = totalPagos,
                    FondoFinalSistema = fondoFinalSistema,
                    FondoFinalContado = corteDto.FondoFinalContado,
                    Diferencia = diferencia
                },
                "La caja se cerró correctamente.");
        }
    }
}