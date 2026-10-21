using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    class DetallarPedidoQryHdl(
         IDbContextFactory<MatviQryDbCtx> _factory
     ) : IRequestHandler<DetallarPedidoQry, DetallarPedidoDto>
    {
        public async Task<DetallarPedidoDto> Handle(
            DetallarPedidoQry request,
            CancellationToken cancellationToken)
        {
            var puedeVerSensibles = request.puedeVerSensibles;
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<OpPedidoEtt>()
                .Where(p => p.Id == request.Id)
                .Select(p => new DetallarPedidoDto
                {
                    // IIdDto
                    Id = p.Id,
                    // IFolioDto
                    CtSucu_Clave = p.Nav_IdCtSucursal.Clave,
                    Numero = p.Numero,
                    // Generales
                    CtSucu_Nombre = p.Nav_IdCtSucursal.Nombre,
                    Fecha = p.Coll_OpPedidoEstatus_IdOpPedido
                        .Where(e => e.IdCtEstatusPedido == (byte)EstatusPedidoEnm.CREADO)
                        .Select(e => e.Fecha)
                        .First(),
                    IdCtCliente = p.IdCtCliente,
                    CtClie_Nombre = p.Nav_IdCtCliente != null ? p.Nav_IdCtCliente.Nombre : null,
                    NombreClienteMostrador = p.NombreClienteMostrador,
                    TelefonoClienteMostrador = p.TelefonoClienteMostrador,
                    CtCanaCapt_Nombre = p.Nav_IdCtCanalCaptura.Nombre,
                    CtEstatus_Nombre = p.Nav_IdCtEstatusPedido.Nombre,
                    IdCtEstatusPedido = p.IdCtEstatusPedido,
                    EntregaDomicilio = p.EntregaDomicilio,
                    DireccionEntrega = p.DireccionEntrega,
                    Notas=p.Notas,
                    Total = puedeVerSensibles ? p.Total : 0,
                    TotalPagado = puedeVerSensibles ? p.TotalPagado : 0,
                    Productos = p.Coll_OpPedidoDetalle_IdOpPedido
                        .Select(d => new DetallarPedidoDetalleDto
                        {
                            Id = d.Id,
                            // IFolioDto
                            CtSucu_Clave = p.Nav_IdCtSucursal.Clave,
                            Numero = p.Numero,
                            // IProductoBaseDto
                            CtProd_Nombre = d.Nav_Cpp_IdCtProducto.Nombre,
                            CtProd_EspecificacionTecnica = d.Nav_Cpp_IdCtProducto.EspecificacionTecnica,
                            CtProd_Marca = d.Nav_Cpp_IdCtProducto.Marca,
                            // IPresentacionProductoBaseDto
                            CtPresProd_IdCtProducto = d.Cpp_IdCtProducto,
                            CtPresProd_Nombre = d.Cpp_Nombre,
                            CtPresProd_FactorUBI = d.Cpp_FactorUBI,
                            // Otros
                            Cantidad = d.Cantidad,
                            CantidadSurtida = d.CantidadSurtida,
                            CantidadEntregada = d.CantidadEntregada,
                            CantidadRechazada = d.CantidadRechazada,
                            CantidadCancelada = d.CantidadCancelada,
                            IdCtEstatusPedidoDetalle = d.IdCtEstatusPedidoDetalle,
                            CtEstatus_Nombre = d.Nav_IdCtEstatusPedidoDetalle.Nombre,
                            Precio = d.Precio,
                            Importe = d.Importe,
                            CtUnidBaseInve_Abreviatura = d.Nav_Cpp_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura
                        }).ToList(),

                    Cobros = p.Coll_OpCobroDetalle_IdOpPedido
                        .Select(cd => new DetallarPedidoCobroDto
                        {
                            Id = cd.Nav_IdOpCobro.Id,
                            CtSucu_Clave = cd.Nav_IdOpCobro.Nav_IdCtSucursal.Clave,
                            Numero = cd.Nav_IdOpCobro.Numero,
                            Fecha = cd.Nav_IdOpCobro.Fecha,
                            MontoTotal = cd.Nav_IdOpCobro.MontoTotal,
                            CtMediPago_Nombre = cd.Nav_IdOpCobro.Nav_IdCtMedioPago.Nombre,
                            CtUsuaCobra_Nombre = cd.Nav_IdOpCobro.Nav_IdCtUsuarioRecibe.Nombre,
                            MontoAplicado = cd.MontoAplicado
                        }).ToList(),
                    Lotes = new List<DetallarPedidoLoteDto>(),
                    HistorialEstatus = p.Coll_OpPedidoEstatus_IdOpPedido
                        .OrderBy(e => e.Fecha)
                        .ThenBy(e => e.Id)
                        .Select(e => new DetallarPedidoEstatusDto
                        {
                            Id = e.Id,
                            IdCtEstatusPedido = e.IdCtEstatusPedido,
                            CtEstatus_Nombre = e.Nav_IdCtEstatusPedido.Nombre,
                            CantidadLineas = p.Coll_OpPedidoDetalle_IdOpPedido.Count(),
                            Fecha = e.Fecha,
                            CtUsuario_Nombre = e.Nav_IdCtUsuario.Nombre,
                            Notas = e.Notas
                        }).ToList()

                });

            var pedido = await query.SingleAsync(cancellationToken);

            // Carga de Lotes asociados al pedido desde movimientos de inventario
            var idDetalles = await db.Set<OpPedidoDetalleEtt>()
                .Where(d => d.IdOpPedido == request.Id)
                .Select(d => d.Id)
                .ToListAsync(cancellationToken);

            var lotesIds = await db.Set<OpMovimientoInventarioEtt>()
                .Where(m => m.Nav_IdOpPedidoDetalleEvento != null && idDetalles.Contains(m.Nav_IdOpPedidoDetalleEvento.IdOpPedidoDetalle))
                .Select(m => m.IdOpLote)
                .Distinct()
                .ToListAsync(cancellationToken);

            pedido.Lotes = await db.Set<OpLoteEtt>()
                .Where(l => lotesIds.Contains(l.Id))
                .Select(l => new DetallarPedidoLoteDto
                {
                    Id = l.Id,
                    CtSucu_Clave = l.Nav_IdCtSucursal.Clave,
                    Numero = l.Numero,
                    CtProd_Nombre = l.Nav_Cpp_IdCtProducto.Nombre,
                    CtProd_EspecificacionTecnica = l.Nav_Cpp_IdCtProducto.EspecificacionTecnica,
                    CtProd_Marca = l.Nav_Cpp_IdCtProducto.Marca,
                    CtPresProd_IdCtProducto = l.Cpp_IdCtProducto,
                    CtPresProd_Nombre = l.Cpp_Nombre,
                    CtPresProd_FactorUBI = l.Cpp_FactorUBI,
                    CantidadInicialUBI = l.CantidadInicialUBI,
                    CantidadDisponibleUBI = l.CantidadDisponibleUBI,
                    CostoUP = l.CostoUP,
                    CostoUBI = l.CostoUBI,
                    Fecha = l.Fecha,
                    CtUnidBaseInve_Abreviatura = l.Nav_Cpp_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura
                })
                .ToListAsync(cancellationToken);

            if (!pedido.HistorialEstatus.Any(h => !h.EsEventoDetalle && h.IdCtEstatusPedido == pedido.IdCtEstatusPedido))
            {
                var ultimoHistorial = pedido.HistorialEstatus.LastOrDefault();
                pedido.HistorialEstatus.Add(new DetallarPedidoEstatusDto
                {
                    Id = 0,
                    EsEventoDetalle = false,
                    IdCtEstatusPedido = pedido.IdCtEstatusPedido,
                    CtEstatus_Nombre = pedido.CtEstatus_Nombre,
                    CantidadLineas = pedido.Productos.Count,
                    Fecha = ultimoHistorial?.Fecha.AddSeconds(1) ?? DateTime.Now,
                    CtUsuario_Nombre = ultimoHistorial?.CtUsuario_Nombre ?? "SISTEMA",
                    Notas = pedido.IdCtEstatusPedido == (byte)EstatusPedidoEnm.ENTREGADO
                        ? "Surtido completo en mostrador."
                        : $"Estatus {pedido.CtEstatus_Nombre}."
                });
            }

            var rawEventosDetalle = await db.Set<OpPedidoDetalleEventoEtt>()
                .Where(ev => ev.Nav_IdOpPedidoDetalle.IdOpPedido == request.Id)
                .Select(ev => new
                {
                    ev.Id,
                    ev.Fecha,
                    ev.IdCtTipoEventoPedidoDetalle,
                    TipoEvento_Nombre = ev.Nav_IdCtTipoEventoPedidoDetalle.Nombre,
                    Usuario_Nombre = ev.Nav_IdCtUsuario.Nombre,
                    ev.Notas,
                    ProductoNombre = ev.Nav_IdOpPedidoDetalle.Nav_Cpp_IdCtProducto.Nombre,
                    PresentacionNombre = ev.Nav_IdOpPedidoDetalle.Cpp_Nombre + " (" + ev.Nav_IdOpPedidoDetalle.Nav_Cpp_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura + ")",
                    ev.Cantidad
                })
                .ToListAsync(cancellationToken);

            if (rawEventosDetalle.Count != 0)
            {
                var agrupados = rawEventosDetalle
                    .GroupBy(ev => new { ev.Fecha, ev.IdCtTipoEventoPedidoDetalle, ev.Usuario_Nombre, ev.Notas })
                    .Select(g => new DetallarPedidoEstatusDto
                    {
                        Id = g.Min(x => x.Id),
                        EsEventoDetalle = true,
                        IdCtTipoEventoPedidoDetalle = g.Key.IdCtTipoEventoPedidoDetalle,
                        CtTipoEvento_Nombre = g.First().TipoEvento_Nombre,
                        CantidadLineas = g.Count(),
                        Fecha = g.Key.Fecha,
                        CtUsuario_Nombre = g.Key.Usuario_Nombre,
                        Notas = g.Key.Notas,
                        Productos = g.Select(p => new DetallarPedidoEstatusItemDto
                        {
                            ProductoNombre = p.ProductoNombre,
                            PresentacionNombre = p.PresentacionNombre,
                            Cantidad = p.Cantidad
                        }).ToList()
                    })
                    .ToList();

                // Asociar cada evento operativo a la fase del pedido general en la que ocurrió
                var pasoEnProceso = pedido.HistorialEstatus
                    .FirstOrDefault(h => h.IdCtEstatusPedido == (byte)EstatusPedidoEnm.EN_PROCESO);

                var pasoCancelado = pedido.HistorialEstatus
                    .FirstOrDefault(h => h.IdCtEstatusPedido == (byte)EstatusPedidoEnm.CANCELADO);

                var pasoFinal = pedido.HistorialEstatus.LastOrDefault();

                foreach (var ev in agrupados)
                {
                    if (ev.IdCtTipoEventoPedidoDetalle == (byte)TipoEventoPedidoDetalleEnm.CANCELADO)
                    {
                        if (pasoCancelado != null)
                        {
                            pasoCancelado.SubEventos.Add(ev);
                        }
                        else if (pasoEnProceso != null)
                        {
                            pasoEnProceso.SubEventos.Add(ev);
                        }
                        else if (pasoFinal != null)
                        {
                            pasoFinal.SubEventos.Add(ev);
                        }
                    }
                    else
                    {
                        if (pasoEnProceso != null)
                        {
                            pasoEnProceso.SubEventos.Add(ev);
                        }
                        else if (pasoFinal != null)
                        {
                            pasoFinal.SubEventos.Add(ev);
                        }
                    }
                }
            }

            pedido.HistorialEstatus = pedido.HistorialEstatus
                .OrderBy(e => e.Fecha)
                .ThenBy(e => e.Id)
                .ToList();

            var saldoAcumulado = pedido.Total;

            foreach (var cobro in pedido.Cobros.OrderBy(c => c.Fecha))
            {
                cobro.SaldoInicial = saldoAcumulado;
                saldoAcumulado -= cobro.MontoAplicado;
            }

            return pedido;
        }
    }
}