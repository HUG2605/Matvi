using Matvi.Back.Application.Common.Enums;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Configuracion.Sucursal.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Configuracion.Sucursal.Handlers
{
    [Transaction]
    class CrearSucursalCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CrearSucursalCmd, OperationResult<bool>>
    {
                /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
            CrearSucursalCmd request,
            CancellationToken cancellationToken)
        {
            var nvoSucursalEtt = new CtSucursalEtt
            {
                IdCtEmpresa = 1, // Única
                Nombre = request.sucursal.NombreFld,
                Clave = request.sucursal.ClaveFld,
                Direccion = request.sucursal.DireccionFld,
                Telefono = request.sucursal.TelefonoFld
            };

            _unitOfWork.Add(nvoSucursalEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #region RELACIONAR USUARIOS SYSTEM Y ADMINISTRADORES CON LA NUEVA SUCURSAL

            byte[] roles = { (byte)RolEnm.SYSTEM, (byte)RolEnm.ADMINISTRADOR };

            var newLstRlUsuSucEtt = await _unitOfWork.Query<CtUsuarioEtt>()
                .Where(i => roles.Contains(i.IdCtRol))
                .Select(i => new RlUsuarioSucursalEtt
                {
                    IdCtUsuario = i.Id,
                    IdCtSucursal = nvoSucursalEtt.Id
                })
                .ToListAsync(cancellationToken);

            _unitOfWork.AttachParentsRange(newLstRlUsuSucEtt);
            _unitOfWork.AddRange(newLstRlUsuSucEtt);

            #endregion

            #region RELACIONAR TODOS LOS PRODUCTOS EXISTENTES CON LA NUEVA SUCURSAL

            var lstIdProductos = _unitOfWork.Query<CtProductoEtt>()
                .Select(p => p.Id)
                .ToList();

            var lstRelProductoSucursalEtt = lstIdProductos
                .Select(idProducto => new RlProductoSucursalEtt
                {
                    IdCtProducto = idProducto,
                    IdCtSucursal = nvoSucursalEtt.Id,
                    Regularizado = false,
                    Ubicacion = null
                }).ToList();

            if (lstRelProductoSucursalEtt is { Count: > 0 })
            {
                _unitOfWork.AddRange(lstRelProductoSucursalEtt);
            }

            #endregion

            return OperationResult<bool>.Success(
                true,
                "La sucursal se creó correctamente.");
        }
    }
}