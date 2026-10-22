using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.TipoGasto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.TipoGasto.Handlers
{
    [Transaction]
    class EliminarTipoGastoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarTipoGastoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EliminarTipoGastoCmd request, 
            CancellationToken cancellationToken)
        {
            var tipoGastoEtt = await _unitOfWork.Query<CtTipoGastoEtt>()
             .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken)
             ?? throw new NotFoundException("Tipo Gasto", request.id);

            _unitOfWork.Remove(tipoGastoEtt);

            return OperationResult<bool>.Success(
                true, 
                "El tipo de gasto se eliminó correctamente.");
        }
    }
}
