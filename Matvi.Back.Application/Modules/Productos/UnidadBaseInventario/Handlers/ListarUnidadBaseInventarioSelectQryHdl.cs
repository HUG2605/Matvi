using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Productos.UnidadBaseInventario.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.UnidadBaseInventario.Handlers
{
    class ListarUnidadBaseInventarioSelectQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarUnidadBaseInventarioSelectQry, List<BaseDto<byte>>>
    {
                /// <inheritdoc/>
public async Task<List<BaseDto<byte>>> Handle(
            ListarUnidadBaseInventarioSelectQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtUnidadBaseInventarioEtt>()
             .Select(cubi => new BaseDto<byte>
             {
                 Id = cubi.Id,
                 Nombre = $"{cubi.Nombre} ({cubi.Abreviatura.ToLower()})"
             });

            var res = await query.ToListAsync(cancellationToken);
            return res;
        }
    }
}
