using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Handlers
{
    [Transaction]
    class EditarTipoGastoCmdHdl(
       IUnitOfWork _unitOfWork
       ) : IRequestHandler<EditarTipoGastoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EditarTipoGastoCmd request, 
            CancellationToken cancellationToken)
        {
            var tipoGastoDto = request.tipoGasto;

            var tipoGastoEtt = await _unitOfWork.FindAsync<CtTipoGastoEtt, byte>(tipoGastoDto.Id, cancellationToken);
            tipoGastoEtt!.Nombre = tipoGastoDto.NombreFld;

            _unitOfWork.Update(tipoGastoEtt);

            return OperationResult<bool>.Success(
                true, 
                "El tipo de gasto se editó correctamente.");
        }
    }
}
