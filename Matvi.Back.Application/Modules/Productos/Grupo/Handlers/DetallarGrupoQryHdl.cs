using Matvi.Back.Application.Modules.Productos.Grupo.Dtos;
using Matvi.Back.Application.Modules.Productos.Grupo.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Grupo.Handlers
{
    class DetallarGrupoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarGrupoQry, DetallarGrupoDto>
    {
                /// <inheritdoc/>
public async Task<DetallarGrupoDto> Handle(
            DetallarGrupoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            
            var query = db.Set<CtGrupoEtt>()
                .Where(cg => cg.Id == request.id)
                .Select(cg => new DetallarGrupoDto
                {
                    Id = cg.Id,
                    CtSecc_Nombre = cg.Nav_IdCtSeccion!.Nombre,
                    Nombre = cg.Nombre,
                    Descripcion = cg.Descripcion
                });

            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}
