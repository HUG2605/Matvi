using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Modules.Ventas.Cliente.Dtos;
using Matvi.Back.Application.Modules.Ventas.Cliente.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Handlers
{
        /// <summary>
    /// Manejador de consulta encargado de resolver los datos para obtener estado credito cliente.
    /// </summary>
public class ObtenerEstadoCreditoClienteQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ObtenerEstadoCreditoClienteQry, EstadoCreditoClienteDto?>
    {
                /// <inheritdoc/>
public async Task<EstadoCreditoClienteDto?> Handle(
            ObtenerEstadoCreditoClienteQry request,
            CancellationToken cancellationToken)
        {
            if (request.IdCliente <= 0) return null;

            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var cliente = await db.Set<CtClienteEtt>()
                .Where(c => c.Id == request.IdCliente)
                .Select(c => new
                {
                    c.Id,
                    c.Nombre,
                    c.LimiteCreditoSemanal
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (cliente == null) return null;

            // 1. Deuda global actual del cliente en todas las sucursales (pedidos no cancelados con saldo)
            var deudaActual = await db.Set<OpPedidoEtt>()
                .Where(p =>
                    p.IdCtCliente == request.IdCliente &&
                    p.IdCtEstatusPedido != (byte)EstatusPedidoEnm.CANCELADO &&
                    p.Total > p.TotalPagado)
                .SumAsync(p => (decimal?)(p.Total - p.TotalPagado), cancellationToken) ?? 0m;

            // 2. Bloqueo por adeudos de semanas anteriores (lunes 00:00:00)
            var hoy = DateTime.Now.Date;
            int diasDesdeLunes = ((int)hoy.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            var inicioSemanaActual = hoy.AddDays(-diasDesdeLunes);

            var tieneAdeudoSemanaAnterior = await db.Set<OpPedidoEtt>()
                .Where(p =>
                    p.IdCtCliente == request.IdCliente &&
                    p.IdCtEstatusPedido != (byte)EstatusPedidoEnm.CANCELADO &&
                    p.Total > p.TotalPagado &&
                    p.Coll_OpPedidoEstatus_IdOpPedido
                        .Where(e => e.IdCtEstatusPedido == (byte)EstatusPedidoEnm.CREADO)
                        .Select(e => e.Fecha)
                        .FirstOrDefault() < inicioSemanaActual)
                .AnyAsync(cancellationToken);

            // 3. Crédito disponible
            var disponible = cliente.LimiteCreditoSemanal - deudaActual;
            if (disponible < 0) disponible = 0;

            // 4. Determinar aptitud
            bool apto = true;
            string motivo = "Apto para crédito";

            if (cliente.LimiteCreditoSemanal <= 0)
            {
                apto = false;
                motivo = "Sin límite de crédito asignado ($0.00)";
            }
            else if (tieneAdeudoSemanaAnterior)
            {
                apto = false;
                motivo = "Bloqueado por adeudos de semanas anteriores sin liquidar";
            }
            else if (disponible <= 0)
            {
                apto = false;
                motivo = "Límite de crédito semanal agotado";
            }

            return new EstadoCreditoClienteDto
            {
                IdCliente = cliente.Id,
                NombreCliente = cliente.Nombre,
                LimiteCreditoSemanal = cliente.LimiteCreditoSemanal,
                DeudaActual = deudaActual,
                CreditoDisponible = disponible,
                TieneAdeudoSemanaAnterior = tieneAdeudoSemanaAnterior,
                AptoParaCredito = apto,
                MotivoEstado = motivo
            };
        }
    }
}
