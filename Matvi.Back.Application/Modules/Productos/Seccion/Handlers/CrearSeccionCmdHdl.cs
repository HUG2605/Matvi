using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Seccion.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Handlers
{
    [Transaction]
    class CrearSeccionCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CrearSeccionCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            CrearSeccionCmd request, 
            CancellationToken cancellationToken)
        {
            var nvoSeccionEtt = new CtSeccionEtt
            {
                Nombre = request.seccion.NombreFld
            };

            _unitOfWork.Add(nvoSeccionEtt);
            
            return OperationResult<bool>.Success(
                true, 
                "La seccion se creó correctamente.");
        }
    }
}
