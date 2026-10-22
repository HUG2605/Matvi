using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Productos.Producto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Producto.Handlers
{
    class ListarProductoSelectCoincidenciaQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarProductoSelectCoincidenciaQry, List<BaseDto<int>>>
    {
                /// <inheritdoc/>
public async Task<List<BaseDto<int>>> Handle(
            ListarProductoSelectCoincidenciaQry request,
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtProductoEtt>()
                .Where(cp => EF.Functions.Like(cp.Nombre, $"%{request.coincidencia}%"))
                .OrderBy(cp => cp.Nombre)
                .Select(cp => new BaseDto<int>
                {
                    Id = cp.Id,
                    Nombre = cp.Nombre 
                    + (string.IsNullOrWhiteSpace(cp.EspecificacionTecnica) ? "" : $" ({cp.EspecificacionTecnica})")
                    + (string.IsNullOrWhiteSpace(cp.Marca) ? "" : $" [{cp.Marca}]"),
                });

            var result = await query.Take(15).ToListAsync();
            return result;
        }
    }
}
