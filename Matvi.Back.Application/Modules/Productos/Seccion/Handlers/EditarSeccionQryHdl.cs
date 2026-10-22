using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Handlers
{
    class EditarSeccionQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarSeccionQry, EditarSeccionDto>
    {
                /// <inheritdoc/>
public async Task<EditarSeccionDto> Handle(
            EditarSeccionQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtSeccionEtt>()
                .Where(cs => cs.Id == request.id)
                .Select(cs => new EditarSeccionDto {
                    Id = cs.Id,
                    NombreFld = cs.Nombre
                });

            var res = await query.SingleAsync(cancellationToken);
            return res;
        }
    }
}
