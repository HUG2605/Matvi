using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Proveedor.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Handlers
{
    [Transaction]
    class EliminarProveedorCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarProveedorCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EliminarProveedorCmd request, 
            CancellationToken cancellationToken)
        {
            var proveedorEtt = await _unitOfWork.Query<CtProveedorEtt>()
             .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken)
             ?? throw new NotFoundException("Proveedor", request.id);

            _unitOfWork.Remove(proveedorEtt);

            return OperationResult<bool>.Success(
                true, 
                "El proveedor se eliminó correctamente.");
        }
    }
}
