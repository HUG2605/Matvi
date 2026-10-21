using Matvi.Back.Application.Modules.Seguridad.Login.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Login.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Handlers
{
    class ObtenerUsuarioSesionLoginQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ObtenerUsuarioSesionLoginQry, UsuarioLoginDto?>
    {
        public async Task<UsuarioLoginDto?> Handle(ObtenerUsuarioSesionLoginQry request, CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtUsuarioEtt>()
                .Where(cu => 
                    cu.Usuario == request.Usuario &&
                    cu.Contrasenia == request.Contrasenia &&
                    cu.Activo)
                .Select(cu => new UsuarioLoginDto
                {
                    Id = cu.Id,
                    Nombre = cu.Nombre,
                    PrimerApellido = cu.PrimerApellido,
                    SegundoApellido = cu.SegundoApellido,
                    RolId = cu.Nav_IdCtRol!.Id,
                    RolNombre = cu.Nav_IdCtRol.Nombre,
                    Funciones = cu.Nav_IdCtRol.Coll_RlRolFuncion_IdCtRol!
                                .Where(i => i.Consultar)
                                .Select(i => new FuncionLoginDto
                                {
                                    Id = i.Nav_IdCtFuncion!.Id,
                                    Nombre = i.Nav_IdCtFuncion.Nombre,
                                    Ruta = i.Nav_IdCtFuncion.Ruta,
                                    CtModu_Id = i.Nav_IdCtFuncion.Nav_IdCtModulo!.Id,
                                    CtModu_Nombre = i.Nav_IdCtFuncion.Nav_IdCtModulo!.Nombre,
                                    CtModu_Area = i.Nav_IdCtFuncion.Nav_IdCtModulo.Area,
                                    CtModu_IconName = i.Nav_IdCtFuncion.Nav_IdCtModulo.IconName,
                                    VerSensibles = i.VerSensibles
                                }).ToList(),
                    Sucursales = cu.Coll_RlUsuarioSucursal_IdCtUsuario!
                                .Select(us => new SucursalLoginDto
                                {
                                    Id = us.IdCtSucursal,
                                    Nombre = us.Nav_IdCtSucursal!.Nombre,
                                    Clave = us.Nav_IdCtSucursal.Clave,
                                    Telefono = us.Nav_IdCtSucursal.Telefono
                                }).ToList()
                  });

            var res = await query.FirstOrDefaultAsync(cancellationToken);
            return res;

        }
    }
}
