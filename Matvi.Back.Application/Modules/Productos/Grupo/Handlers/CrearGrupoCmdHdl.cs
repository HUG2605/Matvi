using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Grupo.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Handlers
{
    [Transaction]
    class CrearGrupoCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CrearGrupoCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            CrearGrupoCmd request, 
            CancellationToken cancellationToken)
        {
            var nvoGrupoEtt = new CtGrupoEtt
            {
                IdCtSeccion = request.grupo.CtSecc_IdFld,
                Nombre = request.grupo.NombreFld,
                Descripcion = request.grupo.DescripcionFld,
            };

            _unitOfWork.Add(nvoGrupoEtt);

            return OperationResult<bool>.Success(
                true, 
                "El grupo se creó correctamente.");
        }
    }
}
