using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Compras.Proveedor.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Handlers
{
    [Transaction]
    class CrearProveedorCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CrearProveedorCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            CrearProveedorCmd request, 
            CancellationToken cancellationToken)
        {
            var proveedorDto = request.proveedor;

            var nvoProveedor = new CtProveedorEtt
            {
                Nombre = proveedorDto.NombreFld,
                Direccion = proveedorDto.DireccionFld,
                Telefono = proveedorDto.TelefonoFld,
                NombreContacto = proveedorDto.NombreContactoFld,
                TelefonoContacto = proveedorDto.TelefonoContactoFld
            };

            _unitOfWork.Add(nvoProveedor);

            return OperationResult<bool>.Success(
                true, 
                "El proveedor se creó correctamente.");
        }
    }
}
