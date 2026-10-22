using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Handlers
{
    [Transaction]
    class EliminarPresentacionProductoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarPresentacionProductoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EliminarPresentacionProductoCmd request, 
            CancellationToken cancellationToken)
        {
            var presentacionProductoEtt = await _unitOfWork.Query<CtPresentacionProductoEtt>()
            .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken)
            ?? throw new NotFoundException("ProductoPresentacion", request.id);

            _unitOfWork.Remove(presentacionProductoEtt);

            return OperationResult<bool>.Success(
                true,
                "La presentacion del producto se eliminó correctamente.");
        }
    }
}
