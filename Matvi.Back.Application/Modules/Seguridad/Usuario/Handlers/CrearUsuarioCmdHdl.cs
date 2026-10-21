using Matvi.Back.Application.Common.Extensions;
using Matvi.Back.Application.Common.Results;
using Matvi.Back.Application.Modules.Seguridad.Usuario.Commands;
using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Entities;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;

namespace Matvi.Back.Application.Modules.Seguridad.Usuario.Handlers
{
    [Transaction]
    class CrearUsuarioCmdHdl(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<CrearUsuarioCmd, OperationResult<bool>>
    {
        public async Task<OperationResult<bool>> Handle(
            CrearUsuarioCmd request, 
            CancellationToken cancellationToken)
        {
            #region AGREGAR NUEVO USUARIO

            var nvoUsuarioEtt = new CtUsuarioEtt
            {
                IdCtRol = request.usuario.RolIdFld,
                Nombre = request.usuario.NombreFld,
                PrimerApellido = request.usuario.PrimerApellidoFld,
                SegundoApellido = request.usuario.SegundoApellidoFld,
                Direccion = request.usuario.DireccionFld,
                Telefono = request.usuario.TelefonoFld,
                Usuario = PrvGenerarUsuario(request.usuario.RolIdFld,
                request.usuario.NombreFld,
                request.usuario.PrimerApellidoFld,
                request.usuario.SegundoApellidoFld),
                Activo = true,
                Contrasenia = request.usuario.NombreFld.ToObfuscated() // o tu lógica
            };

            _unitOfWork.Add(nvoUsuarioEtt);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            #endregion

            #region AGREGAR RELACIÓN CON SUCURSALES

            var nuevoId = nvoUsuarioEtt.Id;

            var lstRelUsuSucEtt = request.usuario.Sucursales?
                .Where(s => s.EstaRelacionada)
                .Select(s => new RlUsuarioSucursalEtt
                {
                    IdCtUsuario = nuevoId,
                    IdCtSucursal = s.Id
                }).ToList();

            if (lstRelUsuSucEtt is { Count: > 0 })
            {
                _unitOfWork.AttachParentsRange(lstRelUsuSucEtt);
                _unitOfWork.AddRange(lstRelUsuSucEtt);
            }

            #endregion

            return OperationResult<bool>.Success(
                true, 
                "El usuario se creó correctamante.");
        }

        string PrvGenerarUsuario(
            byte idCtRol,
            string nombre,
            string primerApellido,
            string segundoApellido)
        {
            var iniciales = $"{nombre[..2]}{primerApellido[..1]}{segundoApellido[..1]}".ToUpperInvariant();
            var aleatorio = Random.Shared.Next(1000, 9999); // Sufijo numérico para unicidad
            return $"{iniciales}{aleatorio}";
        }
    }
}
