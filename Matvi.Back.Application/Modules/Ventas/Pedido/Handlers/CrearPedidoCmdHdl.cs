using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Pedido.Commands;
using Matvi.Back.Application.Modules.Ventas.Pedido.Dtos;
using Matvi.Back.Application.Modules.Ventas.Pedido.Queries;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Common.Services;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Pedido.Handlers
{
    /// <summary>
    /// Handler para crear pedidos de venta.
    /// Implementa la matriz completa de reglas de negocio basada en:
    /// - Regularizado (RlProductoSucursal): controla si el sistema valida existencias
    /// - BajoPedido (CtProducto): permite pedir sin stock
    /// - EntregaMostrador (CtProducto): habilita al vendedor para entregar
    ///
    /// FASE 2: invoca ValidarCapturaPedidoQry antes de insertar (reglas Cliente y
    /// Vendedor); inicializa los nuevos campos de OpPedidoDetalle
    /// (IdCtEstatusPedidoDetalle, CantidadSurtida, CantidadCancelada); maneja la
    /// modalidad Apartado (sin inventario, sin reserva); usa AjustarEstatusPedidoCmd
    /// para registrar la transición inicial en OpPedidoEstatus.
    /// </summary>
    [Transaction]
    class CrearPedidoCmdHdl(
        IUnitOfWork _unitOfWork,
        IConsecutivoService _consecutivoService,
        IWebPushService _webPushService,
        ISender _sender,
        AjustarEstatusPedidoCmdHdl _ajustarEstatusHdl
    ) : IRequestHandler<CrearPedidoCmd, OperationResult<bool>>
    {
        // ID de cliente "Mostrador" según el seed del EST_Matvi.sql.
        private const int ID_CLIENTE_MOSTRADOR = 1;

                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            CrearPedidoCmd request,
            CancellationToken cancellationToken)
        {
            var pedidoDto = request.pedido;
            var fechaAhora = DateTime.Now;
            var esMostrador = pedidoDto.CtClie_IdFld == ID_CLIENTE_MOSTRADOR;

            #region 0. VALIDAR CAJA ABIERTA

            var cajaAbierta = _unitOfWork.Query<OpCorteCajaEtt>()
                .Any(c =>
                    c.IdCtSucursal == pedidoDto.CtSucu_Id &&
                    c.IdCtUsuario == pedidoDto.CtUsua_Id &&
                    c.FechaCierre == null);

            if (!cajaAbierta)
                return OperationResult<bool>.Error(
                    "Debe tener una caja abierta para registrar el pedido.");

            #endregion

            #region 0. VALIDACIÓN TEMPRANA DE CAPTURA (reglas Cliente y Vendedor)

            // Calculamos la existencia disponible por producto para alimentar
            // la validación de Vendedor-4 (BajoPedido sin stock no puede marcarse
            // EntregaMostrador). Usamos las mismas consultas de la región 1 para
            // no duplicar accesos a BD.
            //
            // La región 1 más abajo ya hace la validación fina de stock; aquí
            // solo necesitamos saber si hay >0 para Vendedor-4.

            // Construimos la lista de productos para ValidarCapturaPedido.
            // Se completa dentro de la región 1 para tener la existencia real.

            #endregion

            #region 1. CARGAR DATOS PREVIOS

            var idsProductos = pedidoDto.Productos!
                .Select(p => p.CtPresProd_IdCtProducto)
                .Distinct()
                .ToList();

            // Cargar relaciones producto-sucursal (Regularizado)
            var relProductosSucursal = _unitOfWork.Query<RlProductoSucursalEtt>()
                .Where(r => idsProductos.Contains(r.IdCtProducto)
                         && r.IdCtSucursal == pedidoDto.CtSucu_Id)
                .ToDictionary(r => r.IdCtProducto);

            // Cargar datos de productos (BajoPedido, EntregaMostrador y UnidadBase)
            var productos = _unitOfWork.Query<CtProductoEtt>()
                .Include(p => p.Nav_IdCtUnidadBaseInventario)
                .Where(p => idsProductos.Contains(p.Id))
                .ToDictionary(p => p.Id);

            // Cargar lotes disponibles (para stock).
            // Se incluyen todos los tipos de lote con CantidadDisponibleUBI > 0,
            // incluyendo el INICIAL, porque en él puede residir stock capturado
            // antes de la primera compra formal.
            var lotesDisponibles = _unitOfWork.Query<OpLoteEtt>()
                .Where(l => l.IdCtSucursal == pedidoDto.CtSucu_Id
                         && idsProductos.Contains(l.Cpp_IdCtProducto)
                         && l.CantidadDisponibleUBI > 0)
                .ToList();

            // Cargar lotes iniciales
            var lotesIniciales = _unitOfWork.Query<OpLoteEtt>()
                .Where(l => l.IdCtSucursal == pedidoDto.CtSucu_Id
                         && idsProductos.Contains(l.Cpp_IdCtProducto)
                         && l.IdCtTipoLote == (byte)TipoLoteEnm.INICIAL)
                .ToList();

            #endregion

            #region 1.5 VALIDACIÓN DE REGLAS DE CAPTURA (Cliente y Vendedor)

            // Construimos la lista que consume ValidarCapturaPedidoQryHdl.
            var totalPedido = pedidoDto.Productos!.Sum(p => p.Importe);

            var productosParaValidar = pedidoDto.Productos!
                .Select(p =>
                {
                    var rel = relProductosSucursal.GetValueOrDefault(p.CtPresProd_IdCtProducto);
                    var ctProd = productos.GetValueOrDefault(p.CtPresProd_IdCtProducto);

                    var stockBrutoUBI = lotesDisponibles
                        .Where(l => l.Cpp_IdCtProducto == p.CtPresProd_IdCtProducto)
                        .Sum(l => l.CantidadDisponibleUBI);

                    var stockRealUBI = stockBrutoUBI - (rel?.StockReservadoUBI ?? 0);

                    return new ValidarCapturaPedidoProducto(
                        IdProducto: p.CtPresProd_IdCtProducto,
                        ProductoEntregaMostrador: ctProd?.EntregaMostrador ?? false,
                        ProductoBajoPedido: ctProd?.BajoPedido ?? false,
                        ProductoMarcadoEntregadoMostrador: p.EntregadoMostrador,
                        ExistenciaDisponibleUBI: stockRealUBI
                    );
                })
                .ToList();

            var validacion = await _sender.Send(
                new ValidarCapturaPedidoQry(
                    IdSucursal: pedidoDto.CtSucu_Id,
                    IdCliente: pedidoDto.CtClie_IdFld,
                    IdCanalCaptura: pedidoDto.CtCanaCapt_IdFld,
                    EsMostrador: esMostrador,
                    Total: totalPedido,
                    PagoFld: pedidoDto.PagoFld,
                    EsApartado: pedidoDto.EsApartadoFld,
                    Productos: productosParaValidar),
                cancellationToken);

            if (!validacion.IsSuccessful)
            {
                return OperationResult<bool>.Info(
                    false,
                    validacion.Message ?? "Validación de captura fallida.");
            }

            #endregion

            #region 2. VALIDAR STOCK PARA PRODUCTOS REGULARIZADOS SIN BajoPedido

            // Si Regularizado = true y BajoPedido = false, debe haber stock suficiente.
            // Esta validación NO aplica a pedidos Apartados: por su naturaleza,
            // Apartado no toca inventario ni reserva (ver región 5).
            var erroresStock = new List<string>();

            if (!pedidoDto.EsApartadoFld)
            {
                foreach (var producto in pedidoDto.Productos!)
                {
                    var rel = relProductosSucursal.GetValueOrDefault(producto.CtPresProd_IdCtProducto);
                    var ctProd = productos.GetValueOrDefault(producto.CtPresProd_IdCtProducto);

                    if (ctProd == null) continue;

                    // Solo validar si está regularizado Y NO tiene BajoPedido
                    if (rel != null && rel.Regularizado && !ctProd.BajoPedido)
                    {
                        var cantidadUBI = producto.CantidadFld * producto.CtPresProd_FactorUBI;

                        var stockBrutoUBI = lotesDisponibles
                            .Where(l => l.Cpp_IdCtProducto == producto.CtPresProd_IdCtProducto)
                            .Sum(l => l.CantidadDisponibleUBI);

                        var stockRealUBI = stockBrutoUBI - rel.StockReservadoUBI;

                        if (stockRealUBI < cantidadUBI)
                        {
                            erroresStock.Add(
                                $"Stock insuficiente para '{producto.CtPresProd_Nombre}': " +
                                $"disponible {stockRealUBI:F2}, solicitado {cantidadUBI:F2}.");
                        }
                    }
                }

                if (erroresStock.Any())
                {
                    return OperationResult<bool>.Info(
                        false,
                        "No es posible crear el pedido:\n" + string.Join("\n", erroresStock) +
                        "\n\nHabilite 'BajoPedido' en el producto si desea permitir pedidos sin stock.");
                }
            }

            #endregion

            #region 3. GENERAR NÚMERO DE PEDIDO

            var numero = await _consecutivoService.GenerarNumeroAsync(
                pedidoDto.CtSucu_Id, (byte)TipoConsecutivoEnm.PEDIDO, cancellationToken);

            #endregion

            #region 4. CREAR PEDIDO

            // Determinar si todos los productos se entregan en mostrador
            bool todosMostrador = pedidoDto.Productos!.All(p => p.EntregadoMostrador);

            // Determinar estatus inicial según modalidad:
            // - APARTADO si el usuario marcó apartado (siempre y cuando ValidarCaptura
            //   haya confirmado que el cliente es registrado y el pago es total).
            // - ENTREGADO si todos los productos se entregan en mostrador.
            // - EN_PROCESO en cualquier otro caso.
            EstatusPedidoEnm estatusInicial;
            if (pedidoDto.EsApartadoFld)
            {
                estatusInicial = EstatusPedidoEnm.APARTADO;
            }
            else if (todosMostrador)
            {
                estatusInicial = EstatusPedidoEnm.ENTREGADO;
            }
            else
            {
                estatusInicial = EstatusPedidoEnm.EN_PROCESO;
            }

            var nvoPedidoEtt = new OpPedidoEtt
            {
                IdCtSucursal = pedidoDto.CtSucu_Id,
                Numero = numero,
                IdCtCliente = pedidoDto.CtClie_IdFld,
                NombreClienteMostrador = pedidoDto.ClienteMostradorNombreFld,
                TelefonoClienteMostrador = pedidoDto.ClienteMostradorTelefonoFld,
                IdCtCanalCaptura = pedidoDto.CtCanaCapt_IdFld,
                IdCtEstatusPedido = (byte)EstatusPedidoEnm.CREADO,
                EntregaDomicilio = pedidoDto.EntregaDomicilioFld,
                DireccionEntrega = pedidoDto.DireccionEntregaFld,
                FechaEntregaEstimada = pedidoDto.FechaEntregaEstimadaFld,
                Notas = pedidoDto.NotasFld,
                Total = totalPedido,
                TotalPagado = pedidoDto.PagoFld,
            };

            _unitOfWork.Add(nvoPedidoEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region 5. INSERTAR DETALLE DEL PEDIDO

            // En Apartado: ninguna línea tiene cantidad entregada aún (se entrega
            // cuando el cliente retire). En el resto de modalidades, se respeta
            // la marca EntregadoMostrador por línea.
            var lstPedidoDetalleEtt = pedidoDto.Productos!
                .Select(p =>
                {
                    bool entregadoMostrador = !pedidoDto.EsApartadoFld && p.EntregadoMostrador;
                    return new OpPedidoDetalleEtt
                    {
                        IdOpPedido = nvoPedidoEtt.Id,
                        IdCtEstatusPedidoDetalle = entregadoMostrador
                            ? (byte)EstatusPedidoDetalleEnm.CERRADO
                            : (byte)EstatusPedidoDetalleEnm.PENDIENTE,
                        Cpp_IdCtProducto = p.CtPresProd_IdCtProducto,
                        Cpp_Nombre = p.CtPresProd_Nombre,
                        Cpp_FactorUBI = p.CtPresProd_FactorUBI,
                        Cantidad = p.CantidadFld,
                        CantidadSurtida = entregadoMostrador ? p.CantidadFld : 0,
                        CantidadEntregada = entregadoMostrador ? p.CantidadFld : 0,
                        CantidadRechazada = 0,
                        CantidadCancelada = 0,
                        Precio = p.Precio,
                        Importe = p.Importe,
                        // El movimiento de inventario se registra y vincula por OpPedidoDetalleEvento
                    };
                }).ToList();

            _unitOfWork.AddRange(lstPedidoDetalleEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region 6. PROCESAR INVENTARIO SEGÚN MATRIZ DE REGLAS

            // APARTADO: salta toda la lógica de inventario. El surtido ocurre
            // cuando el cliente retira, y se registrará como evento SURTIDO
            // por línea cuando eso pase.
            if (!pedidoDto.EsApartadoFld)
            {
                foreach (var producto in pedidoDto.Productos!)
                {
                    var detalleEtt = lstPedidoDetalleEtt
                        .First(d => d.Cpp_IdCtProducto == producto.CtPresProd_IdCtProducto
                                 && d.Cpp_Nombre == producto.CtPresProd_Nombre);

                    var rel = relProductosSucursal.GetValueOrDefault(producto.CtPresProd_IdCtProducto);
                    var ctProd = productos.GetValueOrDefault(producto.CtPresProd_IdCtProducto);

                    var cantidadUBI = producto.CantidadFld * producto.CtPresProd_FactorUBI;

                    // Flags para la matriz
                    bool regularizado = rel?.Regularizado ?? false;
                    bool bajoPedido = ctProd?.BajoPedido ?? false;
                    bool entregaMostrador = ctProd?.EntregaMostrador ?? false;
                    bool marcarEntregado = producto.EntregadoMostrador;

                    #region CASO A: Producto se entrega en mostrador (marcarEntregado = true)

                    if (marcarEntregado)
                    {
                        // 1. Insertar evento SURTIDO (receptor NULL = cliente directo en mostrador)
                        var nvoEventoSurtido = new OpPedidoDetalleEventoEtt
                        {
                            IdOpPedidoDetalle = detalleEtt.Id,
                            IdCtTipoEventoPedidoDetalle = (byte)TipoEventoPedidoDetalleEnm.SURTIDO,
                            Cantidad = producto.CantidadFld,
                            Fecha = fechaAhora,
                            IdCtUsuario = pedidoDto.CtUsua_Id,
                            IdCtUsuarioRecibe = null,
                            Notas = "Entrega directa en mostrador al capturar"
                        };
                        _unitOfWork.Add(nvoEventoSurtido);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);

                        // Determinar qué lotes usar según regularización
                        List<OpLoteEtt> lotesParaDescontar;

                        if (!regularizado)
                        {
                            // Antes de regularizar: usar lote INICIAL
                            var loteInicial = await ObtenerOCrearLoteInicial(
                                producto, pedidoDto, fechaAhora, productos, lotesIniciales, cancellationToken);

                            lotesParaDescontar = new List<OpLoteEtt> { loteInicial };
                        }
                        else
                        {
                            // Después de regularizar: usar FIFO normal
                            lotesParaDescontar = lotesDisponibles
                                .Where(l => l.Cpp_IdCtProducto == producto.CtPresProd_IdCtProducto && l.CantidadDisponibleUBI > 0)
                                .OrderBy(l => l.Fecha)
                                .ThenBy(l => l.Id)
                                .ToList();

                            if (lotesParaDescontar.Count == 0)
                            {
                                var loteFallback = lotesIniciales.FirstOrDefault(l => l.Cpp_IdCtProducto == producto.CtPresProd_IdCtProducto)
                                    ?? await ObtenerOCrearLoteInicial(producto, pedidoDto, fechaAhora, productos, lotesIniciales, cancellationToken);

                                lotesParaDescontar = new List<OpLoteEtt> { loteFallback };
                            }
                        }

                        // Descontar de los lotes y generar movimientos vinculados al evento SURTIDO
                        var idPrimerMovimiento = await DescontarLotesYCrearMovimientos(
                            producto, detalleEtt, cantidadUBI, pedidoDto, fechaAhora,
                            lotesParaDescontar, nvoEventoSurtido.Id, regularizado, cancellationToken);

                        // 2. Insertar evento ENTREGADO
                        var nvoEventoEntrega = new OpPedidoDetalleEventoEtt
                        {
                            IdOpPedidoDetalle = detalleEtt.Id,
                            IdCtTipoEventoPedidoDetalle = (byte)TipoEventoPedidoDetalleEnm.ENTREGADO,
                            Cantidad = producto.CantidadFld,
                            Fecha = fechaAhora.AddMilliseconds(100),
                            IdCtUsuario = pedidoDto.CtUsua_Id,
                            Notas = "Entrega directa en mostrador al capturar"
                        };
                        _unitOfWork.Add(nvoEventoEntrega);

                        // Actualizar detalle con referencia al movimiento de salida principal
                        detalleEtt.IdOpMovimientoInventario_Salida = idPrimerMovimiento;
                        _unitOfWork.Update(detalleEtt);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }

                    #endregion

                    #region CASO B: Producto NO se entrega en mostrador (marcarEntregado = false)

                    else
                    {
                        // No se descuenta stock ahora, se reserva para cuando surta el almacenista
                        if (rel != null)
                        {
                            // Solo reservar si hay stock disponible (para productos normalizados)
                            if (regularizado)
                            {
                                var stockBrutoUBI = lotesDisponibles
                                    .Where(l => l.Cpp_IdCtProducto == producto.CtPresProd_IdCtProducto)
                                    .Sum(l => l.CantidadDisponibleUBI);

                                var stockRealUBI = stockBrutoUBI - rel.StockReservadoUBI;

                                if (stockRealUBI >= cantidadUBI)
                                {
                                    // Reservar stock
                                    rel.StockReservadoUBI += cantidadUBI;
                                    _unitOfWork.Update(rel);

                                    // Registrar evento RESERVADO
                                    var nvoEventoReservado = new OpPedidoDetalleEventoEtt
                                    {
                                        IdOpPedidoDetalle = detalleEtt.Id,
                                        IdCtTipoEventoPedidoDetalle = (byte)TipoEventoPedidoDetalleEnm.RESERVADO,
                                        Cantidad = producto.CantidadFld,
                                        Fecha = fechaAhora,
                                        IdCtUsuario = pedidoDto.CtUsua_Id,
                                        Notas = "Reserva de inventario para el pedido"
                                    };
                                    _unitOfWork.Add(nvoEventoReservado);
                                }
                                // Si no hay stock suficiente, no se reserva (seará cuando llegue)
                            }
                            // Si no está regularizado, no se reserva
                        }

                        // El movimiento se vincula a través de OpPedidoDetalleEvento
                    }

                    #endregion
                }

                if (pedidoDto.Productos!.Any(p => !p.EntregadoMostrador))
                {
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            #endregion

            #region 7. HISTORIAL DE ESTATUS (vía AjustarEstatusPedidoCmd)

            // Insertamos la fila de CREADO y la transición al estatus inicial
            // usando AjustarEstatusPedidoCmd, que valida la transición y se
            // ocupa de no duplicar filas si el estatus no cambia.
            var historialCread = new OpPedidoEstatusEtt
            {
                IdOpPedido = nvoPedidoEtt.Id,
                IdCtEstatusPedido = (byte)EstatusPedidoEnm.CREADO,
                Fecha = fechaAhora,
                IdCtUsuario = pedidoDto.CtUsua_Id,
                Notas = "Pedido capturado."
            };

            _unitOfWork.Add(historialCread);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Si el estatus inicial difiere de CREADO, registramos la transición
            // invocando el handler directamente. Esto es importante: NO usamos
            // _sender.Send aquí porque el handler AjustarEstatusPedidoCmdHdl
            // tiene [Transaction] y al pasar por MediatR se activa el
            // TransactionalCmdHandlerDecorator, que llama CommitAsync sobre
            // la transacción compartida de MatviCmdDbCtx (scoped), cerrándola
            // prematuramente y rompiendo el flujo del padre.
            if (estatusInicial != EstatusPedidoEnm.CREADO)
            {
                var ajusteResult = await _ajustarEstatusHdl.Handle(
                    new AjustarEstatusPedidoCmd(
                        new AjustarEstatusPedidoDto
                        {
                            Id = nvoPedidoEtt.Id,
                            NuevoEstatus = estatusInicial,
                            Notas = pedidoDto.EsApartadoFld
                                ? "Pedido apartado: pago 100% al capturar."
                                : (todosMostrador
                                    ? "Surtido completo en mostrador."
                                    : "Pendiente de surtido/almacenista."),
                            CtUsua_Id = pedidoDto.CtUsua_Id
                        }),
                    cancellationToken);

                if (!ajusteResult.IsSuccessful)
                {
                    return OperationResult<bool>.Info(
                        false,
                        ajusteResult.Message ?? "No se pudo registrar el estatus inicial.");
                }
            }

            #endregion

            #region 8. REGISTRAR COBRO SI APLICA

            if (pedidoDto.PagoFld > 0)
            {
                var numeroCobro = await _consecutivoService.GenerarNumeroAsync(
                    pedidoDto.CtSucu_Id, (byte)TipoConsecutivoEnm.COBRO, cancellationToken);

                var idCajaAbierta = _unitOfWork.Query<OpCorteCajaEtt>()
                    .Where(c =>
                        c.IdCtSucursal == pedidoDto.CtSucu_Id &&
                        c.IdCtUsuario == pedidoDto.CtUsua_Id &&
                        c.FechaCierre == null)
                    .Select(c => (int?)c.Id)
                    .FirstOrDefault();

                var nvoCobro = new OpCobroEtt
                {
                    IdCtSucursal = pedidoDto.CtSucu_Id,
                    Numero = numeroCobro,
                    Fecha = fechaAhora,
                    IdCtCliente = pedidoDto.CtClie_IdFld,
                    MontoTotal = pedidoDto.PagoFld,
                    IdCtMedioPago = pedidoDto.CtMediPago_IdFld,
                    IdCtUsuarioRecibe = pedidoDto.CtUsua_Id,
                    IdOpCorteCaja = idCajaAbierta,
                    Notas = null
                };

                _unitOfWork.Add(nvoCobro);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var nvoCobroDetalle = new OpCobroDetalleEtt
                {
                    IdOpCobro = nvoCobro.Id,
                    IdOpPedido = nvoPedidoEtt.Id,
                    MontoAplicado = pedidoDto.PagoFld
                };

                _unitOfWork.Add(nvoCobroDetalle);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #region REGISTRAR MOVIMIENTO BANCARIO

                byte? idCuentaBancaria = pedidoDto.CtCuentaBancaria_IdFld;
                if (!idCuentaBancaria.HasValue && pedidoDto.CtMediPago_IdFld != 1)
                {
                    idCuentaBancaria = _unitOfWork.Query<CtCuentaBancariaEtt>()
                        .Where(c => c.Activa)
                        .OrderBy(c => c.Id)
                        .Select(c => (byte?)c.Id)
                        .FirstOrDefault();
                }

                if (idCuentaBancaria.HasValue && (pedidoDto.CtMediPago_IdFld != 1 || pedidoDto.CtCuentaBancaria_IdFld.HasValue))
                {
                    var nvoMovimientoBancario = new OpMovimientoBancarioEtt
                    {
                        IdCtCuentaBancaria = idCuentaBancaria.Value,
                        Fecha = fechaAhora,
                        Tipo = (byte)TipoMovimientoBancarioEnm.INGRESO,
                        Monto = pedidoDto.PagoFld,
                        Referencia = $"COBRO #{numeroCobro}",
                        Conciliado = false,
                        IdOpCobro = nvoCobro.Id,
                        IdOpGasto = null,
                        IdOpPago = null
                    };

                    _unitOfWork.Add(nvoMovimientoBancario);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                #endregion
            }

            #endregion

            #region 9. NOTIFICACIÓN PUSH A ALMACENISTAS

            // No notificar si es Apartado (no hay nada que surtir todavía)
            // ni si todo se entregó en mostrador (no hay pendientes).
            if (!todosMostrador && !pedidoDto.EsApartadoFld)
            {
                var suscripciones = _unitOfWork
                    .Query<CtUsuarioWebPushSuscripcionEtt>()
                    .Where(s =>
                        s.Activa &&
                        s.Nav_IdCtUsuario!.IdCtRol == (byte)RolEnm.ALMACENISTA &&
                        s.Nav_IdCtUsuario.Coll_RlUsuarioSucursal_IdCtUsuario!
                            .Any(r => r.IdCtSucursal == pedidoDto.CtSucu_Id))
                    .ToList();

                if (suscripciones.Any())
                {
                    await _webPushService.EnviarAsync(
                        suscripciones,
                        titulo: "Nuevo pedido en preparación",
                        cuerpo: $"Pedido #{nvoPedidoEtt.Numero} listo para surtir.",
                        datos: new
                        {
                            tipo = "PEDIDO_EN_PREPARACION",
                            idPedido = nvoPedidoEtt.Id
                        },
                        cancellationToken);

                    // Limpiar suscripciones expiradas
                    var expiradas = suscripciones.Where(s => !s.Activa).ToList();
                    if (expiradas.Any())
                    {
                        expiradas.ForEach(s => _unitOfWork.Update(s));
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }
            }

            #endregion

            #region 9.1 NOTIFICACIÓN PUSH AL ADMINISTRADOR (APARTADOS)

            // Los pedidos apartados no surten todavía: se avisa al administrador de
            // la sucursal para dar seguimiento al cubrimiento/cobro del apartado.
            if (pedidoDto.EsApartadoFld)
            {
                var suscripcionesAdmin = _unitOfWork
                    .Query<CtUsuarioWebPushSuscripcionEtt>()
                    .Where(s =>
                        s.Activa &&
                        (s.Nav_IdCtUsuario!.IdCtRol == (byte)RolEnm.SYSTEM ||
                         s.Nav_IdCtUsuario.IdCtRol == (byte)RolEnm.ADMINISTRADOR) &&
                        s.Nav_IdCtUsuario.Coll_RlUsuarioSucursal_IdCtUsuario!
                            .Any(r => r.IdCtSucursal == pedidoDto.CtSucu_Id))
                    .ToList();

                if (suscripcionesAdmin.Any())
                {
                    await _webPushService.EnviarAsync(
                        suscripcionesAdmin,
                        titulo: "Nuevo pedido apartado",
                        cuerpo: $"Apartado #{nvoPedidoEtt.Numero} por ${pedidoDto.PagoFld:N2}, pendiente de cubrir.",
                        datos: new
                        {
                            tipo = "PEDIDO_APARTADO",
                            idPedido = nvoPedidoEtt.Id
                        },
                        cancellationToken);

                    var expiradasAdmin = suscripcionesAdmin.Where(s => !s.Activa).ToList();
                    if (expiradasAdmin.Any())
                    {
                        expiradasAdmin.ForEach(s => _unitOfWork.Update(s));
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                    }
                }
            }

            #endregion

            return OperationResult<bool>.Success(
                true,
                pedidoDto.EsApartadoFld
                    ? $"El pedido apartado #{numero} se creó correctamente."
                    : $"El pedido #{numero} se creó correctamente.");
        }

        #region MÉTODOS AUXILIARES

        /// <summary>
        /// Obtiene el lote inicial existente o lo crea si no existe.
        /// Se usa cuando el producto aún no está regularizado.
        /// </summary>
        private async Task<OpLoteEtt> ObtenerOCrearLoteInicial(
            AgregarProductoPedidoDto producto,
            CrearPedidoDto pedidoDto,
            DateTime fechaAhora,
            Dictionary<int, CtProductoEtt> productos,
            List<OpLoteEtt> lotesIniciales,
            CancellationToken cancellationToken)
        {
            var loteInicial = lotesIniciales
                .FirstOrDefault(l => l.Cpp_IdCtProducto == producto.CtPresProd_IdCtProducto);

            if (loteInicial == null)
            {
                var ctProducto = productos.GetValueOrDefault(producto.CtPresProd_IdCtProducto);

                var numeroLote = await _consecutivoService.GenerarNumeroAsync(
                    pedidoDto.CtSucu_Id, (byte)TipoConsecutivoEnm.LOTE, cancellationToken);

                loteInicial = new OpLoteEtt
                {
                    IdCtSucursal = pedidoDto.CtSucu_Id,
                    Numero = numeroLote,
                    IdCtTipoLote = (byte)TipoLoteEnm.INICIAL,
                    Cpp_IdCtProducto = producto.CtPresProd_IdCtProducto,
                    Cpp_Nombre = ctProducto?.Nav_IdCtUnidadBaseInventario?.Nombre ?? "UNIDAD",
                    Cpp_FactorUBI = 1,
                    CantidadInicialUBI = 0,
                    CantidadDisponibleUBI = 0,
                    CostoUP = 0,
                    CostoUBI = 0,
                    Fecha = fechaAhora,
                };

                _unitOfWork.Add(loteInicial);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                lotesIniciales.Add(loteInicial);
            }

            return loteInicial;
        }

        /// <summary>
        /// Descuenta cantidad de los lotes (FIFO) y crea movimientos de inventario.
        /// Retorna el ID del primer movimiento generado.
        /// </summary>
        private async Task<int?> DescontarLotesYCrearMovimientos(
            AgregarProductoPedidoDto producto,
            OpPedidoDetalleEtt detalleEtt,
            decimal cantidadUBI,
            CrearPedidoDto pedidoDto,
            DateTime fechaAhora,
            List<OpLoteEtt> lotes,
            int? idOpPedidoDetalleEvento,
            bool regularizado,
            CancellationToken cancellationToken)
        {
            var cantidadPendienteUBI = cantidadUBI;
            int? idPrimerMovimiento = null;

            if (!regularizado)
            {
                // Antes de regularizar: el lote inicial absorbe todo (puede quedar negativo)
                var loteInicial = lotes.First();
                loteInicial.CantidadDisponibleUBI -= cantidadPendienteUBI;
                _unitOfWork.Update(loteInicial);

                var numeroMovimiento = await _consecutivoService.GenerarNumeroAsync(
                    pedidoDto.CtSucu_Id,
                    (byte)TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO,
                    cancellationToken);

                var movimiento = new OpMovimientoInventarioEtt
                {
                    IdCtSucursal = pedidoDto.CtSucu_Id,
                    Numero = numeroMovimiento,
                    Cpp_IdCtProducto = producto.CtPresProd_IdCtProducto,
                    Cpp_Nombre = producto.CtPresProd_Nombre,
                    Cpp_FactorUBI = producto.CtPresProd_FactorUBI,
                    CantidadUBI = cantidadPendienteUBI,
                    Fecha = fechaAhora,
                    IdCtTipoMovimiento = (byte)TipoMovimientoEnm.SAL_VENTA,
                    IdOpPedidoDetalleEvento = idOpPedidoDetalleEvento,
                    IdOpLote = loteInicial.Id,
                    CostoUBI = loteInicial.CostoUBI,
                    CostoTotal = cantidadPendienteUBI * loteInicial.CostoUBI,
                    IdCtUsuario = pedidoDto.CtUsua_Id
                };

                _unitOfWork.Add(movimiento);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return movimiento.Id;
            }

            // Cuando está regularizado: FIFO estricto sobre los lotes disponibles
            foreach (var lote in lotes)
            {
                if (cantidadPendienteUBI <= 0) break;
                if (lote.CantidadDisponibleUBI <= 0) continue;

                var cantidadTomarUBI = Math.Min(cantidadPendienteUBI, lote.CantidadDisponibleUBI);

                lote.CantidadDisponibleUBI -= cantidadTomarUBI;
                _unitOfWork.Update(lote);

                var numeroMovimiento = await _consecutivoService.GenerarNumeroAsync(
                    pedidoDto.CtSucu_Id,
                    (byte)TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO,
                    cancellationToken);

                var movimiento = new OpMovimientoInventarioEtt
                {
                    IdCtSucursal = pedidoDto.CtSucu_Id,
                    Numero = numeroMovimiento,
                    Cpp_IdCtProducto = producto.CtPresProd_IdCtProducto,
                    Cpp_Nombre = producto.CtPresProd_Nombre,
                    Cpp_FactorUBI = producto.CtPresProd_FactorUBI,
                    CantidadUBI = cantidadTomarUBI,
                    Fecha = fechaAhora,
                    IdCtTipoMovimiento = (byte)TipoMovimientoEnm.SAL_VENTA,
                    IdOpPedidoDetalleEvento = idOpPedidoDetalleEvento,
                    IdOpLote = lote.Id,
                    CostoUBI = lote.CostoUBI,
                    CostoTotal = cantidadTomarUBI * lote.CostoUBI,
                    IdCtUsuario = pedidoDto.CtUsua_Id
                };

                _unitOfWork.Add(movimiento);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                idPrimerMovimiento ??= movimiento.Id;
                cantidadPendienteUBI -= cantidadTomarUBI;
            }

            // Si aún queda cantidad pendiente (por ejemplo, en caso de BajoPedido sin stock suficiente)
            if (cantidadPendienteUBI > 0)
            {
                var loteRemanente = lotes.LastOrDefault();
                if (loteRemanente != null)
                {
                    loteRemanente.CantidadDisponibleUBI -= cantidadPendienteUBI;
                    _unitOfWork.Update(loteRemanente);

                    var numeroMovimiento = await _consecutivoService.GenerarNumeroAsync(
                        pedidoDto.CtSucu_Id,
                        (byte)TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO,
                        cancellationToken);

                    var movimiento = new OpMovimientoInventarioEtt
                    {
                        IdCtSucursal = pedidoDto.CtSucu_Id,
                        Numero = numeroMovimiento,
                        Cpp_IdCtProducto = producto.CtPresProd_IdCtProducto,
                        Cpp_Nombre = producto.CtPresProd_Nombre,
                        Cpp_FactorUBI = producto.CtPresProd_FactorUBI,
                        CantidadUBI = cantidadPendienteUBI,
                        Fecha = fechaAhora,
                        IdCtTipoMovimiento = (byte)TipoMovimientoEnm.SAL_VENTA,
                        IdOpPedidoDetalleEvento = idOpPedidoDetalleEvento,
                        IdOpLote = loteRemanente.Id,
                        CostoUBI = loteRemanente.CostoUBI,
                        CostoTotal = cantidadPendienteUBI * loteRemanente.CostoUBI,
                        IdCtUsuario = pedidoDto.CtUsua_Id
                    };

                    _unitOfWork.Add(movimiento);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    idPrimerMovimiento ??= movimiento.Id;
                }
            }

            return idPrimerMovimiento;
        }

        #endregion
    }
}