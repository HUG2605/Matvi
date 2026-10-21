using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Compra.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Common.Services;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Matvi.Back.Application.Modules.Compras.Compra.Handlers
{
    [Transaction]
    class CrearCompraCmdHdl(
        IUnitOfWork _unitOfWork,
        IConsecutivoService _consecutivoService
    ) : IRequestHandler<CrearCompraCmd, OperationResult<bool>>
    {
        /// <summary>
        /// 1. Generar numero de compra (BtConsecutivo)
        /// 2. Insertar OpCompra
        /// 3. Por cada uno de los productos de la compra:
        ///     #Insertar en OpCompraDetalle
        ///     #Insertar en OpLote (tipo COMPRA)
        ///     #Insertar en OpMovimientoInventario (tipo COMPRA)
        /// 4. Si pago > 0:
        ///     #Aplicar FIFO sobre compras pendientes del proveedor 
        ///     #Crear OpPago
        ///     #Crear OpPagoDetalle x compras afectadas
        ///     #Actualizar TotalPagado en cada OpCompra afectada
        /// </summary>
        public async Task<OperationResult<bool>> Handle(
            CrearCompraCmd request,
            CancellationToken cancellationToken)
        {
            if (request.compra.Productos == null || request.compra.Productos.Count == 0)
                return OperationResult<bool>.Info(false,
                    "Debe agregar al menos un producto a la compra.");

            var compraDto = request.compra;
            var fechaAhora = DateTime.Now;

            #region VALIDAR CAJA ABIERTA

            var cajaAbierta = _unitOfWork.Query<OpCorteCajaEtt>()
                .Any(c =>
                    c.IdCtSucursal == compraDto.CtSucu_Id &&
                    c.IdCtUsuario == compraDto.CtUsua_Id &&
                    c.FechaCierre == null);

            if (!cajaAbierta)
                return OperationResult<bool>.Error(
                    "Debe tener una caja abierta para registrar la compra.");

            #endregion

            #region VALIDAR PRODUCTOS REGULARIZADOS

            var idsProductos = compraDto.Productos
                .Select(p => p.CtPresProd_IdCtProducto)
                .Distinct()
                .ToList();

            var relProductosSucursal = _unitOfWork.Query<RlProductoSucursalEtt>()
                .Where(r => idsProductos.Contains(r.IdCtProducto)
                         && r.IdCtSucursal == compraDto.CtSucu_Id)
                .ToDictionary(r => r.IdCtProducto);

            var productosNoRegularizados = new List<string>();

            foreach (var producto in compraDto.Productos)
            {
                var rel = relProductosSucursal.GetValueOrDefault(producto.CtPresProd_IdCtProducto);
                if (rel == null || !rel.Regularizado)
                {
                    if (!productosNoRegularizados.Contains(producto.CtPresProd_Nombre))
                    {
                        productosNoRegularizados.Add(producto.CtPresProd_Nombre);
                    }
                }
            }

            if (productosNoRegularizados.Count > 0)
            {
                return OperationResult<bool>.Info(
                    false,
                    "No es posible registrar la compra. Los siguientes productos deben regularizarse en el inventario antes de su primera compra:\n" +
                    string.Join("\n", productosNoRegularizados.Select(p => $"• {p}")));
            }

            #endregion

            #region AGREGAR LA COMPRA

            var numero = await _consecutivoService.GenerarNumeroAsync(
                compraDto.CtSucu_Id, (byte)TipoConsecutivoEnm.COMPRA, cancellationToken);

            var nvoCompraEtt = new OpCompraEtt
            {
                IdCtSucursal = compraDto.CtSucu_Id,
                Numero = numero,
                Fecha = fechaAhora,
                IdCtProveedor = compraDto.CtProv_IdFld,
                Total = compraDto.Productos.Sum(p => p.Importe),
                TotalPagado = 0

            };

            _unitOfWork.Add(nvoCompraEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region POR CADA PRODUCTO

            foreach (var item in compraDto.Productos)
            {
                // Calculos en UBI
                var cantidadUBI = item.CantidadFld * item.CtPresProd_FactorUBI;
                var costoUBI = item.CtPresProd_FactorUBI == 0 ? 0
                                    : Math.Round(item.CostoUP / item.CtPresProd_FactorUBI, 5);
                var costoTotalUBI = cantidadUBI * costoUBI;

                #region AGREGAR EN OPLOTE

                var numeroLote = await _consecutivoService.GenerarNumeroAsync(
                    compraDto.CtSucu_Id, (byte)TipoConsecutivoEnm.LOTE, cancellationToken);

                var nvoLoteEtt = new OpLoteEtt
                {
                    IdCtSucursal = compraDto.CtSucu_Id,
                    Numero = numeroLote,
                    IdCtTipoLote = (byte)TipoLoteEnm.COMPRA,
                    Cpp_IdCtProducto = item.CtPresProd_IdCtProducto,
                    Cpp_Nombre = item.CtPresProd_Nombre,
                    Cpp_FactorUBI = item.CtPresProd_FactorUBI,
                    CantidadInicialUBI = cantidadUBI,
                    CantidadDisponibleUBI = cantidadUBI,
                    CostoUP = item.CostoUP,
                    CostoUBI = costoUBI,
                    Fecha = fechaAhora
                };

                _unitOfWork.Add(nvoLoteEtt);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #endregion

                #region AGREGAR EN OPCOMPRADETALLE

                var nvoDetalleEtt = new OpCompraDetalleEtt
                {
                    IdOpCompra = nvoCompraEtt.Id,
                    Cpp_IdCtProducto = item.CtPresProd_IdCtProducto,
                    Cpp_Nombre = item.CtPresProd_Nombre,
                    Cpp_FactorUBI = item.CtPresProd_FactorUBI,
                    Cantidad = item.CantidadFld,
                    CostoUP = item.CostoUP,
                    CostoUBI = costoUBI,
                    Importe = item.Importe,
                    IdOpLote = nvoLoteEtt.Id
                };

                _unitOfWork.Add(nvoDetalleEtt);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #endregion

                #region AGREGAR EN OPMOVIMIENTO DE INVENTARIO

                var numeroMovimiento = await _consecutivoService.GenerarNumeroAsync(
                    compraDto.CtSucu_Id, (byte)TipoConsecutivoEnm.MOVIMIENTO_DE_INVENTARIO, cancellationToken);

                var nvoMovimientoEtt = new OpMovimientoInventarioEtt
                {
                    IdCtSucursal = compraDto.CtSucu_Id,
                    Numero = numeroMovimiento,
                    Cpp_IdCtProducto = item.CtPresProd_IdCtProducto,
                    Cpp_Nombre = item.CtPresProd_Nombre,
                    Cpp_FactorUBI = item.CtPresProd_FactorUBI,
                    CantidadUBI = cantidadUBI,
                    Fecha = fechaAhora,
                    IdCtTipoMovimiento = (byte)TipoMovimientoEnm.ENT_COMPRA,
                    IdOpPedidoDetalleEvento = null,
                    IdOpCompraDetalle = nvoDetalleEtt.Id,
                    IdOpTraspasoDetalle = null,
                    IdOpLote = nvoLoteEtt.Id,
                    CostoUBI = costoUBI,
                    CostoTotal = costoTotalUBI,
                    IdCtUsuario = compraDto.CtUsua_Id
                };

                _unitOfWork.Add(nvoMovimientoEtt);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #endregion
            }

            #endregion

            #region AGREGAR EN OPPAGO

            if (compraDto.PagoFld > 0)
            {
                var numeroPago = await _consecutivoService.GenerarNumeroAsync(
                    compraDto.CtSucu_Id, (byte)TipoConsecutivoEnm.PAGO, cancellationToken);

                var idCajaAbierta = _unitOfWork.Query<OpCorteCajaEtt>()
                    .Where(c =>
                        c.IdCtSucursal == compraDto.CtSucu_Id &&
                        c.IdCtUsuario == compraDto.CtUsua_Id &&
                        c.FechaCierre == null)
                    .Select(c => (int?)c.Id)
                    .FirstOrDefault();

                var nvoPagoEtt = new OpPagoEtt
                {
                    IdCtSucursal = compraDto.CtSucu_Id,
                    Numero = numeroPago,
                    Fecha = fechaAhora,
                    IdCtProveedor = compraDto.CtProv_IdFld,
                    MontoTotal = compraDto.PagoFld,
                    IdCtMedioPago = compraDto.CtMediPago_IdFld,
                    IdCtUsuarioPaga = compraDto.CtUsua_Id,
                    IdOpCorteCaja = idCajaAbierta,
                    Notas = compraDto.NotasPagoFld
                };

                _unitOfWork.Add(nvoPagoEtt);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #region APLICAR FIFO Y AGREGAR OPPAGODETALLE

                var comprasPendientes = _unitOfWork.Query<OpCompraEtt>()
                    .Where(c =>
                        c.IdCtSucursal == compraDto.CtSucu_Id &&
                        c.IdCtProveedor == compraDto.CtProv_IdFld &&
                        c.Total > c.TotalPagado)
                    .OrderBy(c => c.Fecha)
                    .ToList();

                var montoRestante = compraDto.PagoFld;

                foreach (var compra in comprasPendientes)
                {
                    if (montoRestante <= 0) break;

                    var saldoCompra = compra.Total - compra.TotalPagado;
                    var montoAplicado = Math.Min(montoRestante, saldoCompra);

                    // Crear detalle del pago
                    var nvoDetallePagoEtt = new OpPagoDetalleEtt
                    {
                        IdOpPago = nvoPagoEtt.Id,
                        IdOpCompra = compra.Id,
                        MontoAplicado = montoAplicado
                    };

                    _unitOfWork.Add(nvoDetallePagoEtt);

                    // Actualizar TotalPagado — usar Update para que EF lo detecte
                    compra.TotalPagado += montoAplicado;
                    _unitOfWork.Update(compra);

                    montoRestante -= montoAplicado;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #endregion

                #region REGISTRAR MOVIMIENTO BANCARIO

                byte? idCuentaBancaria = compraDto.CtCuentaBancaria_IdFld;
                if (!idCuentaBancaria.HasValue && compraDto.CtMediPago_IdFld != 1)
                {
                    idCuentaBancaria = _unitOfWork.Query<CtCuentaBancariaEtt>()
                        .Where(c => c.Activa)
                        .OrderBy(c => c.Id)
                        .Select(c => (byte?)c.Id)
                        .FirstOrDefault();
                }

                if (idCuentaBancaria.HasValue && (compraDto.CtMediPago_IdFld != 1 || compraDto.CtCuentaBancaria_IdFld.HasValue))
                {
                    var nvoMovimientoBancario = new OpMovimientoBancarioEtt
                    {
                        IdCtCuentaBancaria = idCuentaBancaria.Value,
                        Fecha = fechaAhora,
                        Tipo = (byte)TipoMovimientoBancarioEnm.EGRESO,
                        Monto = compraDto.PagoFld,
                        Referencia = $"COMPRA #{numero}",
                        Conciliado = false,
                        IdOpCobro = null,
                        IdOpGasto = null,
                        IdOpPago = nvoPagoEtt.Id
                    };

                    _unitOfWork.Add(nvoMovimientoBancario);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                #endregion
            }

            #endregion

            return OperationResult<bool>.Success(true, "La compra se registró correctamente.");
        }
    }
}