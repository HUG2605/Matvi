using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Configuracion.Empresa.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Empresa.Handlers
{
    [Transaction]
    class EditarEmpresaCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EditarEmpresaCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EditarEmpresaCmd request,
            CancellationToken cancellationToken)
        {
            var empresaEtt = await _unitOfWork.FindAsync<CtEmpresaEtt, byte>(request.empresa.Id, cancellationToken);
            empresaEtt!.Nombre = request.empresa.NombreFld;
            empresaEtt.Logotipo = request.empresa.LogotipoFld;
            empresaEtt.Telefono = request.empresa.TelefonoFld;

            _unitOfWork.Update(empresaEtt);

            return OperationResult<bool>.Success(
                true, 
                "La empresa se editó correctamente.");
        }
    }
}
