using Matvi.Back.Application.Common.Extensions;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Handlers
{
    [Transaction]
    class EditarUsuarioCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<EditarUsuarioCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            EditarUsuarioCmd request, 
            CancellationToken cancellationToken)
        {
            var usuarioDto = request.usuario;

            #region ACTUALIZAR USUARIO

            var usuarioEtt = await _unitOfWork.FindAsync<CtUsuarioEtt, short>(usuarioDto.Id, cancellationToken);
             
            usuarioEtt!.IdCtRol = usuarioDto.RolIdFld;
            usuarioEtt.Nombre = usuarioDto.NombreFld;
            usuarioEtt.PrimerApellido = usuarioDto.PrimerApellidoFld;
            usuarioEtt.SegundoApellido = usuarioDto.SegundoApellidoFld;
            usuarioEtt.Direccion = usuarioDto.DireccionFld;
            usuarioEtt.Telefono = usuarioDto.TelefonoFld;
            usuarioEtt.Contrasenia = usuarioDto.ReiniciarContraseniaFld
                ? usuarioEtt.Usuario.ToObfuscated()
                : usuarioEtt.Contrasenia;
            usuarioEtt.Activo = usuarioDto.ActivoFld;

            _unitOfWork.Update(usuarioEtt);

            #endregion

            #region ELIMINAR RELACIONES ANTERIORES DE SUCURSALES

            var relaciones = await _unitOfWork.Query<RlUsuarioSucursalEtt>()
             .Where(i => i.IdCtUsuario == usuarioDto.Id)
             .ToListAsync(cancellationToken);

            _unitOfWork.RemoveRange(relaciones);

            #endregion

            #region AGREGAR RELACIONES NUEVAS DE SUCURSALES

            var lstRelUsuSucEtt = usuarioDto.Sucursales?
                .Where(s => s.EstaRelacionada)
                .Select(s => new RlUsuarioSucursalEtt
                {
                    IdCtUsuario = usuarioDto.Id,
                    IdCtSucursal = s.Id
                }).ToList();

            if (lstRelUsuSucEtt is { Count: > 0 })
            {
                _unitOfWork.AddRange(lstRelUsuSucEtt);
            }

            #endregion

            return OperationResult<bool>.Success(
                true, 
                "El usuario se editó correctamente.");
        }
    }
}
