using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using Matvi.Back.Application.Modules.Productos.Producto.Helpers;
using Matvi.Back.Application.Modules.Productos.Producto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Producto.Handlers
{
    class DetallarProductoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarProductoQry, DetallarProductoDto>
    {
        public async Task<DetallarProductoDto> Handle(
            DetallarProductoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtProductoEtt>()
                .Where(cp => cp.Id == request.id)
                .Select(cp => new DetallarProductoDto {
                    // IIdDto
                    Id = cp.Id,
                    // IProductoBaseDto
                    CtProd_Nombre = cp.Nombre,
                    CtProd_EspecificacionTecnica = cp.EspecificacionTecnica,
                    CtProd_Marca = cp.Marca,
                    // Otros
                    CtSecc_Nombre = cp.Nav_IdCtGrupo!.Nav_IdCtSeccion!.Nombre,
                    CtGrup_Nombre = cp.Nav_IdCtGrupo!.Nombre,
                    CtGrup_Descripcion = cp.Nav_IdCtGrupo!.Descripcion,
                    CtUnidBaseInve_Nombre = $"{cp.Nav_IdCtUnidadBaseInventario!.Nombre} ({cp.Nav_IdCtUnidadBaseInventario.Abreviatura.ToLower()})" ,
                    BajoPedido = cp.BajoPedido,
                    EntregaMostrador = cp.EntregaMostrador,
                    Imagen = cp.Imagen,
                    CodigoBarrasInterno = cp.CodigoBarrasInterno,
                    CodigoBarras = cp.CodigoBarras,
                    Activo = cp.Activo
                });
                     
            var producto = await query.SingleAsync(cancellationToken);
            producto.Proveedores = await ProductoQueriesHelper
                .GetProveedoresRelacionados(db, request.id)
                .ToListAsync(cancellationToken);

            return producto;
        }
    }
}
