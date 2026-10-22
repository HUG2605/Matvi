using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Producto.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Producto.Handlers
{
    [Transaction]
    class EliminarProductoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarProductoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EliminarProductoCmd request, 
            CancellationToken cancellationToken)
        {
            var productoEtt = await _unitOfWork.Query<CtProductoEtt>()
                .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken) 
                ?? throw new NotFoundException("Producto", request.id);

            _unitOfWork.Remove(productoEtt);

            return OperationResult<bool>.Success(
                true, 
                "El producto se eliminó correctamente.");
        }
    }
}
