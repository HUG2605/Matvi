using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Productos.Grupo.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Handlers
{
    [Transaction]
    class EditarGrupoCmdHdl(
        IUnitOfWork _unitOfWork
       ) : IRequestHandler<EditarGrupoCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EditarGrupoCmd request, 
            CancellationToken cancellationToken)
        {
            var grupoEtt = await _unitOfWork.FindAsync<CtGrupoEtt, short>(request.grupo.Id, cancellationToken);

            grupoEtt!.IdCtSeccion = request.grupo.CtSecc_IdFld;
            grupoEtt.Nombre = request.grupo.NombreFld;
            grupoEtt.Descripcion = request.grupo.DescripcionFld;
           
            _unitOfWork.Update(grupoEtt);

            return OperationResult<bool>.Success(
                true, 
                "El grupo se editó correctamente.");
        }
    }
}
