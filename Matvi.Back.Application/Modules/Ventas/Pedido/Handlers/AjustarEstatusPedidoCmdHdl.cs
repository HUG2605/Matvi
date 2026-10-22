using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Commands;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    /// <summary>
    /// Cambia el estatus de header de un pedido y registra la transición en
    /// OpPedidoEstatus para auditoría.
    ///
    /// Valida que la transición sea permitida:
    /// - CREADO       → APARTADO | EN_PROCESO | CANCELADO
    /// - APARTADO     → EN_PROCESO | CANCELADO
    /// - EN_PROCESO   → ENTREGADO | ENTREGADO_PARCIAL | RECHAZADO | CANCELADO
    /// - ENTREGADO, ENTREGADO_PARCIAL, RECHAZADO, CANCELADO → terminales (sin transición).
    /// </summary>
    [Transaction]
    public class AjustarEstatusPedidoCmdHdl(
        IUnitOfWork _unitOfWork
    ) : IRequestHandler<AjustarEstatusPedidoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            AjustarEstatusPedidoCmd request,
            CancellationToken cancellationToken)
        {
            var dto = request.ajuste;
            var fechaAhora = DateTime.Now;

            var pedido = await _unitOfWork
                .Query<OpPedidoEtt>()
                .FirstOrDefaultAsync(p => p.Id == dto.Id, cancellationToken);

            if (pedido == null)
            {
                return OperationResult<bool>.Error(
                    $"No se encontró el pedido con Id {dto.Id}.");
            }

            var estatusActual = (EstatusPedidoEnm)pedido.IdCtEstatusPedido;

            if (!EsTransicionValida(estatusActual, dto.NuevoEstatus))
            {
                return OperationResult<bool>.Info(
                    false,
                    $"Transición no permitida: {estatusActual} → {dto.NuevoEstatus}.");
            }

            if (estatusActual == dto.NuevoEstatus)
            {
                // No-op: el pedido ya estaba en el estatus deseado. Evitamos
                // duplicar filas en OpPedidoEstatus.
                return OperationResult<bool>.Success(
                    true,
                    $"El pedido ya estaba en estatus {dto.NuevoEstatus}.");
            }

            // Actualizar header.
            pedido.IdCtEstatusPedido = (byte)dto.NuevoEstatus;
            _unitOfWork.Update(pedido);

            // Insertar auditoría.
            var nuevaAuditoria = new OpPedidoEstatusEtt
            {
                IdOpPedido = pedido.Id,
                IdCtEstatusPedido = (byte)dto.NuevoEstatus,
                Fecha = fechaAhora,
                IdCtUsuario = dto.CtUsua_Id,
                Notas = dto.Notas ?? string.Empty
            };

            _unitOfWork.Add(nuevaAuditoria);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(
                true,
                $"Pedido {pedido.Id} pasó de {estatusActual} a {dto.NuevoEstatus}.");
        }

        /// <summary>
        /// Máquina de estados del header. Se conserva como mapa explícito para
        /// que sea trivial de leer y modificar cuando se agreguen más estatus.
        /// </summary>
        private static bool EsTransicionValida(
            EstatusPedidoEnm actual,
            EstatusPedidoEnm nuevo)
        {
            if (actual == nuevo) return true;

            switch (actual)
            {
                case EstatusPedidoEnm.CREADO:
                    return nuevo is EstatusPedidoEnm.APARTADO
                        or EstatusPedidoEnm.EN_PROCESO
                        or EstatusPedidoEnm.ENTREGADO
                        or EstatusPedidoEnm.CANCELADO;

                case EstatusPedidoEnm.APARTADO:
                    return nuevo is EstatusPedidoEnm.EN_PROCESO
                        or EstatusPedidoEnm.CANCELADO;

                case EstatusPedidoEnm.EN_PROCESO:
                    return nuevo is EstatusPedidoEnm.ENTREGADO
                        or EstatusPedidoEnm.ENTREGADO_PARCIAL
                        or EstatusPedidoEnm.RECHAZADO
                        or EstatusPedidoEnm.CANCELADO;

                // Estatus terminales.
                case EstatusPedidoEnm.ENTREGADO:
                case EstatusPedidoEnm.ENTREGADO_PARCIAL:
                case EstatusPedidoEnm.RECHAZADO:
                case EstatusPedidoEnm.CANCELADO:
                default:
                    return false;
            }
        }
    }
}