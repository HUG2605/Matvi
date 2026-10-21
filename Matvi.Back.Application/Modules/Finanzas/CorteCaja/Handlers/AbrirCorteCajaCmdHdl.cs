using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.CorteCaja.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Common.Services;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.CorteCaja.Handlers
{
    [Transaction]
    class AbrirCorteCajaCmdHdl(
        IUnitOfWork _unitOfWork,
        IConsecutivoService _consecutivoService
    ) : IRequestHandler<AbrirCorteCajaCmd, OperationResult<bool>>
    {
        /// <summary>
        /// 1. Validar que el usuario no tenga una caja abierta
        /// 2. Generar numero de corte de caja (BtConsecutivo)
        /// 3. Insertar OpCorteCaja con FechaApertura actual y FondoInicial
        /// </summary>
        public async Task<OperationResult<bool>> Handle(
            AbrirCorteCajaCmd request,
            CancellationToken cancellationToken)
        {
            var corteDto = request.corte;

            #region VALIDAR QUE NO EXISTA UNA CAJA ABIERTA

            var cajaAbierta = _unitOfWork.Query<OpCorteCajaEtt>()
                .Any(c =>
                    c.IdCtSucursal == corteDto.CtSucu_Id &&
                    c.IdCtUsuario == corteDto.CtUsua_Id &&
                    c.FechaCierre == null);

            if (cajaAbierta)
                return OperationResult<bool>.Warning(false,
                    "Ya tiene una caja abierta. Debe cerrarla antes de abrir una nueva.");

            #endregion

            #region AGREGAR LA CAJA

            var numeroCorte = await _consecutivoService.GenerarNumeroAsync(
                corteDto.CtSucu_Id, (byte)TipoConsecutivoEnm.CORTE_DE_CAJA, cancellationToken);

            var nvaCajaEtt = new OpCorteCajaEtt
            {
                IdCtSucursal = corteDto.CtSucu_Id,
                Numero = numeroCorte,
                IdCtUsuario = corteDto.CtUsua_Id,
                FechaApertura = DateTime.Now,
                FondoInicial = corteDto.FondoInicial,
                Notas = corteDto.Notas
            };

            _unitOfWork.Add(nvaCajaEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region LIGADO DE RESPALDO DE MOVIMIENTOS PENDIENTES

            // Los movimientos (cobros/pagos/gastos/entregas) capturados antes de
            // abrir la caja no tienen caja asignada: se ligan a esta nueva caja
            // cuando pertenecen al periodo desde la última caja cerrada (o del día
            // en curso) hasta la apertura actual.

            var inicioPeriodo = _unitOfWork.Query<OpCorteCajaEtt>()
                .Where(x =>
                    x.IdCtSucursal == corteDto.CtSucu_Id &&
                    x.IdCtUsuario == corteDto.CtUsua_Id &&
                    x.FechaCierre != null)
                .OrderByDescending(x => x.FechaCierre!.Value)
                .Select(x => x.FechaCierre!.Value)
                .FirstOrDefault();

            if (inicioPeriodo == default)
                inicioPeriodo = DateTime.Today;

            var fechaApertura = nvaCajaEtt.FechaApertura;

            var cobrosPeriodo = _unitOfWork.Query<OpCobroEtt>()
                .Where(c =>
                    c.IdCtSucursal == corteDto.CtSucu_Id &&
                    c.IdCtUsuarioRecibe == corteDto.CtUsua_Id &&
                    c.IdOpCorteCaja == null &&
                    c.Fecha >= inicioPeriodo &&
                    c.Fecha <= fechaApertura)
                .ToList();

            foreach (var cobro in cobrosPeriodo)
            {
                cobro.IdOpCorteCaja = nvaCajaEtt.Id;
                _unitOfWork.Update(cobro);
            }

            var gastosPeriodo = _unitOfWork.Query<OpGastoEtt>()
                .Where(c =>
                    c.IdCtSucursal == corteDto.CtSucu_Id &&
                    c.IdCtUsuario == corteDto.CtUsua_Id &&
                    c.IdOpCorteCaja == null &&
                    c.FechaPago >= inicioPeriodo &&
                    c.FechaPago <= fechaApertura)
                .ToList();

            foreach (var gasto in gastosPeriodo)
            {
                gasto.IdOpCorteCaja = nvaCajaEtt.Id;
                _unitOfWork.Update(gasto);
            }

            var pagosPeriodo = _unitOfWork.Query<OpPagoEtt>()
                .Where(c =>
                    c.IdCtSucursal == corteDto.CtSucu_Id &&
                    c.IdCtUsuarioPaga == corteDto.CtUsua_Id &&
                    c.IdOpCorteCaja == null &&
                    c.Fecha >= inicioPeriodo &&
                    c.Fecha <= fechaApertura)
                .ToList();

            foreach (var pago in pagosPeriodo)
            {
                pago.IdOpCorteCaja = nvaCajaEtt.Id;
                _unitOfWork.Update(pago);
            }

            if (cobrosPeriodo.Count > 0 || gastosPeriodo.Count > 0 ||
                pagosPeriodo.Count > 0)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            #endregion

            return OperationResult<bool>.Success(true, "La caja se abrió correctamente.");
        }
    }
}