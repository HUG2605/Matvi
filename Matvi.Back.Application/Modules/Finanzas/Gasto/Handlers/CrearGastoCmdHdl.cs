using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Common.Services;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Handlers
{
    [Transaction]
    class CrearGastoCmdHdl(
        IUnitOfWork _unitOfWork,
        IConsecutivoService _consecutivoService
    ) : IRequestHandler<CrearGastoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            CrearGastoCmd request,
            CancellationToken cancellationToken)
        {
            var gastoDto = request.gasto;
            var fechaAhora = DateTime.Now;

            #region VALIDAR CAJA ABIERTA

            var cajaAbierta = _unitOfWork.Query<OpCorteCajaEtt>()
                .Any(c =>
                    c.Id == gastoDto.IdOpCorteCaja &&
                    c.IdCtSucursal == gastoDto.CtSucu_Id &&
                    c.IdCtUsuario == gastoDto.CtUsua_Id &&
                    c.FechaCierre == null);

            if (!cajaAbierta)
                return OperationResult<bool>.Error(
                    "Debe tener una caja abierta para registrar el gasto.");

            #endregion

            #region AGREGAR EL GASTO

            var numero = await _consecutivoService.GenerarNumeroAsync(
                gastoDto.CtSucu_Id, (byte)TipoConsecutivoEnm.GASTO, cancellationToken);

            var nvoGastoEtt = new OpGastoEtt
            {
                IdCtSucursal = gastoDto.CtSucu_Id,
                Numero = numero,
                FechaPago = fechaAhora,
                IdCtTipoGasto = gastoDto.CtTipoGast_Id,
                Proveedor = gastoDto.ProveedorFld,
                Concepto = gastoDto.ConceptoFld,
                Importe = gastoDto.ImporteFld,
                IdCtMedioPago = gastoDto.CtMediPago_IdFld,
                IdCtUsuario = gastoDto.CtUsua_Id,
                IdOpCorteCaja = gastoDto.IdOpCorteCaja,
                Notas = gastoDto.NotasGastoFld
            };

            _unitOfWork.Add(nvoGastoEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region REGISTRAR MOVIMIENTO BANCARIO

            byte? idCuentaBancaria = gastoDto.CtCuentaBancaria_IdFld;
            if (!idCuentaBancaria.HasValue && gastoDto.CtMediPago_IdFld != 1)
            {
                idCuentaBancaria = _unitOfWork.Query<CtCuentaBancariaEtt>()
                    .Where(c => c.Activa)
                    .OrderBy(c => c.Id)
                    .Select(c => (byte?)c.Id)
                    .FirstOrDefault();
            }

            if (idCuentaBancaria.HasValue && (gastoDto.CtMediPago_IdFld != 1 || gastoDto.CtCuentaBancaria_IdFld.HasValue))
            {
                var nvoMovimientoBancario = new OpMovimientoBancarioEtt
                {
                    IdCtCuentaBancaria = idCuentaBancaria.Value,
                    Fecha = fechaAhora,
                    Tipo = (byte)TipoMovimientoBancarioEnm.EGRESO,
                    Monto = gastoDto.ImporteFld,
                    Referencia = $"GASTO #{numero}",
                    Conciliado = false,
                    IdOpCobro = null,
                    IdOpGasto = nvoGastoEtt.Id,
                    IdOpPago = null
                };

                _unitOfWork.Add(nvoMovimientoBancario);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            #endregion

            return OperationResult<bool>.Success(true, "El gasto se registró correctamente.");
        }
    }
}