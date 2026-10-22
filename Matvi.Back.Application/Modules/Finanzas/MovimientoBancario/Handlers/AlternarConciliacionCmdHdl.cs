using System.Threading;
using System.Threading.Tasks;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Commands;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Finanzas.MovimientoBancario.Handlers
{
    class AlternarConciliacionCmdHdl(
        IUnitOfWork _unitOfWork
    ) : IRequestHandler<AlternarConciliacionCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            AlternarConciliacionCmd request,
            CancellationToken cancellationToken)
        {
            var mov = await _unitOfWork.Query<OpMovimientoBancarioEtt>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (mov == null)
            {
                return OperationResult<bool>.Error("El movimiento bancario no fue encontrado.");
            }

            // Cambiar o forzar estado
            mov.Conciliado = request.ForzarEstado ?? !mov.Conciliado;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var mensaje = mov.Conciliado
                ? "Movimiento bancario conciliado correctamente."
                : "Se ha removido la conciliación del movimiento bancario.";

            return OperationResult<bool>.Success(mov.Conciliado, mensaje);
        }
    }
}
