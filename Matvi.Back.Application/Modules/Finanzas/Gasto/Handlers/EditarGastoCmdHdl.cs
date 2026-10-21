using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Handlers
{
    [Transaction]
    class EditarGastoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EditarGastoCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EditarGastoCmd request,
            CancellationToken cancellationToken)
        {
            var gastoDto = request.gasto;

            var gastoEtt = await _unitOfWork.FindAsync<OpGastoEtt, int>(gastoDto.Id, cancellationToken);
            if (gastoEtt == null)
            {
                return OperationResult<bool>.Error($"No se encontró el gasto con Id {gastoDto.Id}.");
            }

            gastoEtt.IdCtTipoGasto = gastoDto.CtTipoGast_Id;
            gastoEtt.Concepto = gastoDto.ConceptoFld;
            gastoEtt.Notas = gastoDto.NotasGastoFld;
            gastoEtt.IdCtMedioPago = gastoDto.CtMediPago_IdFld;
            gastoEtt.Proveedor = gastoDto.ProveedorFld;
            gastoEtt.Importe = gastoDto.ImporteFld;
            _unitOfWork.Update(gastoEtt);

            #region SINCRONIZAR MOVIMIENTO BANCARIO

            var movBancario = await _unitOfWork.Query<OpMovimientoBancarioEtt>()
                .FirstOrDefaultAsync(m => m.IdOpGasto == gastoEtt.Id, cancellationToken);

            if (gastoDto.CtCuentaBancaria_IdFld.HasValue)
            {
                if (movBancario != null)
                {
                    movBancario.IdCtCuentaBancaria = gastoDto.CtCuentaBancaria_IdFld.Value;
                    movBancario.Monto = gastoDto.ImporteFld;
                    movBancario.Referencia = $"GASTO #{gastoEtt.Numero}";
                    _unitOfWork.Update(movBancario);
                }
                else
                {
                    var nvoMovimientoBancario = new OpMovimientoBancarioEtt
                    {
                        IdCtCuentaBancaria = gastoDto.CtCuentaBancaria_IdFld.Value,
                        Fecha = DateTime.Now,
                        Tipo = (byte)TipoMovimientoBancarioEnm.EGRESO,
                        Monto = gastoDto.ImporteFld,
                        Referencia = $"GASTO #{gastoEtt.Numero}",
                        Conciliado = false,
                        IdOpCobro = null,
                        IdOpGasto = gastoEtt.Id,
                        IdOpPago = null
                    };
                    _unitOfWork.Add(nvoMovimientoBancario);
                }
            }
            else if (movBancario != null)
            {
                _unitOfWork.Remove(movBancario);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            return OperationResult<bool>.Success(
                true,
                "El gasto se editó correctamente.");
        }
    }
}