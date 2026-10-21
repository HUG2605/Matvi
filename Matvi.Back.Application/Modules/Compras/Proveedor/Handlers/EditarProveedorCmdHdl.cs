using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Proveedor.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Handlers
{
    [Transaction]
    class EditarProveedorCmdHdl(
       IUnitOfWork _unitOfWork
       ) : IRequestHandler<EditarProveedorCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EditarProveedorCmd request, 
            CancellationToken cancellationToken)
        {
            var proveedorDto = request.proveedor;

            var proveedorEtt = await _unitOfWork.FindAsync<CtProveedorEtt, int>(proveedorDto.Id, cancellationToken);

            proveedorEtt!.Nombre = proveedorDto.NombreFld;
            proveedorEtt.Direccion = proveedorDto.DireccionFld;
            proveedorEtt.Telefono = proveedorDto.TelefonoFld;
            proveedorEtt.NombreContacto = proveedorDto.NombreContactoFld;
            proveedorEtt.TelefonoContacto = proveedorDto.TelefonoContactoFld;

            _unitOfWork.Update(proveedorEtt);

            return OperationResult<bool>.Success(
                true, 
                "El proveedor se editó correctamente.");
        }
    }
}
