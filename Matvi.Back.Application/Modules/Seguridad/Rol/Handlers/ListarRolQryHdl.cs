using Matvi.Back.Application.Modules.Seguridad.Rol.Dtos;
using Matvi.Back.Application.Modules.Seguridad.Rol.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Seguridad.Rol.Handlers
{
    class ListarRolQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarRolQry, List<ListarRolDto>>
    {
                /// <inheritdoc/>
public async Task<List<ListarRolDto>> Handle(
            ListarRolQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtRolEtt>()
                .Where(r => r.Id != 0)
                .Select(r => new ListarRolDto
                {
                    Id = r.Id,
                    Nombre = r.Nombre
                });

            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }
}
