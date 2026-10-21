using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Rol.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Handlers
{
    class EditarRolQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarRolQry, EditarRolDto>
    {
        public async Task<EditarRolDto> Handle(
            EditarRolQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            // 1. Obtener datos del rol
            var rol = await db.Set<CtRolEtt>()
                .Where(cu => cu.Id == request.id)
                .Select(cu => new EditarRolDto
                {
                    Id = cu.Id,
                    NombreFld = cu.Nombre,
                })
                .SingleAsync(cancellationToken);

            // 2. Obtener funciones del rol
            var funciones = await db.Set<RlRolFuncionEtt>()
                .Where(rrf => rrf.IdCtRol == request.id)
                .Select(rrf => new RolFuncionDto
                {
                    Id = rrf.Id,
                    FuncionId = rrf.Nav_IdCtFuncion!.Id,
                    FuncionNombre = rrf.Nav_IdCtFuncion!.Nombre,
                    ModuloId = rrf.Nav_IdCtFuncion.Nav_IdCtModulo!.Id,
                    ModuloNombre = rrf.Nav_IdCtFuncion.Nav_IdCtModulo!.Nombre,
                    Consultar = rrf.Consultar,
                    VerSensibles = rrf.VerSensibles
                })
                .ToListAsync(cancellationToken);

            // 3. Obtener acciones del rol
            var acciones = await db.Set<RlRolAccionEtt>()
                .Where(rra => rra.IdCtRol == request.id)
                .Select(rra => new RolAccionAccesoPermisoDto
                {
                    Id = rra.Id,
                    FuncionId = rra.Nav_IdCtAccion!.IdCtFuncion,
                    AccionNombre = rra.Nav_IdCtAccion!.Nombre,
                    TieneAcceso = rra.TieneAcceso
                })
                .ToListAsync(cancellationToken);

            // 4. Agrupar acciones por función (optimización clave)
            var accionesPorFuncion = acciones
                .GroupBy(a => a.FuncionId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // 5. Asignar acciones a cada función
            foreach (var funcion in funciones)
            {
                if (accionesPorFuncion.TryGetValue(funcion.FuncionId, out var lista))
                    funcion.Acciones = lista;
                else
                    funcion.Acciones = new List<RolAccionAccesoPermisoDto>();
            }

            rol.Funciones = funciones;
            return rol;
        }
    }
}
