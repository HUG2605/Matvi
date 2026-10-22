using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Handlers
{
    [Transaction]
    class EditarSucursalCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EditarSucursalCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            EditarSucursalCmd request, 
            CancellationToken cancellationToken)
        {
            var sucursalEtt = await _unitOfWork.FindAsync<CtSucursalEtt, byte>(request.sucursal.Id, cancellationToken);

            sucursalEtt!.Nombre = request.sucursal.NombreFld;
            sucursalEtt.Clave = request.sucursal.ClaveFld;
            sucursalEtt.Direccion = request.sucursal.DireccionFld;
            sucursalEtt.Telefono = request.sucursal.TelefonoFld;

            _unitOfWork.Update(sucursalEtt);

            return OperationResult<bool>.Success(
                true, 
                "La sucursal se editó correctamente.");
        }
    }
}
