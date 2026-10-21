using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Seccion.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Handlers
{
    [Transaction]
    class EliminarSeccionCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarSeccionCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EliminarSeccionCmd request, 
            CancellationToken cancellationToken)
        {
            var seccionEtt = await _unitOfWork.Query<CtSeccionEtt>()
              .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken)
              ?? throw new NotFoundException("Seccion", request.id);

            _unitOfWork.Remove(seccionEtt);

            return OperationResult<bool>.Success(
                true, 
                "La seccion se eliminó correctamente.");
        }
    }
}
