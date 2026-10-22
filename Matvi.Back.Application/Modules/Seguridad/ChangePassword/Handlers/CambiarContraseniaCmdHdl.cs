using Matvi.Back.Application.Common.Extensions;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.ChangePassword.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using Matvi.Transversal.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.ChangePassword.Handlers
{
    [Transaction]
    class CambiarContraseniaCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CambiarContraseniaCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            CambiarContraseniaCmd request, 
            CancellationToken cancellationToken)
        {
            var ctUsuEtt = await _unitOfWork.Query<CtUsuarioEtt>()
              .SingleOrDefaultAsync(i => i.Id == request.usuario.Id, cancellationToken)
              ?? throw new NotFoundException(nameof(CtUsuarioEtt), request.usuario.Id);

            ctUsuEtt.Contrasenia = request.usuario.ContraseniaFld.ToObfuscated();

            _unitOfWork.Update(ctUsuEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // No necesitas marcar como Modified, EF ya lo detecta
            return OperationResult<bool>.Success(
                true,
                "La contraseña se cambió correctamente, a continuación inicie sesión con su nueva contraseña."
            );

        }
    }
}
