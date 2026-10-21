// CrearPagoCmdHdl.cs
using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Pago.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Common.Services;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Pago.Handlers
{
    [Transaction]
    class CrearPagoCmdHdl(
        IUnitOfWork _unitOfWork,
        IConsecutivoService _consecutivoService
    ) : IRequestHandler<CrearPagoCmd, OperationResult<bool>>
    {
        /// <summary>
        /// 1. Generar numero de pago (BtConsecutivo)
        /// 2. Insertar OpPago
        /// 3. Aplicar FIFO sobre compras pendientes del proveedor
        ///     #Crear OpPagoDetalle x compras afectadas
        ///     #Actualizar TotalPagado en cada OpCompra afectada
        /// 4. Si CtCuentaBancaria presente, registrar OpMovimientoBancario
        /// </summary>
        public async Task<OperationResult<bool>> Handle(
            CrearPagoCmd request,
            CancellationToken cancellationToken)
        {
            var pagoDto = request.pago;
            var fechaAhora = DateTime.Now;

            #region VALIDAR CAJA ABIERTA

            var cajaAbierta = _unitOfWork.Query<OpCorteCajaEtt>()
                .Any(c =>
                    c.Id == pagoDto.IdOpCorteCaja &&
                    c.IdCtSucursal == pagoDto.CtSucu_Id &&
                    c.IdCtUsuario == pagoDto.CtUsua_Id &&
                    c.FechaCierre == null);

            if (!cajaAbierta)
                return OperationResult<bool>.Error(
                    "Debe tener una caja abierta para registrar el pago.");

            #endregion

            // Verificar que existan compras pendientes
            var comprasPendientes = _unitOfWork.Query<OpCompraEtt>()
                .Where(c =>
                    c.IdCtSucursal == pagoDto.CtSucu_Id &&
                    c.IdCtProveedor == pagoDto.CtProv_IdFld &&
                    c.Total > c.TotalPagado)
                .OrderBy(c => c.Fecha)
                .ToList();

            if (comprasPendientes.Count == 0)
                return OperationResult<bool>.Info(false,
                    "El proveedor no tiene compras pendientes de pago.");

            #region AGREGAR EL PAGO

            var numeroPago = await _consecutivoService.GenerarNumeroAsync(
                pagoDto.CtSucu_Id, (byte)TipoConsecutivoEnm.PAGO, cancellationToken);

            var nvoPagoEtt = new OpPagoEtt
            {
                IdCtSucursal = pagoDto.CtSucu_Id,
                Numero = numeroPago,
                Fecha = fechaAhora,
                IdCtProveedor = pagoDto.CtProv_IdFld,
                MontoTotal = pagoDto.MontoFld,
                IdCtMedioPago = pagoDto.CtMediPago_IdFld,
                IdCtUsuarioPaga = pagoDto.CtUsua_Id,
                IdOpCorteCaja = pagoDto.IdOpCorteCaja,
                Notas = pagoDto.NotasPagoFld
            };

            _unitOfWork.Add(nvoPagoEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region APLICAR FIFO Y AGREGAR OPPAGODETALLE

            var montoRestante = pagoDto.MontoFld;

            foreach (var compra in comprasPendientes)
            {
                if (montoRestante <= 0) break;

                var saldoCompra = compra.Total - compra.TotalPagado;
                var montoAplicado = Math.Min(montoRestante, saldoCompra);

                var nvoDetallePagoEtt = new OpPagoDetalleEtt
                {
                    IdOpPago = nvoPagoEtt.Id,
                    IdOpCompra = compra.Id,
                    MontoAplicado = montoAplicado
                };

                _unitOfWork.Add(nvoDetallePagoEtt);

                compra.TotalPagado += montoAplicado;
                _unitOfWork.Update(compra);

                montoRestante -= montoAplicado;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region REGISTRAR MOVIMIENTO BANCARIO

            byte? idCuentaBancaria = pagoDto.CtCuentaBancaria_IdFld;
            if (!idCuentaBancaria.HasValue && pagoDto.CtMediPago_IdFld != 1)
            {
                idCuentaBancaria = _unitOfWork.Query<CtCuentaBancariaEtt>()
                    .Where(c => c.Activa)
                    .OrderBy(c => c.Id)
                    .Select(c => (byte?)c.Id)
                    .FirstOrDefault();
            }

            if (idCuentaBancaria.HasValue && (pagoDto.CtMediPago_IdFld != 1 || pagoDto.CtCuentaBancaria_IdFld.HasValue))
            {
                var nvoMovimientoBancario = new OpMovimientoBancarioEtt
                {
                    IdCtCuentaBancaria = idCuentaBancaria.Value,
                    Fecha = fechaAhora,
                    Tipo = (byte)TipoMovimientoBancarioEnm.EGRESO,
                    Monto = pagoDto.MontoFld,
                    Referencia = $"PAGO #{numeroPago}",
                    Conciliado = false,
                    IdOpCobro = null,
                    IdOpGasto = null,
                    IdOpPago = nvoPagoEtt.Id
                };

                _unitOfWork.Add(nvoMovimientoBancario);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            #endregion

            return OperationResult<bool>.Success(true, "El pago se registró correctamente.");
        }
    }
}