using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Handlers
{
    [Transaction]
    class EliminarSucursalCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarSucursalCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EliminarSucursalCmd request, 
            CancellationToken cancellationToken)
        {
            var sucursalEtt = await _unitOfWork.Query<CtSucursalEtt>()
                .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken)
                ?? throw new NotFoundException("Sucursal", request.id);

            _unitOfWork.Remove(sucursalEtt);

            return OperationResult<bool>.Success(
                true, 
                "La sucursal se eliminó correctamente.");
        }
    }
}
