using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Compras.Proveedor.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Compras.Proveedor.Handlers
{
    class ListarProveedorSelectCoincidenciaQryHdl(
        IDbContextFactory<MatviQryDbCtx> factory
    ) : IRequestHandler<ListarProveedorSelectCoincidenciaQry, List<BaseDto<int>>>
    {
                /// <inheritdoc/>
public async Task<List<BaseDto<int>>> Handle(
            ListarProveedorSelectCoincidenciaQry request,
            CancellationToken cancellationToken)
        {
            using var db = await factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtProveedorEtt>()
                .Where(p => EF.Functions.Like(p.Nombre, $"%{request.coincidencia}%"))
                .OrderBy(p => p.Nombre)
                .Select(p => new BaseDto<int>
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                });

            var result = await query.Take(15).ToListAsync();
            return result;
        }
    }
}