using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    /// <summary>
    /// Implementa las reglas de validación previas a la captura del pedido.
    /// Ver la doc de <see cref="ValidarCapturaPedidoQry"/> para el detalle de cada regla.
    /// </summary>
    class ValidarCapturaPedidoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
    ) : IRequestHandler<ValidarCapturaPedidoQry, OperationResult<bool>>
    {
        // IDs de catálogo asumidos por seed del EST_Matvi.sql. Se centralizan aquí
        // para no acoplarse a literales mágicos en el resto del flujo.
        private const byte CANAL_PRESENCIAL = 1;
        private const int ID_CLIENTE_MOSTRADOR = 1;

                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            ValidarCapturaPedidoQry request,
            CancellationToken cancellationToken)
        {
            var errores = new List<string>();

            #region Reglas del Cliente

            // Regla Cliente-1: si es Mostrador, solo canal PRESENCIAL.
            if (request.EsMostrador && request.IdCanalCaptura != CANAL_PRESENCIAL)
            {
                errores.Add(
                    "Los pedidos de mostrador solo se capturan de forma presencial.");
            }

            // Regla Cliente-2 / Cliente-10 / Crédito Semanal: validaciones de crédito para clientes registrados.
            if (!request.EsMostrador)
            {
                using var db = await _factory.CreateDbContextAsync(cancellationToken);

                var clienteEtt = await db.Set<CtClienteEtt>()
                    .FirstOrDefaultAsync(c => c.Id == request.IdCliente, cancellationToken);

                // Si se solicita crédito (Pago < Total)
                if (request.PagoFld < request.Total)
                {
                    var nuevoADeber = request.Total - request.PagoFld;

                    // 1. Deuda global actual del cliente en TODAS las sucursales
                    var deudaGlobalActual = await db.Set<OpPedidoEtt>()
                        .Where(p =>
                            p.IdCtCliente == request.IdCliente &&
                            p.IdCtEstatusPedido != (byte)EstatusPedidoEnm.CANCELADO &&
                            p.Total > p.TotalPagado)
                        .SumAsync(p => (decimal?)(p.Total - p.TotalPagado), cancellationToken) ?? 0m;

                    // Validar límite de crédito semanal global
                    if (clienteEtt != null && clienteEtt.LimiteCreditoSemanal > 0)
                    {
                        if (deudaGlobalActual + nuevoADeber > clienteEtt.LimiteCreditoSemanal)
                        {
                            errores.Add(
                                $"El crédito solicitado excede el límite de crédito semanal global del cliente (${clienteEtt.LimiteCreditoSemanal:N2}). " +
                                $"Deuda global actual: ${deudaGlobalActual:N2}, nuevo adeudo: ${nuevoADeber:N2}.");
                        }
                    }

                    // 2. Bloqueo por adeudos no cubiertos antes del domingo (lunes inicio de semana)
                    var hoy = DateTime.Now.Date;
                    int diasDesdeLunes = ((int)hoy.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
                    var inicioSemanaActual = hoy.AddDays(-diasDesdeLunes); // Lunes 00:00:00

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

                    if (tieneAdeudoSemanaAnterior)
                    {
                        errores.Add(
                            "El cliente tiene adeudos pendientes de semanas anteriores que no fueron cubiertos antes del domingo. " +
                            "No se le puede otorgar nuevo crédito hasta que liquide sus saldos atrasados.");
                    }
                }
            }

            // Regla Cliente-3 / Cliente-9: si es Mostrador, PagoFld debe ser exactamente el Total.
            if (request.EsMostrador && request.PagoFld < request.Total)
            {
                errores.Add(
                    "Los pedidos de mostrador deben cubrir el importe total al momento de la captura.");
            }

            // Regla Cliente-4 (Apartado): si EsApartado, el cliente NO puede ser Mostrador.
            if (request.EsApartado && request.EsMostrador)
            {
                errores.Add(
                    "La modalidad de apartado es exclusiva para clientes registrados.");
            }

            // Regla Cliente-4: si EsApartado, PagoFld debe ser exactamente el Total.
            if (request.EsApartado && request.PagoFld != request.Total)
            {
                errores.Add(
                    "En la modalidad de apartado el pago debe cubrir el 100% del pedido.");
            }

            #endregion

            #region Reglas del Vendedor (por producto)

            foreach (var p in request.Productos)
            {
                // Regla Vendedor-3: EntregaMostrador solo si ctProd.EntregaMostrador == true.
                if (p.ProductoMarcadoEntregadoMostrador && !p.ProductoEntregaMostrador)
                {
                    errores.Add(
                        $"El producto {p.IdProducto} no permite entrega en mostrador.");
                }

                // Regla Vendedor-4: BajoPedido sin stock no puede marcarse EntregadoMostrador.
                if (p.ProductoMarcadoEntregadoMostrador
                    && p.ProductoBajoPedido
                    && p.ExistenciaDisponibleUBI <= 0)
                {
                    errores.Add(
                        $"El producto {p.IdProducto} está sin existencia y no puede " +
                        $"entregarse en mostrador (es de pedido).");
                }

                // Regla Vendedor-5: si EsApartado, no se permite EntregaMostrador.
                if (request.EsApartado && p.ProductoMarcadoEntregadoMostrador)
                {
                    errores.Add(
                        $"En pedidos apartados no se marca entrega en mostrador. " +
                        $"El surtido ocurre cuando el cliente retira. Producto {p.IdProducto}.");
                }
            }

            #endregion

            if (errores.Count > 0)
            {
                return OperationResult<bool>.Error(
                    "No es posible crear el pedido:\n" + string.Join("\n", errores));
            }

            return OperationResult<bool>.Success(true, "Validación correcta.");
        }
    }
}