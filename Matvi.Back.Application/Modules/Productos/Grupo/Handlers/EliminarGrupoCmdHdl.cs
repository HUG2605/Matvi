using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Grupo.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Handlers
{
    [Transaction]
    class EliminarGrupoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarGrupoCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EliminarGrupoCmd request, 
            CancellationToken cancellationToken)
        {
            var grupoEtt = await _unitOfWork.Query<CtGrupoEtt>()
            .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken)
            ?? throw new NotFoundException("Grupo", request.id);

            _unitOfWork.Remove(grupoEtt);

            return OperationResult<bool>.Success(
                true, 
                "El grupo se eliminó correctamente.");
        }
    }
}
