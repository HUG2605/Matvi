using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Seccion.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Handlers
{
    [Transaction]
    class EditarSeccionCmdHdl(
       IUnitOfWork _unitOfWork
       ) : IRequestHandler<EditarSeccionCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EditarSeccionCmd request, 
            CancellationToken cancellationToken)
        {
            var seccionEtt = await _unitOfWork.FindAsync<CtSeccionEtt, byte>(request.seccion.Id, cancellationToken);
            seccionEtt!.Nombre = request.seccion.NombreFld;

            _unitOfWork.Update(seccionEtt);

            return OperationResult<bool>.Success(
                true, 
                "La seccion se editó correctamente.");
        }
    }
}
