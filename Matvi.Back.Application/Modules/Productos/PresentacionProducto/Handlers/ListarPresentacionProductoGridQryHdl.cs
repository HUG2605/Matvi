using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Handlers
{
    class ListarPresentacionProductoGridQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarPresentacionProductoGridQry, List<ListarProductoSucursalDto>>
    {
                /// <inheritdoc/>
public async Task<List<ListarProductoSucursalDto>> Handle(
            ListarPresentacionProductoGridQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtPresentacionProductoEtt>()
              .Where(pp => pp.IdCtProducto == request.idProducto)
              .Select(pp => new ListarProductoSucursalDto
              {
                  Id = pp.Id,
                  CtPresProd_Descripcion = $"{pp.Nombre} ({pp.FactorUBI.ToString("#,##0.###")} {pp.Nav_IdCtProducto!.Nav_IdCtUnidadBaseInventario!.Abreviatura.ToLower()})",
                  EsParaCompra = pp.EsParaCompra,
                  EsParaVenta = pp.EsParaVenta,
                  PrecioVenta = pp.PrecioVenta,
                  PrecioMayoreo = pp.PrecioMayoreo,
                  UnidadesMayoreo = pp.UnidadesMayoreo,
                  EsPresentacionBase = pp.Nombre == pp.Nav_IdCtProducto!.Nav_IdCtUnidadBaseInventario!.Nombre
              });

            var productoPresentaciones = await query.ToListAsync(cancellationToken);
            return productoPresentaciones;
        }
    }
}
