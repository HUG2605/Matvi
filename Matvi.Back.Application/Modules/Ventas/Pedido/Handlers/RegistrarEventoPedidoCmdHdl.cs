using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Commands;
using Matvi.Back.Application.Modules.Ventas.Pedido.Common;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    /// <summary>
    /// Handler para registrar eventos operativos individuales sobre una línea de <c>OpPedidoDetalleEvento</c>.
    ///
    /// Por cada evento:
    /// 1. Inserta una fila en OpPedidoDetalleEvento (fuente de verdad).
    /// 2. Actualiza los cachés de la línea (CantidadSurtida, CantidadCancelada,
    ///    CantidadEntregada, CantidadRechazada).
    /// 3. Recalcula el IdCtEstatusPedidoDetalle mediante <see cref="LineaPedidoCalc"/>.
    ///
    /// Este handler NO toca el estatus del header del pedido; eso es responsabilidad
    /// de AjustarEstatusPedidoCmdHdl, que normalmente se invoca a continuación desde
    /// el orquestador (EntregarPedido, CancelarPedido, etc.) cuando la última línea
    /// se cierra.
    /// </summary>
    [Transaction]
    class RegistrarEventoPedidoCmdHdl(
        IUnitOfWork _unitOfWork,
        AjustarEstatusPedidoCmdHdl _ajustarEstatusHdl
    ) : IRequestHandler<RegistrarEventoPedidoCmd, OperationResult<long>>
    {
        public async Task<OperationResult<long>> Handle(
            RegistrarEventoPedidoCmd request,
            CancellationToken cancellationToken)
        {
            var dto = request.evento;
            var fechaAhora = DateTime.Now;

            #region 1. CARGAR LÍNEA

            var detalle = await _unitOfWork
                .Query<OpPedidoDetalleEtt>()
                .FirstOrDefaultAsync(d => d.Id == dto.IdOpPedidoDetalle, cancellationToken);

            if (detalle == null)
            {
                return OperationResult<long>.Error(
                    $"No se encontró la línea de pedido con Id {dto.IdOpPedidoDetalle}.");
            }

            if (detalle.IdOpPedido != dto.Id)
            {
                return OperationResult<long>.Error(
                    "La línea no pertenece al pedido indicado.");
            }

            // Línea cancelada: solo se permite registrar un evento CANCELADO adicional
            // si no había sido cancelada antes (idempotencia vía caché).
            if (detalle.IdCtEstatusPedidoDetalle == (byte)EstatusPedidoDetalleEnm.CANCELADO
                && dto.TipoEvento != TipoEventoPedidoDetalleEnm.CANCELADO)
            {
                return OperationResult<long>.Error(
                    "La línea está cancelada y no admite más eventos.");
            }

            #endregion

            #region 2. VALIDAR CANTIDAD SEGÚN TIPO DE EVENTO

            var porResolver = LineaPedidoCalc.PorResolver(detalle);

            switch (dto.TipoEvento)
            {
                case TipoEventoPedidoDetalleEnm.SURTIDO:
                    var maxSurtible = detalle.Cantidad - detalle.CantidadSurtida - detalle.CantidadCancelada;
                    if (dto.Cantidad > maxSurtible)
                    {
                        return OperationResult<long>.Error(
                            $"La cantidad a surtir {dto.Cantidad} supera el máximo surtible {maxSurtible} de la línea {detalle.Id}.");
                    }
                    break;

                case TipoEventoPedidoDetalleEnm.ENTREGADO:
                case TipoEventoPedidoDetalleEnm.RECHAZADO:
                    var custodiaDisponible = detalle.CantidadSurtida - detalle.CantidadEntregada - detalle.CantidadRechazada;
                    var maxPermitido = Math.Min(custodiaDisponible, porResolver);
                    if (dto.Cantidad > maxPermitido)
                    {
                        return OperationResult<long>.Error(
                            $"La cantidad {dto.Cantidad} supera la custodia disponible ({custodiaDisponible}) " +
                            $"o por resolver ({porResolver}) de la línea {detalle.Id}.");
                    }
                    break;

                case TipoEventoPedidoDetalleEnm.CANCELADO:
                    var maxCancelable = detalle.Cantidad - detalle.CantidadSurtida - detalle.CantidadCancelada;
                    if (dto.Cantidad > maxCancelable)
                    {
                        return OperationResult<long>.Error(
                            $"No se puede cancelar {dto.Cantidad} en la línea {detalle.Id}: solo hay {maxCancelable} cancelable.");
                    }
                    break;

                case TipoEventoPedidoDetalleEnm.RESERVADO:
                    if (dto.Cantidad > detalle.Cantidad)
                    {
                        return OperationResult<long>.Error(
                            $"La cantidad reservada {dto.Cantidad} supera la cantidad de la línea {detalle.Cantidad}.");
                    }
                    break;

                default:
                    return OperationResult<long>.Error(
                        $"Tipo de evento no soportado: {dto.TipoEvento}.");
            }

            #endregion

            #region 3. INSERTAR EVENTO (fuente de verdad)

            var nvoEvento = new OpPedidoDetalleEventoEtt
            {
                IdOpPedidoDetalle = detalle.Id,
                IdCtTipoEventoPedidoDetalle = (byte)dto.TipoEvento,
                Cantidad = dto.Cantidad,
                Fecha = fechaAhora,
                IdCtUsuario = dto.CtUsua_Id,
                Notas = dto.Notas
            };

            _unitOfWork.Add(nvoEvento);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region 4. ACTUALIZAR CACHÉS DE LA LÍNEA

            switch (dto.TipoEvento)
            {
                case TipoEventoPedidoDetalleEnm.SURTIDO:
                    detalle.CantidadSurtida += dto.Cantidad;
                    break;

                case TipoEventoPedidoDetalleEnm.CANCELADO:
                    detalle.CantidadCancelada += dto.Cantidad;
                    break;

                case TipoEventoPedidoDetalleEnm.ENTREGADO:
                    detalle.CantidadEntregada += dto.Cantidad;
                    break;

                case TipoEventoPedidoDetalleEnm.RECHAZADO:
                    detalle.CantidadRechazada += dto.Cantidad;
                    break;

                // RESERVADO no afecta cachés (es un marcador de estado logístico).
                case TipoEventoPedidoDetalleEnm.RESERVADO:
                    break;
            }

            // Recalcular estatus de línea mediante helper centralizado.
            detalle.IdCtEstatusPedidoDetalle = (byte)LineaPedidoCalc.CalcularEstatusLinea(detalle);

            _unitOfWork.Update(detalle);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region 5. RECALCULAR ESTATUS DEL PEDIDO (Figura 3)

            var pedidoEtt = await _unitOfWork.Query<OpPedidoEtt>()
                .Include(p => p.Coll_OpPedidoDetalle_IdOpPedido)
                .FirstOrDefaultAsync(p => p.Id == detalle.IdOpPedido, cancellationToken);

            if (pedidoEtt != null)
            {
                var estatusFinal = LineaPedidoCalc.DeterminarEstatusFinalHeader(pedidoEtt.Coll_OpPedidoDetalle_IdOpPedido);
                if (estatusFinal.HasValue)
                {
                    if (pedidoEtt.IdCtEstatusPedido != (byte)estatusFinal.Value)
                    {
                        await _ajustarEstatusHdl.Handle(new AjustarEstatusPedidoCmd(new AjustarEstatusPedidoDto
                        {
                            Id = pedidoEtt.Id,
                            NuevoEstatus = estatusFinal.Value,
                            Notas = $"Cierre de pedido por evento {dto.TipoEvento} en línea #{detalle.Id}.",
                            CtUsua_Id = dto.CtUsua_Id
                        }), cancellationToken);
                    }
                }
                else if (dto.TipoEvento == TipoEventoPedidoDetalleEnm.SURTIDO && pedidoEtt.IdCtEstatusPedido == (byte)EstatusPedidoEnm.CREADO)
                {
                    await _ajustarEstatusHdl.Handle(new AjustarEstatusPedidoCmd(new AjustarEstatusPedidoDto
                    {
                        Id = pedidoEtt.Id,
                        NuevoEstatus = EstatusPedidoEnm.EN_PROCESO,
                        Notas = $"Primer surtido registrado en línea #{detalle.Id}.",
                        CtUsua_Id = dto.CtUsua_Id
                    }), cancellationToken);
                }
            }

            #endregion

            return OperationResult<long>.Success(
                nvoEvento.Id,
                $"Evento {dto.TipoEvento} registrado correctamente sobre la línea {detalle.Id}.");
        }
    }
}