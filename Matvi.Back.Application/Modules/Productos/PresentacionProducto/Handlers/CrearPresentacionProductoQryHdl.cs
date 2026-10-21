using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Dtos;
using Matvi.Back.Application.Modules.Productos.PresentacionProducto.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.PresentacionProducto.Handlers
{
    class CrearPresentacionProductoQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<CrearPresentacionProductoQry, CrearPresentacionProductoDto>
    {
        public async Task<CrearPresentacionProductoDto> Handle(
            CrearPresentacionProductoQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtProductoEtt>()
                .Where(cp => cp.Id == request.productoId)
                .Select(cp => new CrearPresentacionProductoDto
                {
                    CtProd_IdFld = cp.Id,
                    CtProd_Nombre = cp.Nombre,
                    CtProd_EspecificacionTecnica = cp.EspecificacionTecnica,
                    CtProd_Marca = cp.Marca,
                    NombreFld = string.Empty,
                    FactorUBIFld = 0,
                    CtUnidBaseInve_Abreviatura = cp.Nav_IdCtUnidadBaseInventario.Abreviatura.ToLower(),
                    PrecioVentaFld = 0,
                });

            var presentacionProducto = await query.SingleAsync(cancellationToken);
            return presentacionProducto;
        }
    }
}
