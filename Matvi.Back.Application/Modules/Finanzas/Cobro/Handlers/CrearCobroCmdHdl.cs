using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Cobro.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Common.Services;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Cobro.Handlers
{
    [Transaction]
    class CrearCobroCmdHdl(
        IUnitOfWork _unitOfWork,
        IConsecutivoService _consecutivoService
    ) : IRequestHandler<CrearCobroCmd, OperationResult<bool>>
    {
        /// <summary>
        /// 1. Verificar que el cliente tenga pedidos pendientes de cobro
        /// 2. Generar numero de cobro (BtConsecutivo)
        /// 3. Insertar OpCobro
        /// 4. Aplicar FIFO sobre pedidos pendientes del cliente
        ///     (orden por fecha de creación tomada del estatus CREADO,
        ///      ya que OpPedido ya no tiene columna Fecha propia)
        ///     #Crear OpCobroDetalle x pedidos afectados
        ///     #Actualizar TotalPagado en cada OpPedido afectado
        /// </summary>
        public async Task<OperationResult<bool>> Handle(
            CrearCobroCmd request,
            CancellationToken cancellationToken)
        {
            var cobroDto = request.cobro;
            var fechaAhora = DateTime.Now;

            #region VALIDAR CAJA ABIERTA

            var cajaAbierta = _unitOfWork.Query<OpCorteCajaEtt>()
                .Any(c =>
                    c.Id == cobroDto.IdOpCorteCaja &&
                    c.IdCtSucursal == cobroDto.CtSucu_Id &&
                    c.IdCtUsuario == cobroDto.CtUsua_Id &&
                    c.FechaCierre == null);

            if (!cajaAbierta)
                return OperationResult<bool>.Error(
                    "Debe tener una caja abierta para registrar el cobro.");

            #endregion

            #region VERIFICAR PEDIDOS PENDIENTES

            var pedidosPendientes = _unitOfWork.Query<OpPedidoEtt>()
                .Where(p =>
                    p.IdCtSucursal == cobroDto.CtSucu_Id &&
                    p.IdCtCliente == cobroDto.CtClie_IdFld &&
                    p.Total > p.TotalPagado)
                .Select(p => new
                {
                    Pedido = p,
                    FechaCreado = p.Coll_OpPedidoEstatus_IdOpPedido
                        .Where(e => e.IdCtEstatusPedido == (byte)EstatusPedidoEnm.CREADO)
                        .Select(e => e.Fecha)
                        .First()
                })
                .OrderBy(x => x.FechaCreado)
                .Select(x => x.Pedido)
                .ToList();

            if (pedidosPendientes.Count == 0)
                return OperationResult<bool>.Info(false,
                    "El cliente no tiene pedidos pendientes de cobro.");

            #endregion

            #region VALIDAR QUE EL MONTO NO EXCEDA EL SALDO PENDIENTE

            var saldoTotalPendiente = pedidosPendientes
                .Sum(p => p.Total - p.TotalPagado);

            if (cobroDto.MontoFld > saldoTotalPendiente)
                return OperationResult<bool>.Error(
                    $"El monto del cobro (${cobroDto.MontoFld:N2}) excede el saldo pendiente del cliente (${saldoTotalPendiente:N2}).");

            #endregion

            #region AGREGAR EL COBRO

            var numeroCobro = await _consecutivoService.GenerarNumeroAsync(
                cobroDto.CtSucu_Id, (byte)TipoConsecutivoEnm.COBRO, cancellationToken);

            var nvoCobroEtt = new OpCobroEtt
            {
                IdCtSucursal = cobroDto.CtSucu_Id,
                Numero = numeroCobro,
                Fecha = fechaAhora,
                IdCtCliente = cobroDto.CtClie_IdFld,
                MontoTotal = cobroDto.MontoFld,
                IdCtMedioPago = cobroDto.CtMediPago_IdFld,
                IdCtUsuarioRecibe = cobroDto.CtUsua_Id,
                IdOpCorteCaja = cobroDto.IdOpCorteCaja,
                Notas = cobroDto.NotasFld
            };

            _unitOfWork.Add(nvoCobroEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region APLICAR FIFO Y AGREGAR OPCOBRODETALLE

            var montoRestante = cobroDto.MontoFld;

            foreach (var pedido in pedidosPendientes)
            {
                if (montoRestante <= 0) break;

                var saldoPedido = pedido.Total - pedido.TotalPagado;
                var montoAplicado = Math.Min(montoRestante, saldoPedido);

                var nvoDetalleCobroEtt = new OpCobroDetalleEtt
                {
                    IdOpCobro = nvoCobroEtt.Id,
                    IdOpPedido = pedido.Id,
                    MontoAplicado = montoAplicado
                };

                _unitOfWork.Add(nvoDetalleCobroEtt);

                pedido.TotalPagado += montoAplicado;
                _unitOfWork.Update(pedido);

                montoRestante -= montoAplicado;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region REGISTRAR MOVIMIENTO BANCARIO

            byte? idCuentaBancaria = cobroDto.CtCuentaBancaria_IdFld;
            if (!idCuentaBancaria.HasValue && cobroDto.CtMediPago_IdFld != 1)
            {
                idCuentaBancaria = _unitOfWork.Query<CtCuentaBancariaEtt>()
                    .Where(c => c.Activa)
                    .OrderBy(c => c.Id)
                    .Select(c => (byte?)c.Id)
                    .FirstOrDefault();
            }

            if (idCuentaBancaria.HasValue && (cobroDto.CtMediPago_IdFld != 1 || cobroDto.CtCuentaBancaria_IdFld.HasValue))
            {
                var nvoMovimientoBancario = new OpMovimientoBancarioEtt
                {
                    IdCtCuentaBancaria = idCuentaBancaria.Value,
                    Fecha = fechaAhora,
                    Tipo = (byte)TipoMovimientoBancarioEnm.INGRESO,
                    Monto = cobroDto.MontoFld,
                    Referencia = $"COBRO #{numeroCobro}",
                    Conciliado = false,
                    IdOpCobro = nvoCobroEtt.Id,
                    IdOpGasto = null,
                    IdOpPago = null
                };

                _unitOfWork.Add(nvoMovimientoBancario);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            #endregion

            return OperationResult<bool>.Success(true, "El cobro se registró correctamente.");
        }
    }
}