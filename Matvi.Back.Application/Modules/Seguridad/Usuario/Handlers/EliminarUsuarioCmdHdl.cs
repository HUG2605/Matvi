using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Handlers
{
    [Transaction]
    class EliminarUsuarioCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EliminarUsuarioCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EliminarUsuarioCmd request, 
            CancellationToken cancellationToken)
        {
            var usuarioEtt = await _unitOfWork.Query<CtUsuarioEtt>()
                .SingleOrDefaultAsync(i => i.Id == request.id, cancellationToken)
                ?? throw new NotFoundException("Usuario", request.id);

            _unitOfWork.Remove(usuarioEtt);

            return OperationResult<bool>.Success(
                true,
                "El usuario se eliminó correctamente."
            );
        }
    }
}
