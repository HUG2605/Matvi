using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Handlers
{
    class EditarPresentacionProductoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarPresentacionProductoQry, EditarPresentacionProductoDto>
    {
                /// <inheritdoc/>
public async Task<EditarPresentacionProductoDto> Handle(
            EditarPresentacionProductoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtPresentacionProductoEtt>()
                .Where(cpp => cpp.Id == request.id)
                .Select(cpp => new EditarPresentacionProductoDto
                {
                    Id = cpp.Id,
                    CtProd_IdFld = cpp.IdCtProducto,
                    CtProd_Nombre = cpp.Nav_IdCtProducto.Nombre,
                    CtProd_EspecificacionTecnica = cpp.Nav_IdCtProducto.EspecificacionTecnica,
                    CtProd_Marca = cpp.Nav_IdCtProducto.Marca,
                    NombreFld = cpp.Nombre,
                    FactorUBIFld = cpp.FactorUBI,
                    CtUnidBaseInve_Abreviatura = cpp.Nav_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura,
                    EsParaCompraFld = cpp.EsParaCompra,
                    EsParaVentaFld = cpp.EsParaVenta,
                    PrecioVentaFld = cpp.PrecioVenta,
                    PrecioMayoreoFld = cpp.PrecioMayoreo,
                    UnidadesMayoreoFld = cpp.UnidadesMayoreo,
                    EsPresentacionBase = cpp.Nombre == cpp.Nav_IdCtProducto.Nav_IdCtUnidadBaseInventario.Nombre
                });

            var presentacionProducto = await query.SingleAsync(cancellationToken);
            return presentacionProducto;
        }
    }
}
