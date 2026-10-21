using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Handlers
{
    class EditarProductoSucursalQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<EditarProductoSucursalQry, EditarProductoSucursalDto>
    {
        public async Task<EditarProductoSucursalDto> Handle(
            EditarProductoSucursalQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<RlProductoSucursalEtt>()
              .Where(ps =>
                    ps.IdCtProducto == request.idProducto
                 && ps.IdCtSucursal == request.idSucursal)
              .Select(ps => new EditarProductoSucursalDto
              {
                  CtProd_Id = ps.IdCtProducto,
                  CtProd_Nombre = ps.Nav_IdCtProducto.Nombre,
                  CtProd_EspecificacionTecnica = ps.Nav_IdCtProducto.EspecificacionTecnica,
                  CtProd_Marca = ps.Nav_IdCtProducto.Marca,
                  CtSuc_Id = ps.IdCtSucursal,
                  CtSuc_Nombre = ps.Nav_IdCtSucursal.Nombre,
                  StockMinimoUBIFld = ps.StockMinimoUBI,
                  StockMaximoUBIFld = ps.StockMaximoUBI,
                  CtUnidBaseInve_Abreviatura = ps.Nav_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura,
                  Regularizado = ps.Regularizado,
                  UbicacionFld = ps.Ubicacion
              });

            var productoSucursal = await query.SingleAsync(cancellationToken);
            return productoSucursal;
        }
    }
}
