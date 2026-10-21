using Matvi.Back.Application.Common.Dtos;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Handlers
{
    class ListarPresentacionProductoSelectQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarPresentacionProductoSelectQry, List<BaseDto<int>>>
    {
        public async Task<List<BaseDto<int>>> Handle(
            ListarPresentacionProductoSelectQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtPresentacionProductoEtt>()
            .Where(pp => 
                pp.IdCtProducto == request.idProducto
                && ((request.paraCompra && pp.EsParaCompra) ||
                    (request.paraVenta && pp.EsParaVenta))
                )
            .Select(pp => new BaseDto<int>
            {
                Id = pp.Id,
                Nombre = $"{pp.Nombre} {pp.FactorUBI.ToString("0.###")} {pp.Nav_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura.ToLower()}",
            });

            var productoPresentaciones = await query.ToListAsync(cancellationToken);
            return productoPresentaciones;
        }
    }
}
