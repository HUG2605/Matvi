using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.Gasto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.Gasto.Handlers
{
    [Transaction]
    class EliminarGastoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarGastoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EliminarGastoCmd request,
            CancellationToken cancellationToken)
        {
            var gastoEtt = await _unitOfWork.Query<OpGastoEtt>()
                .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken)
                ?? throw new NotFoundException("Gasto", request.id);

            #region ELIMINAR MOVIMIENTO BANCARIO ASOCIADO

            var movsBancarios = await _unitOfWork.Query<OpMovimientoBancarioEtt>()
                .Where(m => m.IdOpGasto == request.id)
                .ToListAsync(cancellationToken);

            foreach (var mov in movsBancarios)
            {
                _unitOfWork.Remove(mov);
            }

            #endregion

            _unitOfWork.Remove(gastoEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(
                true,
                "El gasto se eliminó correctamente.");
        }
    }
}

