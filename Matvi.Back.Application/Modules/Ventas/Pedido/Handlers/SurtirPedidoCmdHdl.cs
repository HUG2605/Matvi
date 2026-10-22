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
    /// Handler para surtir un pedido.
    /// Soporta pedidos en estatus CREADO, APARTADO o EN_PROCESO.
    /// Por cada línea: inserta evento SURTIDO con IdCtUsuarioRecibe = repartidor (o null si DIRECTO),
    /// genera movimientos SAL_VENTA FIFO y actualiza cachés de línea.
    /// Modalidades de surtido:
    ///   - REPARTIDOR → header EN_PROCESO; la entrega posterior la registra EntregarPedidoCmdHdl.
    ///   - DIRECTO    → inserta también evento ENTREGADO en el acto; header puede quedar ENTREGADO.
    /// </summary>
    [Transaction]
    public class SurtirPedidoCmdHdl(
        IUnitOfWork _unitOfWork,
        IConsecutivoService _consecutivoService,
        IWebPushService _webPushService,
        AjustarEstatusPedidoCmdHdl _ajustarEstatusHdl
    ) : IRequestHandler<SurtirPedidoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            SurtirPedidoCmd request,
            CancellationToken cancellationToken)
        {
            var dto = request.pedido;

            #region 1. VALIDACIONES PREVIAS

            // 1a. Coherencia del modelo de surtido
            if (dto.TipoSurtido == TipoSurtidoEnm.REPARTIDOR)
            {
                if (dto.IdCtUsuarioRepartidor is null or <= 0)
                {
                    return OperationResult<bool>.Info(
                        false,
                        "Debe seleccionar un repartidor para esta modalidad de surtido.");
                }
            }
            else if (dto.TipoSurtido == TipoSurtidoEnm.DIRECTO)
            {
                if (dto.IdCtUsuarioRepartidor.HasValue)
                {
                    return OperationResult<bool>.Info(
                        false,
                        "En la modalidad 'Entrega directa' no debe indicar un repartidor.");
                }
            }
            else
            {
                return OperationResult<bool>.Info(
                    false,
                    "Modalidad de surtido no reconocida.");
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

            // 1c. Que el pedido esté en un estatus válido para surtir (CREADO, APARTADO o EN_PROCESO)
            if (pedidoEtt.IdCtEstatusPedido != (byte)EstatusPedidoEnm.CREADO &&
                pedidoEtt.IdCtEstatusPedido != (byte)EstatusPedidoEnm.APARTADO &&
                pedidoEtt.IdCtEstatusPedido != (byte)EstatusPedidoEnm.EN_PROCESO)
            {
                return OperationResult<bool>.Info(
                    false,
                    $"El pedido no se puede surtir en su estatus actual. " +
                    $"Estatus esperados: CREADO, APARTADO o EN_PROCESO; " +
                    $"estatus actual: {pedidoEtt.IdCtEstatusPedido}.");
            }

            #endregion

            #region 2. RESOLVER LÍNEAS Y CANTIDADES A SURTIR

            var lineasDictDto = (dto.Lineas ?? [])
                .ToDictionary(l => l.IdOpPedidoDetalle, l => l.CantidadASurtir);

            bool surtirTodoRemanente = !lineasDictDto.Any(l => l.Value > 0);

            var lineasASurtir = new List<(OpPedidoDetalleEtt Detalle, decimal CantidadASurtir)>();

            foreach (var detalle in pedidoEtt.Coll_OpPedidoDetalle_IdOpPedido)
            {
                var pendientePorSurtir = detalle.Cantidad - detalle.CantidadSurtida - detalle.CantidadCancelada;
                if (pendientePorSurtir <= 0) continue;

                decimal cantASurtir = surtirTodoRemanente
                    ? pendientePorSurtir
                    : lineasDictDto.GetValueOrDefault(detalle.Id, 0m);

                if (cantASurtir <= 0) continue;

                if (cantASurtir > pendientePorSurtir)
                {
                    return OperationResult<bool>.Info(
                        false,
                        $"La cantidad a surtir ({cantASurtir:F2}) en '{detalle.Cpp_Nombre}' supera el pendiente por surtir ({pendientePorSurtir:F2}).");
                }

                lineasASurtir.Add((detalle, cantASurtir));
            }

            if (!lineasASurtir.Any())
            {
                return OperationResult<bool>.Info(
                    false,
                    "No hay cantidades pendientes por surtir en este pedido o las cantidades capturadas son 0.");
            }

            #endregion

            #region 3. VALIDACIÓN DE STOCK PARA PRODUCTOS REGULARIZADOS

            var idsProductos = lineasASurtir
                .Select(l => l.Detalle.Cpp_IdCtProducto)
                .Distinct()
                .ToList();

            var relProductosSucursal = await _unitOfWork.Query<RlProductoSucursalEtt>()
                .Where(r => idsProductos.Contains(r.IdCtProducto)
                         && r.IdCtSucursal == pedidoEtt.IdCtSucursal)
                .ToDictionaryAsync(r => r.IdCtProducto, cancellationToken);

            var productosDict = await _unitOfWork.Query<CtProductoEtt>()
                .Include(p => p.Nav_IdCtUnidadBaseInventario)
                .Where(p => idsProductos.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, cancellationToken);

            var lotesDisponibles = await _unitOfWork.Query<OpLoteEtt>()
                .Where(l => l.IdCtSucursal == pedidoEtt.IdCtSucursal
                         && idsProductos.Contains(l.Cpp_IdCtProducto))
                .ToListAsync(cancellationToken);

            var erroresStock = new List<string>();

            foreach (var (detalle, cantASurtir) in lineasASurtir)
            {
                var rel = relProductosSucursal.GetValueOrDefault(detalle.Cpp_IdCtProducto);
                var ctProd = productosDict.GetValueOrDefault(detalle.Cpp_IdCtProducto);

                if (ctProd != null && rel != null && rel.Regularizado && !ctProd.BajoPedido)
                {
                    var cantASurtirUBI = cantASurtir * detalle.Cpp_FactorUBI;

                    var stockLotesUBI = lotesDisponibles
                        .Where(l => l.Cpp_IdCtProducto == detalle.Cpp_IdCtProducto && l.CantidadDisponibleUBI > 0)
                        .Sum(l => l.CantidadDisponibleUBI);

                    if (stockLotesUBI < cantASurtirUBI)
                    {
                        erroresStock.Add(
                            $"Stock insuficiente en lotes para '{detalle.Cpp_Nombre}': " +
                            $"disponible {stockLotesUBI:F2}, requerido {cantASurtirUBI:F2}.");
                    }
                }
            }

            if (erroresStock.Any())
            {
                return OperationResult<bool>.Info(
                    false,
                    "No es posible surtir el pedido:\n" + string.Join("\n", erroresStock));
            }

            #endregion

            #region 4. RESOLVER DATOS DE REPARTIDOR / USUARIO

            string nombreRepartidor = string.Empty;

            if (dto.TipoSurtido == TipoSurtidoEnm.REPARTIDOR)
            {
                var repartidorEtt = await _unitOfWork
                    .FindAsync<CtUsuarioEtt, short>(
                        dto.IdCtUsuarioRepartidor!.Value,
                        cancellationToken);

                if (repartidorEtt != null)
                {
                    nombreRepartidor =
                        $"{repartidorEtt.Nombre} {repartidorEtt.PrimerApellido} {repartidorEtt.SegundoApellido}"
                            .Trim();
                }
            }

            var fechaAhora = DateTime.Now;
            short usuarioOperacion = dto.CtUsua_Id > 0 ? dto.CtUsua_Id : (short)0;

            #endregion

            #region 5. REGISTRAR EVENTOS, MOVIMIENTOS DE INVENTARIO Y ACTUALIZAR CACHÉS

            foreach (var (detalle, cantASurtir) in lineasASurtir)
            {
                var rel = relProductosSucursal.GetValueOrDefault(detalle.Cpp_IdCtProducto);
                var ctProd = productosDict.GetValueOrDefault(detalle.Cpp_IdCtProducto);
                bool regularizado = rel?.Regularizado ?? false;
                var cantASurtirUBI = cantASurtir * detalle.Cpp_FactorUBI;

                // 5a. Insertar evento SURTIDO
                var nvoEventoSurtido = new OpPedidoDetalleEventoEtt
                {
                    IdOpPedidoDetalle = detalle.Id,
                    IdCtTipoEventoPedidoDetalle = (byte)TipoEventoPedidoDetalleEnm.SURTIDO,
                    Cantidad = cantASurtir,
                    Fecha = fechaAhora,
                    IdCtUsuario = usuarioOperacion,
                    IdCtUsuarioRecibe = dto.TipoSurtido == TipoSurtidoEnm.REPARTIDOR ? dto.IdCtUsuarioRepartidor : null,
                    Notas = dto.TipoSurtido == TipoSurtidoEnm.REPARTIDOR
                        ? $"Surtido para repartidor: {nombreRepartidor}"
                        : "Surtido desde almacén para entrega directa"
                };

                _unitOfWork.Add(nvoEventoSurtido);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // 5b. Descontar de lotes y registrar movimientos de inventario SAL_VENTA
                List<OpLoteEtt> lotesParaDescontar;
                if (!regularizado)
                {
                    // Lote inicial (puede quedar negativo)
                    var loteInicial = lotesDisponibles
                        .FirstOrDefault(l => l.Cpp_IdCtProducto == detalle.Cpp_IdCtProducto
                                          && l.IdCtTipoLote == (byte)TipoLoteEnm.INICIAL);

                    if (loteInicial == null)
                    {
                        var numeroLote = await _consecutivoService.GenerarNumeroAsync(
                            pedidoEtt.IdCtSucursal, (byte)TipoConsecutivoEnm.LOTE, cancellationToken);

                        loteInicial = new OpLoteEtt
                        {
                            IdCtSucursal = pedidoEtt.IdCtSucursal,
                            Numero = numeroLote,
                            IdCtTipoLote = (byte)TipoLoteEnm.INICIAL,
                            Cpp_IdCtProducto = detalle.Cpp_IdCtProducto,
                            Cpp_Nombre = ctProd?.Nav_IdCtUnidadBaseInventario?.Nombre ?? "UNIDAD",
                            Cpp_FactorUBI = 1,
                            CantidadInicialUBI = 0,
                            CantidadDisponibleUBI = 0,
                            CostoUP = 0,
                            CostoUBI = 0,
                            Fecha = fechaAhora
                        };

                        _unitOfWork.Add(loteInicial);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                        lotesDisponibles.Add(loteInicial);
                    }

                    lotesParaDescontar = [loteInicial];
                }
                else
                {
                    // Lotes regulares FIFO
                    lotesParaDescontar = lotesDisponibles
                        .Where(l => l.Cpp_IdCtProducto == detalle.Cpp_IdCtProducto && l.CantidadDisponibleUBI > 0)
                        .OrderBy(l => l.Fecha)
                        .ThenBy(l => l.Id)
                        .ToList();

                    if (lotesParaDescontar.Count == 0)
                    {
                        var loteFallback = lotesDisponibles
                            .FirstOrDefault(l => l.Cpp_IdCtProducto == detalle.Cpp_IdCtProducto);

                        if (loteFallback != null)
                        {
                            lotesParaDescontar = [loteFallback];
                        }
                    }
                }

                var cantidadPendienteUBI = cantASurtirUBI;

                if (!regularizado)
                {
                    var loteInicial = lotesParaDescontar.First();
                    loteInicial.CantidadDisponibleUBI -= cantidadPendienteUBI;
                    _unitOfWork.Update(loteInicial);

                    var numeroMovimiento = await _consecutivoService.GenerarNumeroAsync(
                        pedidoEtt.IdCtSucursal,
                        (byte)TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO,
                        cancellationToken);

                    var movimiento = new OpMovimientoInventarioEtt
                    {
                        IdCtSucursal = pedidoEtt.IdCtSucursal,
                        Numero = numeroMovimiento,
                        Cpp_IdCtProducto = detalle.Cpp_IdCtProducto,
                        Cpp_Nombre = detalle.Cpp_Nombre,
                        Cpp_FactorUBI = detalle.Cpp_FactorUBI,
                        CantidadUBI = cantidadPendienteUBI,
                        Fecha = fechaAhora,
                        IdCtTipoMovimiento = (byte)TipoMovimientoEnm.SAL_VENTA,
                        IdOpPedidoDetalleEvento = nvoEventoSurtido.Id,
                        IdOpLote = loteInicial.Id,
                        CostoUBI = loteInicial.CostoUBI,
                        CostoTotal = cantidadPendienteUBI * loteInicial.CostoUBI,
                        IdCtUsuario = usuarioOperacion
                    };

                    _unitOfWork.Add(movimiento);
                    cantidadPendienteUBI = 0;
                }
                else
                {
                    foreach (var lote in lotesParaDescontar)
                    {
                        if (cantidadPendienteUBI <= 0) break;
                        if (lote.CantidadDisponibleUBI <= 0) continue;

                        var cantidadTomarUBI = Math.Min(cantidadPendienteUBI, lote.CantidadDisponibleUBI);

                        lote.CantidadDisponibleUBI -= cantidadTomarUBI;
                        _unitOfWork.Update(lote);

                        var numeroMovimiento = await _consecutivoService.GenerarNumeroAsync(
                            pedidoEtt.IdCtSucursal,
                            (byte)TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO,
                            cancellationToken);

                        var movimiento = new OpMovimientoInventarioEtt
                        {
                            IdCtSucursal = pedidoEtt.IdCtSucursal,
                            Numero = numeroMovimiento,
                            Cpp_IdCtProducto = detalle.Cpp_IdCtProducto,
                            Cpp_Nombre = detalle.Cpp_Nombre,
                            Cpp_FactorUBI = detalle.Cpp_FactorUBI,
                            CantidadUBI = cantidadTomarUBI,
                            Fecha = fechaAhora,
                            IdCtTipoMovimiento = (byte)TipoMovimientoEnm.SAL_VENTA,
                            IdOpPedidoDetalleEvento = nvoEventoSurtido.Id,
                            IdOpLote = lote.Id,
                            CostoUBI = lote.CostoUBI,
                            CostoTotal = cantidadTomarUBI * lote.CostoUBI,
                            IdCtUsuario = usuarioOperacion
                        };

                        _unitOfWork.Add(movimiento);
                        cantidadPendienteUBI -= cantidadTomarUBI;
                    }

                    if (cantidadPendienteUBI > 0)
                    {
                        var loteRemanente = lotesParaDescontar.LastOrDefault()
                            ?? lotesDisponibles.FirstOrDefault(l => l.Cpp_IdCtProducto == detalle.Cpp_IdCtProducto);

                        if (loteRemanente != null)
                        {
                            loteRemanente.CantidadDisponibleUBI -= cantidadPendienteUBI;
                            _unitOfWork.Update(loteRemanente);

                            var numeroMovimiento = await _consecutivoService.GenerarNumeroAsync(
                                pedidoEtt.IdCtSucursal,
                                (byte)TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO,
                                cancellationToken);

                            var movimiento = new OpMovimientoInventarioEtt
                            {
                                IdCtSucursal = pedidoEtt.IdCtSucursal,
                                Numero = numeroMovimiento,
                                Cpp_IdCtProducto = detalle.Cpp_IdCtProducto,
                                Cpp_Nombre = detalle.Cpp_Nombre,
                                Cpp_FactorUBI = detalle.Cpp_FactorUBI,
                                CantidadUBI = cantidadPendienteUBI,
                                Fecha = fechaAhora,
                                IdCtTipoMovimiento = (byte)TipoMovimientoEnm.SAL_VENTA,
                                IdOpPedidoDetalleEvento = nvoEventoSurtido.Id,
                                IdOpLote = loteRemanente.Id,
                                CostoUBI = loteRemanente.CostoUBI,
                                CostoTotal = cantidadPendienteUBI * loteRemanente.CostoUBI,
                                IdCtUsuario = usuarioOperacion
                            };

                            _unitOfWork.Add(movimiento);
                        }
                    }
                }

                // 5c. Descontar de la reserva en RlProductoSucursal si estaba reservado
                if (rel != null && rel.StockReservadoUBI > 0)
                {
                    rel.StockReservadoUBI = Math.Max(0m, rel.StockReservadoUBI - cantASurtirUBI);
                    _unitOfWork.Update(rel);
                }

                // 5d. En entrega DIRECTO: el surtido y la entrega ocurren en el acto.
                //     Para REPARTIDOR el receptor queda en IdCtUsuarioRecibe del evento SURTIDO;
                //     la entrega formal la registra EntregarPedidoCmdHdl.
                if (dto.TipoSurtido == TipoSurtidoEnm.DIRECTO)
                {
                    var nvoEventoEntrega = new OpPedidoDetalleEventoEtt
                    {
                        IdOpPedidoDetalle = detalle.Id,
                        IdCtTipoEventoPedidoDetalle = (byte)TipoEventoPedidoDetalleEnm.ENTREGADO,
                        Cantidad = cantASurtir,
                        Fecha = fechaAhora.AddMilliseconds(100),
                        IdCtUsuario = usuarioOperacion,
                        Notas = "Entrega directa al cliente desde almacén"
                    };
                    _unitOfWork.Add(nvoEventoEntrega);
                    detalle.CantidadEntregada += cantASurtir;
                }

                // 5e. Actualizar cachés de la línea
                detalle.CantidadSurtida += cantASurtir;
                detalle.IdCtEstatusPedidoDetalle = (byte)LineaPedidoCalc.CalcularEstatusLinea(detalle);

                _unitOfWork.Update(detalle);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region 6. ACTUALIZAR ESTATUS DEL PEDIDO Y AUDITORÍA

            var estatusFinal = LineaPedidoCalc.DeterminarEstatusFinalHeader(pedidoEtt.Coll_OpPedidoDetalle_IdOpPedido);
            var nuevoEstatus = estatusFinal ?? EstatusPedidoEnm.EN_PROCESO;

            if (dto.TipoSurtido == TipoSurtidoEnm.REPARTIDOR)
            {
                pedidoEtt.IdCtUsuarioRepartidor = dto.IdCtUsuarioRepartidor!.Value;

                if (!string.IsNullOrWhiteSpace(nombreRepartidor))
                {
                    var notasActuales = pedidoEtt.Notas ?? string.Empty;
                    var sufijoSurtido = $"Surtido por: {nombreRepartidor}";
                    if (!notasActuales.Contains(sufijoSurtido))
                    {
                        pedidoEtt.Notas = string.IsNullOrWhiteSpace(notasActuales)
                            ? sufijoSurtido
                            : $"{notasActuales} | {sufijoSurtido}";
                    }
                }
            }

            _unitOfWork.Update(pedidoEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Transicionar estatus del pedido vía AjustarEstatusPedidoCmdHdl si difiere del actual
            if (pedidoEtt.IdCtEstatusPedido != (byte)nuevoEstatus)
            {
                var notasAjuste = nuevoEstatus == EstatusPedidoEnm.ENTREGADO
                    ? "Todas las líneas entregadas completas desde almacén"
                    : (dto.TipoSurtido == TipoSurtidoEnm.REPARTIDOR
                        ? $"Pedido en proceso. Asignado a repartidor: {nombreRepartidor}"
                        : "Surtido parcial en almacén");

                var resAjuste = await _ajustarEstatusHdl.Handle(
                    new AjustarEstatusPedidoCmd(
                        new AjustarEstatusPedidoDto
                        {
                            Id = pedidoEtt.Id,
                            NuevoEstatus = nuevoEstatus,
                            Notas = notasAjuste,
                            CtUsua_Id = usuarioOperacion
                        }),
                    cancellationToken);

                if (!resAjuste.IsSuccessful)
                {
                    return OperationResult<bool>.Info(false, resAjuste.Message ?? "No fue posible ajustar el estatus del pedido.");
                }
            }

            #endregion

            #region 7. NOTIFICACIÓN PUSH AL REPARTIDOR

            if (dto.TipoSurtido == TipoSurtidoEnm.REPARTIDOR
                && dto.IdCtUsuarioRepartidor.HasValue)
            {
                var suscripcionesRepartidor = _unitOfWork
                    .Query<CtUsuarioWebPushSuscripcionEtt>()
                    .Where(s =>
                        s.Activa &&
                        s.IdCtUsuario == dto.IdCtUsuarioRepartidor.Value)
                    .ToList();

                if (suscripcionesRepartidor.Any())
                {
                    await _webPushService.EnviarAsync(
                        suscripcionesRepartidor,
                        titulo: "Pedido asignado",
                        cuerpo: $"Tienes un pedido asignado para entregar a domicilio.",
                        datos: new
                        {
                            tipo = "PEDIDO_ASIGNADO_REPARTIDOR",
                            idPedido = pedidoEtt.Id
                        },
                        cancellationToken);

                    var expiradas = suscripcionesRepartidor.Where(s => !s.Activa).ToList();
                    if (expiradas.Any())
                    {
                        expiradas.ForEach(s => _unitOfWork.Update(s));
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }
            }

            #endregion

            return OperationResult<bool>.Success(
                true,
                dto.TipoSurtido == TipoSurtidoEnm.DIRECTO
                    ? (estatusFinal.HasValue
                        ? "El pedido se surtió y entregó completamente."
                        : "El surtido y entrega parcial se registraron correctamente.")
                    : "El pedido se surtió correctamente al repartidor.");
        }
    }
}

