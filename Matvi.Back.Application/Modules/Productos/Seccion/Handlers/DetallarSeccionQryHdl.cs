using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Handlers
{
    class DetallarSeccionQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarSeccionQry, DetallarSeccionDto>
    {
        public async Task<DetallarSeccionDto> Handle(
            DetallarSeccionQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtSeccionEtt>()
                .AsNoTracking()
                .Where(cs => cs.Id == request.id)
                .Select(cs => new DetallarSeccionDto
                {
                    Id = cs.Id,
                    Nombre = cs.Nombre,
                    LstGrupo = cs.Coll_CtGrupo_IdCtSeccion
                    .Select(cg => cg.Nombre)
                    .ToList()
                });

            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}
