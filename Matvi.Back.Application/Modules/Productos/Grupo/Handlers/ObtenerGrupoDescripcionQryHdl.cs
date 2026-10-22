using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Handlers
{
    class ObtenerGrupoDescripcionQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ObtenerGrupoDescripcionQry, string>
    {
                /// <inheritdoc/>
public async Task<string> Handle(
            ObtenerGrupoDescripcionQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtGrupoEtt>()
                .Where(i => i.Id == request.id)
                .Select(i => i.Descripcion);

            var descripcion = await query.FirstAsync();
            return descripcion;
        }
    }
}
