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
    /// Handler para registrar la entrega a domicilio de un pedido por el repartidor.
    ///
    /// Por cada línea del pedido recibe cuánto se entregó al cliente y cuánto se
    /// rechazó. Registra los eventos ENTREGADO y/o RECHAZADO en OpPedidoDetalleEvento,
    /// actualiza los cachés de la línea y transiciona el estatus del header:
    ///   - Todas las líneas entregadas → ENTREGADO.
    ///   - Rechazo total (nada entregado)  → RECHAZADO.
    ///   - Entrega parcial o rechazos mezclados → ENTREGADO_PARCIAL.
    ///
    /// El rechazo genera movimientos de inventario de reingreso (ENT_REINGRESO)
    /// vinculados al evento RECHAZADO, reintegrando el stock a los lotes originales
    /// de salida vía FIFO y registrando los costos correspondientes.
    /// </summary>
    [Transaction]
    public class EntregarPedidoCmdHdl(
        IUnitOfWork _unitOfWork,
        IConsecutivoService _consecutivoService,
        AjustarEstatusPedidoCmdHdl _ajustarEstatusHdl
    ) : IRequestHandler<EntregarPedidoCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EntregarPedidoCmd request,
            CancellationToken cancellationToken)
        {
            var dto = request.entrega;

            #region 1. VALIDACIONES PREVIAS

            if (dto.Lineas == null || dto.Lineas.Count == 0)
            {
                return OperationResult<bool>.Info(
                    false,
                    "Debe indicar al menos una línea con cantidades entregadas o rechazadas.");
            }

            var lineaConCantidad = dto.Lineas
                .FirstOrDefault(l => l.CantidadEntregar > 0 || l.CantidadRechazar > 0);

            if (lineaConCantidad == null)
            {
                return OperationResult<bool>.Info(
                    false,
                    "Debe capturar cantidades entregadas o rechazadas en al menos una línea.");
            }

            foreach (var linea in dto.Lineas)
            {
                if (linea.CantidadEntregar < 0 || linea.CantidadRechazar < 0)
                {
                    return OperationResult<bool>.Info(
                        false,
                        "Las cantidades entregadas o rechazadas no pueden ser negativas.");
                }

                var totalLinea = linea.CantidadEntregar + linea.CantidadRechazar;
                if (totalLinea == 0)
                {
                    continue;
                }

                if (linea.CantidadRechazar > 0 && string.IsNullOrWhiteSpace(linea.NotasRechazo))
                {
                    return OperationResult<bool>.Info(
                        false,
                        "Debe indicar el motivo del rechazo cuando hay cantidad rechazada.");
                }
            }

            // 1b. Cargar pedido incluyendo sus líneas de detalle
            var pedidoEtt = await _unitOfWork
                .Query<OpPedidoEtt>()
                .Include(p => p.Coll_OpPedidoDetalle_IdOpPedido)
                .FirstOrDefaultAsync(p => p.Id == dto.Id, cancellationToken);

            if (pedidoEtt == null)
            {
                return OperationResult<bool>.Info(
                    false,
                    $"No se encontró el pedido con Id {dto.Id}.");
            }

            // 1c. El pedido debe estar EN_PROCESO (surtido y bajo custodia del repartidor)
            if (pedidoEtt.IdCtEstatusPedido != (byte)EstatusPedidoEnm.EN_PROCESO)
            {
                return OperationResult<bool>.Info(
                    false,
                    $"El pedido no se puede entregar en su estatus actual. " +
                    $"Estatus esperado: EN_PROCESO, estatus actual: {pedidoEtt.IdCtEstatusPedido}.");
            }

            // 1d. Debe estar asignado al repartidor que reporta la entrega
            if (pedidoEtt.IdCtUsuarioRepartidor != dto.IdCtUsuarioRepartidor)
            {
                return OperationResult<bool>.Info(
                    false,
                    "Este pedido no está asignado a tu usuario, por lo que no puedes registrarle la entrega.");
            }

            #endregion

            #region 2. VALIDAR CANTIDADES VS CUSTODIA E INDICES DE LÍNEA

            var lineasPedido = pedidoEtt.Coll_OpPedidoDetalle_IdOpPedido.ToList();

            var lineasFaltantes = new List<string>();
            foreach (var ioPd in dto.Lineas)
            {
                if (!lineasPedido.Any(d => d.Id == ioPd.IdOpPedidoDetalle))
                {
                    lineasFaltantes.Add($"línea {ioPd.IdOpPedidoDetalle}");
                }
            }

            if (lineasFaltantes.Any())
            {
                return OperationResult<bool>.Info(
                    false,
                    "Una o más líneas no pertenecen al pedido: " + string.Join(", ", lineasFaltantes) + ".");
            }

            var errores = new List<string>();

            foreach (var lineaDto in dto.Lineas)
            {
                var detalle = lineasPedido.First(d => d.Id == lineaDto.IdOpPedidoDetalle);

                var cantidadEntregada = detalle.CantidadEntregada;
                var cantidadRechazada = detalle.CantidadRechazada;
                var custodia = detalle.CantidadSurtida - cantidadEntregada - cantidadRechazada;

                var totalAOperar =
                    lineaDto.CantidadEntregar
                    + lineaDto.CantidadRechazar;

                if (totalAOperar == 0) continue;

                if (custodia <= 0)
                {
                    errores.Add(
                        $"'{detalle.Cpp_Nombre}': no hay cantidad bajo custodia del repartidor.");
                    continue;
                }

                if (totalAOperar > custodia)
                {
                    errores.Add(
                        $"'{detalle.Cpp_Nombre}': entregar/rechazar {totalAOperar:F2} " +
                        $"supera la custodia disponible {custodia:F2}.");
                }
            }

            if (dto.Lineas.Any(l => l.CantidadRechazar > 0) && pedidoEtt.TotalPagado > 0)
            {
                return OperationResult<bool>.Info(
                    false,
                    "No es posible registrar rechazo de mercancía en un pedido que cuenta con cobros o anticipos registrados.");
            }

            if (errores.Any())
            {
                return OperationResult<bool>.Info(
                    false,
                    "No es posible registrar la entrega:\n" + string.Join("\n", errores));
            }

            #endregion

            #region 3. REGISTRAR EVENTOS POR LÍNEA

            var fechaAhora = DateTime.Now;

            foreach (var lineaDto in dto.Lineas)
            {
                var detalle = lineasPedido.First(d => d.Id == lineaDto.IdOpPedidoDetalle);

                if (lineaDto.CantidadEntregar == 0 && lineaDto.CantidadRechazar == 0)
                    continue;

                // Registrar evento RECHAZADO y reingreso FIFO multi-lote
                if (lineaDto.CantidadRechazar > 0)
                {
                    var nvoEventoRechazado = new OpPedidoDetalleEventoEtt
                    {
                        IdOpPedidoDetalle = detalle.Id,
                        IdCtTipoEventoPedidoDetalle = (byte)TipoEventoPedidoDetalleEnm.RECHAZADO,
                        Cantidad = lineaDto.CantidadRechazar,
                        Fecha = fechaAhora.AddMilliseconds(200),
                        IdCtUsuario = dto.IdCtUsuarioRepartidor,
                        IdCtUsuarioRecibe = null,
                        Notas = $"Rechazado por el cliente. Motivo: {lineaDto.NotasRechazo}"
                    };
                    _unitOfWork.Add(nvoEventoRechazado);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    var cantidadRechazarUBIPendiente = lineaDto.CantidadRechazar * detalle.Cpp_FactorUBI;

                    // Traer movimientos SAL_VENTA de la línea ordenados por Fecha / Id
                    var movimientosSalida = await _unitOfWork.Query<OpMovimientoInventarioEtt>()
                        .Where(m => m.Nav_IdOpPedidoDetalleEvento != null
                                 && m.Nav_IdOpPedidoDetalleEvento.IdOpPedidoDetalle == detalle.Id
                                 && m.IdCtTipoMovimiento == (byte)TipoMovimientoEnm.SAL_VENTA)
                        .OrderBy(m => m.Fecha)
                        .ThenBy(m => m.Id)
                        .ToListAsync(cancellationToken);

                    if (movimientosSalida.Count == 0)
                    {
                        return OperationResult<bool>.Info(
                            false,
                            $"'{detalle.Cpp_Nombre}': no se encontraron movimientos de salida de inventario para reingresar el rechazo.");
                    }

                    // Traer reingresos previos para descontar lo ya retornado por lote
                    var reingresosPrevios = await _unitOfWork.Query<OpMovimientoInventarioEtt>()
                        .Where(m => m.Nav_IdOpPedidoDetalleEvento != null
                                 && m.Nav_IdOpPedidoDetalleEvento.IdOpPedidoDetalle == detalle.Id
                                 && m.IdCtTipoMovimiento == (byte)TipoMovimientoEnm.ENT_REINGRESO)
                        .ToListAsync(cancellationToken);

                    var reingresadoPorLote = reingresosPrevios
                        .GroupBy(r => r.IdOpLote)
                        .ToDictionary(g => g.Key, g => g.Sum(r => r.CantidadUBI));

                    var salidasPorLote = movimientosSalida
                        .GroupBy(m => m.IdOpLote)
                        .Select(g => new
                        {
                            IdOpLote = g.Key,
                            TotalSalidaUBI = g.Sum(m => m.CantidadUBI),
                            CostoUBI = g.First().CostoUBI
                        })
                        .ToList();

                    foreach (var grupoLote in salidasPorLote)
                    {
                        if (cantidadRechazarUBIPendiente <= 0) break;

                        var yaReingresado = reingresadoPorLote.GetValueOrDefault(grupoLote.IdOpLote, 0m);
                        var disponibleParaReingreso = grupoLote.TotalSalidaUBI - yaReingresado;

                        if (disponibleParaReingreso <= 0) continue;

                        var cantidadATomarUBI = Math.Min(cantidadRechazarUBIPendiente, disponibleParaReingreso);

                        var lote = await _unitOfWork.FindAsync<OpLoteEtt, int>(grupoLote.IdOpLote, cancellationToken);
                        if (lote != null)
                        {
                            lote.CantidadDisponibleUBI += cantidadATomarUBI;
                            _unitOfWork.Update(lote);
                        }

                        var numeroMovimiento = await _consecutivoService.GenerarNumeroAsync(
                            pedidoEtt.IdCtSucursal,
                            (byte)TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO,
                            cancellationToken);

                        var costoUnit = grupoLote.CostoUBI ?? lote?.CostoUBI ?? 0m;

                        var movReingreso = new OpMovimientoInventarioEtt
                        {
                            IdCtSucursal = pedidoEtt.IdCtSucursal,
                            Numero = numeroMovimiento,
                            Cpp_IdCtProducto = detalle.Cpp_IdCtProducto,
                            Cpp_Nombre = detalle.Cpp_Nombre,
                            Cpp_FactorUBI = detalle.Cpp_FactorUBI,
                            CantidadUBI = cantidadATomarUBI,
                            Fecha = fechaAhora,
                            IdCtTipoMovimiento = (byte)TipoMovimientoEnm.ENT_REINGRESO,
                            IdOpPedidoDetalleEvento = nvoEventoRechazado.Id,
                            IdOpLote = grupoLote.IdOpLote,
                            CostoUBI = costoUnit,
                            CostoTotal = cantidadATomarUBI * costoUnit,
                            IdCtUsuario = dto.IdCtUsuarioRepartidor
                        };

                        _unitOfWork.Add(movReingreso);
                        cantidadRechazarUBIPendiente -= cantidadATomarUBI;
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    detalle.CantidadRechazada += lineaDto.CantidadRechazar;
                }

                // Registrar evento ENTREGADO
                if (lineaDto.CantidadEntregar > 0)
                {
                    var nvoEventoEntregado = new OpPedidoDetalleEventoEtt
                    {
                        IdOpPedidoDetalle = detalle.Id,
                        IdCtTipoEventoPedidoDetalle = (byte)TipoEventoPedidoDetalleEnm.ENTREGADO,
                        Cantidad = lineaDto.CantidadEntregar,
                        Fecha = fechaAhora.AddMilliseconds(100),
                        IdCtUsuario = dto.IdCtUsuarioRepartidor,
                        Notas = "Entregado al cliente en domicilio"
                    };
                    _unitOfWork.Add(nvoEventoEntregado);

                    detalle.CantidadEntregada += lineaDto.CantidadEntregar;
                }

                detalle.IdCtEstatusPedidoDetalle = (byte)LineaPedidoCalc.CalcularEstatusLinea(detalle);
                _unitOfWork.Update(detalle);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region 4. DETERMINAR ESTATUS FINAL DEL HEADER

            var estatusDestino = LineaPedidoCalc.DeterminarEstatusFinalHeader(lineasPedido);

            if (estatusDestino.HasValue)
            {
                if (estatusDestino.Value is EstatusPedidoEnm.ENTREGADO_PARCIAL or EstatusPedidoEnm.RECHAZADO)
                {
                    pedidoEtt.Total = lineasPedido.Sum(d => d.CantidadEntregada * d.Precio);
                    _unitOfWork.Update(pedidoEtt);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                var resAjuste = await _ajustarEstatusHdl.Handle(
                    new AjustarEstatusPedidoCmd(
                        new AjustarEstatusPedidoDto
                        {
                            Id = pedidoEtt.Id,
                            NuevoEstatus = estatusDestino.Value,
                            Notas = estatusDestino.Value == EstatusPedidoEnm.ENTREGADO
                                ? "Pedido entregado completamente por el repartidor"
                                : estatusDestino.Value == EstatusPedidoEnm.RECHAZADO
                                    ? "Pedido rechazado en su totalidad por el cliente"
                                    : "Entrega parcial con rechazos o cancelaciones",
                            CtUsua_Id = dto.IdCtUsuarioRepartidor
                        }),
                    cancellationToken);

                if (!resAjuste.IsSuccessful)
                {
                    return OperationResult<bool>.Info(false, resAjuste.Message ?? "No fue posible ajustar el estatus del pedido.");
                }
            }
            else
            {
                // El pedido continúa en EN_PROCESO ya que todavía hay líneas o remanentes por entregar/surtir
            }

            #endregion

            #region 5. REGISTRAR COBRO EN ENTREGA (SI APLICA)

            if (dto.MontoCobro.HasValue && dto.MontoCobro.Value > 0)
            {
                if (!dto.IdCtMedioPago.HasValue)
                {
                    return OperationResult<bool>.Info(
                        false,
                        "Debe seleccionar el medio de pago para registrar el cobro.");
                }

                var saldoPendientePedido = pedidoEtt.Total - pedidoEtt.TotalPagado;
                if (dto.MontoCobro.Value > saldoPendientePedido)
                {
                    return OperationResult<bool>.Info(
                        false,
                        $"El monto a cobrar (${dto.MontoCobro.Value:N2}) excede el saldo pendiente del pedido (${saldoPendientePedido:N2}).");
                }

                var cajaActiva = await _unitOfWork.Query<OpCorteCajaEtt>()
                    .FirstOrDefaultAsync(c =>
                        c.IdCtSucursal == pedidoEtt.IdCtSucursal &&
                        c.IdCtUsuario == dto.IdCtUsuarioRepartidor &&
                        c.FechaCierre == null, cancellationToken);

                var numeroCobro = await _consecutivoService.GenerarNumeroAsync(
                    pedidoEtt.IdCtSucursal,
                    (byte)TipoConsecutivoEnm.COBRO,
                    cancellationToken);

                var nvoCobroEtt = new OpCobroEtt
                {
                    IdCtSucursal = pedidoEtt.IdCtSucursal,
                    Numero = numeroCobro,
                    Fecha = fechaAhora,
                    IdCtCliente = pedidoEtt.IdCtCliente,
                    MontoTotal = dto.MontoCobro.Value,
                    IdCtMedioPago = dto.IdCtMedioPago.Value,
                    IdCtUsuarioRecibe = dto.IdCtUsuarioRepartidor,
                    IdOpCorteCaja = cajaActiva?.Id,
                    Notas = string.IsNullOrWhiteSpace(dto.NotasCobro)
                        ? $"Cobro registrado en la entrega del pedido #{pedidoEtt.Numero}"
                        : dto.NotasCobro
                };

                _unitOfWork.Add(nvoCobroEtt);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var nvoDetalleCobroEtt = new OpCobroDetalleEtt
                {
                    IdOpCobro = nvoCobroEtt.Id,
                    IdOpPedido = pedidoEtt.Id,
                    MontoAplicado = dto.MontoCobro.Value
                };

                _unitOfWork.Add(nvoDetalleCobroEtt);
                pedidoEtt.TotalPagado += dto.MontoCobro.Value;
                _unitOfWork.Update(pedidoEtt);

                // Movimiento bancario si el medio de pago es banco/transferencia/tarjeta o si especificó cuenta
                byte? idCuentaBancaria = dto.IdCtCuentaBancaria;
                if (!idCuentaBancaria.HasValue && dto.IdCtMedioPago.Value != 1)
                {
                    idCuentaBancaria = await _unitOfWork.Query<CtCuentaBancariaEtt>()
                        .Where(c => c.Activa)
                        .OrderBy(c => c.Id)
                        .Select(c => (byte?)c.Id)
                        .FirstOrDefaultAsync(cancellationToken);
                }

                if (idCuentaBancaria.HasValue && (dto.IdCtMedioPago.Value != 1 || dto.IdCtCuentaBancaria.HasValue))
                {
                    var nvoMovimientoBancario = new OpMovimientoBancarioEtt
                    {
                        IdCtCuentaBancaria = idCuentaBancaria.Value,
                        Fecha = fechaAhora,
                        Tipo = (byte)TipoMovimientoBancarioEnm.INGRESO,
                        Monto = dto.MontoCobro.Value,
                        Referencia = $"COBRO #{numeroCobro}",
                        Conciliado = false,
                        IdOpCobro = nvoCobroEtt.Id,
                        IdOpGasto = null,
                        IdOpPago = null
                    };

                    _unitOfWork.Add(nvoMovimientoBancario);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            #endregion

            #region 6. NOTA SOBRE CUSTODIA RESIDUAL

            var custodiaResidual = lineasPedido.Sum(d =>
                d.CantidadSurtida
                - d.CantidadEntregada
                - d.CantidadRechazada);

            var mensaje = estatusDestino switch
            {
                EstatusPedidoEnm.ENTREGADO => "El pedido se registró como completamente entregado.",
                EstatusPedidoEnm.RECHAZADO => "El pedido se registró como rechazado en su totalidad.",
                EstatusPedidoEnm.ENTREGADO_PARCIAL => "El pedido se registró como entregado parcialmente y quedó cerrado.",
                _ => "La entrega parcial se registró correctamente. El pedido continúa en proceso."
            };

            if (dto.MontoCobro.HasValue && dto.MontoCobro.Value > 0)
            {
                mensaje += $" Se registró un cobro de ${dto.MontoCobro.Value:N2}.";
            }

            if (custodiaResidual > 0)
            {
                mensaje +=
                    $" Quedaron {custodiaResidual:F2} unidades sin reportar entre la custodia del repartidor.";
            }

            #endregion

            return OperationResult<bool>.Success(true, mensaje);
        }
    }
}