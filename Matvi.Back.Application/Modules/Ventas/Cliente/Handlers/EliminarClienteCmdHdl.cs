using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Ventas.Cliente.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Ventas.Cliente.Handlesrs
{
    [Transaction]
    class EliminarClienteCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarClienteCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EliminarClienteCmd request, 
            CancellationToken cancellationToken)
        {
            var clienteEtt = await _unitOfWork.Query<CtClienteEtt>()
              .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken)
              ?? throw new NotFoundException("Cliente", request.id);

            _unitOfWork.Remove(clienteEtt);

            return OperationResult<bool>.Success(
                true, 
                "El cliente se eliminó correctamente.");
        }
    }
}
