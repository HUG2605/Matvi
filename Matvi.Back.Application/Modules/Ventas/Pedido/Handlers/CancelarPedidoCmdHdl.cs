using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Commands;
using Matvi.Back.Application.Modules.Ventas.Pedido.Common;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Common.Services;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    /// <summary>
    /// Handler para cancelar mercancía de un pedido administrativamente.
    /// Solo permite cancelar cantidades que nunca fueron surtidas (Cantidad - Surtida - Cancelada).
    /// Registra el evento CANCELADO por línea, actualiza los contadores de caché,
    /// libera la reserva correspondiente en RlProductoSucursal si existía y transiciona
    /// el estatus del encabezado según las reglas de negocio.
    /// </summary>
    [Transaction]
    public class CancelarPedidoCmdHdl(
        IUnitOfWork _unitOfWork,
        IConsecutivoService _consecutivoService,
        AjustarEstatusPedidoCmdHdl _ajustarEstatusHdl
    ) : IRequestHandler<CancelarPedidoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            CancelarPedidoCmd request,
            CancellationToken cancellationToken)
        {
            var dto = request.cancelar;

            #region 1. VALIDACIONES PREVIAS

            if (string.IsNullOrWhiteSpace(dto.Motivo))
            {
                return OperationResult<bool>.Info(
                    false,
                    "Debe indicar el motivo de la cancelación.");
            }

            var pedidoEtt = await _unitOfWork.Query<OpPedidoEtt>()
                .Include(p => p.Coll_OpPedidoDetalle_IdOpPedido)
                    .ThenInclude(d => d.Coll_OpPedidoDetalleEvento_IdOpPedidoDetalle)
                .FirstOrDefaultAsync(p => p.Id == dto.Id, cancellationToken);

            if (pedidoEtt == null)
            {
                return OperationResult<bool>.Info(
                    false,
                    $"No se encontró el pedido con Id {dto.Id}.");
            }

            var estatusActual = (EstatusPedidoEnm)pedidoEtt.IdCtEstatusPedido;
            if (estatusActual is EstatusPedidoEnm.ENTREGADO
                or EstatusPedidoEnm.ENTREGADO_PARCIAL
                or EstatusPedidoEnm.RECHAZADO
                or EstatusPedidoEnm.CANCELADO)
            {
                return OperationResult<bool>.Info(
                    false,
                    $"El pedido no puede ser cancelado en su estatus terminal actual ({estatusActual}).");
            }

            var lineasPedido = pedidoEtt.Coll_OpPedidoDetalle_IdOpPedido.ToList();
            if (lineasPedido.Count == 0)
            {
                return OperationResult<bool>.Info(
                    false,
                    "El pedido no contiene líneas para cancelar.");
            }

            #endregion

            #region 2. RESOLVER LÍNEAS Y CANTIDADES A CANCELAR

            var operaciones = new List<(OpPedidoDetalleEtt Detalle, decimal CantidadACancelar)>();

            if (dto.Lineas == null || dto.Lineas.Count == 0)
            {
                // Cancelar todo lo cancelable disponible en todas las líneas
                foreach (var det in lineasPedido)
                {
                    var cancelable = det.Cantidad - det.CantidadSurtida - det.CantidadCancelada;
                    if (cancelable > 0)
                    {
                        operaciones.Add((det, cancelable));
                    }
                }
            }
            else
            {
                var errores = new List<string>();

                foreach (var lDto in dto.Lineas)
                {
                    if (lDto.CantidadACancelar <= 0) continue;

                    var det = lineasPedido.FirstOrDefault(d => d.Id == lDto.IdOpPedidoDetalle);
                    if (det == null)
                    {
                        errores.Add($"La línea {lDto.IdOpPedidoDetalle} no pertenece al pedido.");
                        continue;
                    }

                    var cancelable = det.Cantidad - det.CantidadSurtida - det.CantidadCancelada;
                    if (cancelable <= 0)
                    {
                        errores.Add($"'{det.Cpp_Nombre}': la línea no tiene mercancía cancelable disponible (ya tiene mercancía surtida o cancelada).");
                        continue;
                    }

                    if (lDto.CantidadACancelar > cancelable)
                    {
                        errores.Add($"'{det.Cpp_Nombre}': la cantidad a cancelar ({lDto.CantidadACancelar:F2}) supera la cantidad cancelable disponible ({cancelable:F2}).");
                        continue;
                    }

                    operaciones.Add((det, lDto.CantidadACancelar));
                }

                if (errores.Any())
                {
                    return OperationResult<bool>.Info(
                        false,
                        "No fue posible procesar la cancelación:\n" + string.Join("\n", errores));
                }
            }

            if (operaciones.Count == 0)
            {
                return OperationResult<bool>.Info(
                    false,
                    "No se identificaron cantidades cancelables en las líneas del pedido.");
            }

            #endregion

            #region 3. REGISTRAR EVENTOS DE CANCELACIÓN Y LIBERAR RESERVAS

            var fechaAhora = DateTime.Now;
            var notaEvento = pedidoEtt.TotalPagado > 0
                ? $"{dto.Motivo} (Pedido con saldo pagado de {pedidoEtt.TotalPagado:C2})"
                : dto.Motivo;

            foreach (var (detalle, cantACancelar) in operaciones)
            {
                // 3a. Insertar evento CANCELADO
                var nvoEventoCancelado = new OpPedidoDetalleEventoEtt
                {
                    IdOpPedidoDetalle = detalle.Id,
                    IdCtTipoEventoPedidoDetalle = (byte)TipoEventoPedidoDetalleEnm.CANCELADO,
                    Cantidad = cantACancelar,
                    Fecha = fechaAhora,
                    IdCtUsuario = dto.IdCtUsuario,
                    Notas = notaEvento
                };
                _unitOfWork.Add(nvoEventoCancelado);

                // 3b. Liberar reserva en RlProductoSucursal (si la línea tenía stock reservado)
                // reservadoLínea = max(0, ΣRESERVADO − ΣSURTIDO − ΣCANCELADO_PREVIO)
                var eventosLinea = detalle.Coll_OpPedidoDetalleEvento_IdOpPedidoDetalle.ToList();

                var sumaReservado = eventosLinea
                    .Where(e => e.IdCtTipoEventoPedidoDetalle == (byte)TipoEventoPedidoDetalleEnm.RESERVADO)
                    .Sum(e => e.Cantidad);

                var sumaSurtido = eventosLinea
                    .Where(e => e.IdCtTipoEventoPedidoDetalle == (byte)TipoEventoPedidoDetalleEnm.SURTIDO)
                    .Sum(e => e.Cantidad);

                var sumaCanceladoPrevio = eventosLinea
                    .Where(e => e.IdCtTipoEventoPedidoDetalle == (byte)TipoEventoPedidoDetalleEnm.CANCELADO)
                    .Sum(e => e.Cantidad);

                var reservadoLinea = Math.Max(0m, sumaReservado - sumaSurtido - sumaCanceladoPrevio);
                var cantALiberarPres = Math.Min(reservadoLinea, cantACancelar);
                var cantALiberarUBI = cantALiberarPres * detalle.Cpp_FactorUBI;

                if (cantALiberarUBI > 0)
                {
                    var rel = await _unitOfWork.Query<RlProductoSucursalEtt>()
                        .FirstOrDefaultAsync(r => r.IdCtSucursal == pedidoEtt.IdCtSucursal
                                               && r.IdCtProducto == detalle.Cpp_IdCtProducto, cancellationToken);

                    if (rel != null && rel.StockReservadoUBI > 0)
                    {
                        rel.StockReservadoUBI = Math.Max(0m, rel.StockReservadoUBI - cantALiberarUBI);
                        _unitOfWork.Update(rel);
                    }
                }

                // 3c. Actualizar cachés y estatus de la línea
                detalle.CantidadCancelada += cantACancelar;
                detalle.IdCtEstatusPedidoDetalle = (byte)LineaPedidoCalc.CalcularEstatusLinea(detalle);
                _unitOfWork.Update(detalle);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region 4. DEVOLUCIÓN EN CAJA (si el pedido tenía saldo pagado)

            if (pedidoEtt.TotalPagado > 0)
            {
                // Buscar la caja abierta: primero por el Id que envió el frontend,
                // y si no, buscar la única caja abierta del usuario en esa sucursal.
                int? idCajaParaDevolucion = null;

                if (dto.IdOpCorteCaja.HasValue)
                {
                    var cajaValida = _unitOfWork.Query<OpCorteCajaEtt>()
                        .Any(c =>
                            c.Id == dto.IdOpCorteCaja.Value &&
                            c.IdCtSucursal == pedidoEtt.IdCtSucursal &&
                            c.FechaCierre == null);

                    if (cajaValida)
                        idCajaParaDevolucion = dto.IdOpCorteCaja.Value;
                }

                if (!idCajaParaDevolucion.HasValue)
                {
                    idCajaParaDevolucion = _unitOfWork.Query<OpCorteCajaEtt>()
                        .Where(c =>
                            c.IdCtSucursal == pedidoEtt.IdCtSucursal &&
                            c.IdCtUsuario == dto.CtUsua_Id &&
                            c.FechaCierre == null)
                        .Select(c => (int?)c.Id)
                        .FirstOrDefault();
                }

                if (!idCajaParaDevolucion.HasValue)
                {
                    return OperationResult<bool>.Info(
                        false,
                        "La cancelación no pudo completarse: el pedido tiene saldo pagado " +
                        $"({pedidoEtt.TotalPagado:C2}) pero no se encontró una caja abierta " +
                        "para registrar la devolución. Abra su corte de caja e intente de nuevo.");
                }

                // Registrar el gasto de devolución (egreso de caja)
                const byte TIPO_GASTO_DEVOLUCIONES = 26;  // GASTOS POR DEVOLUCIONES
                const byte MEDIO_PAGO_EFECTIVO = 1;

                var numeroGasto = await _consecutivoService.GenerarNumeroAsync(
                    pedidoEtt.IdCtSucursal, (byte)TipoConsecutivoEnm.GASTO, cancellationToken);

                var nvoGastoDevolucion = new OpGastoEtt
                {
                    IdCtSucursal   = pedidoEtt.IdCtSucursal,
                    Numero         = numeroGasto,
                    FechaPago      = fechaAhora,
                    IdCtTipoGasto  = TIPO_GASTO_DEVOLUCIONES,
                    Proveedor      = $"DEVOLUCIÓN PEDIDO #{pedidoEtt.Numero}",
                    Concepto       = $"Devolución por cancelación de pedido #{pedidoEtt.Numero}. Motivo: {dto.Motivo}",
                    Importe        = pedidoEtt.TotalPagado,
                    IdCtMedioPago  = MEDIO_PAGO_EFECTIVO,
                    IdCtUsuario    = dto.CtUsua_Id,
                    IdOpCorteCaja  = idCajaParaDevolucion.Value,
                    Notas          = dto.Motivo
                };

                _unitOfWork.Add(nvoGastoDevolucion);

                // Reflejar la devolución en el pedido: el saldo pagado queda en cero
                pedidoEtt.TotalPagado = 0;
                _unitOfWork.Update(pedidoEtt);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            #endregion

            #region 5. DETERMINAR ESTATUS FINAL DEL HEADER

            var estatusFinal = LineaPedidoCalc.DeterminarEstatusFinalHeader(lineasPedido);

            if (estatusFinal.HasValue)
            {
                if (estatusFinal.Value is EstatusPedidoEnm.ENTREGADO_PARCIAL or EstatusPedidoEnm.CANCELADO)
                {
                    pedidoEtt.Total = lineasPedido.Sum(d => d.CantidadEntregada * d.Precio);
                    _unitOfWork.Update(pedidoEtt);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                if (pedidoEtt.IdCtEstatusPedido != (byte)estatusFinal.Value)
                {
                    var notasAjuste = estatusFinal.Value == EstatusPedidoEnm.CANCELADO
                        ? $"Pedido cancelado administrativamente. Motivo: {dto.Motivo}"
                        : (estatusFinal.Value == EstatusPedidoEnm.ENTREGADO
                            ? "Pedido cerrado con entregas completadas tras cancelación"
                            : "Pedido cerrado parcialmente con cancelaciones");

                    var resAjuste = await _ajustarEstatusHdl.Handle(
                        new AjustarEstatusPedidoCmd(
                            new AjustarEstatusPedidoDto
                            {
                                Id = pedidoEtt.Id,
                                NuevoEstatus = estatusFinal.Value,
                                Notas = notasAjuste,
                                CtUsua_Id = dto.IdCtUsuario
                            }),
                        cancellationToken);

                    if (!resAjuste.IsSuccessful)
                    {
                        return OperationResult<bool>.Info(
                            false,
                            resAjuste.Message ?? "No fue posible actualizar el estatus del pedido.");
                    }
                }
            }

            #endregion

            return OperationResult<bool>.Success(
                true,
                estatusFinal == EstatusPedidoEnm.CANCELADO
                    ? "El pedido ha sido cancelado exitosamente."
                    : "La cancelación de mercancía ha sido registrada exitosamente.");
        }
    }
}
