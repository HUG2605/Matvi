using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using Matvi.Back.Application.Modules.Productos.Producto.Helpers;
using Matvi.Back.Application.Modules.Productos.Producto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Producto.Handlers
{
    class EditarProductoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarProductoQry, EditarProductoDto>
    {
        public async Task<EditarProductoDto> Handle(
            EditarProductoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtProductoEtt>()
                .Where(cp => cp.Id == request.id)
                .Select(cp => new EditarProductoDto
                {
                    Id = cp.Id,
                    NombreFld = cp.Nombre,
                    EspecificacionTecnicaFld = cp.EspecificacionTecnica,
                    MarcaFld = cp.Marca,
                    CtSecc_IdFld = cp.Nav_IdCtGrupo!.IdCtSeccion,
                    CtGrup_IdFld = cp.IdCtGrupo,
                    CtUnidBaseInve_IdFld = cp.IdCtUnidadBaseInventario,
                    CtUnidBaseInve_Nombre = $"{cp.Nav_IdCtUnidadBaseInventario!.Nombre} ({cp.Nav_IdCtUnidadBaseInventario!.Abreviatura.ToLower()})",
                    BajoPedidoFld = cp.BajoPedido,
                    EntregaMostradorFld = cp.EntregaMostrador,
                    ImagenFld = cp.Imagen,
                    CodigoBarrasInternoFld = cp.CodigoBarrasInterno,
                    CodigoBarrasFld = cp.CodigoBarras,
                    ActivoFld = cp.Activo,
                });

            var producto = await query.SingleAsync(cancellationToken);
            producto.Proveedores = await ProductoQueriesHelper
                    .GetProveedoresRelacionados(db, request.id)
                    .ToListAsync(cancellationToken);

            return producto;
        }
    }
}
