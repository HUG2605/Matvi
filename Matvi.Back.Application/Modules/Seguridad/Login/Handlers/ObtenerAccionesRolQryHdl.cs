using Matvi.Back.Application.Modules.Seguridad.Login.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Login.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Login.Handlers
{
    class ObtenerAccionesRolQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ObtenerAccionesRolQry, List<AccionLoginDto>>
    {
        public async Task<List<AccionLoginDto>> Handle(
            ObtenerAccionesRolQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<RlRolAccionEtt>()
                .Where(rra =>
                    rra.IdCtRol == request.idRol &&
                    rra.TieneAcceso == true)
                .Select(rra => new AccionLoginDto
                {
                    Accion = rra.Nav_IdCtAccion!.Nombre,
                    Funcion = rra.Nav_IdCtAccion.Nav_IdCtFuncion!.Nombre,
                });

            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }
}
