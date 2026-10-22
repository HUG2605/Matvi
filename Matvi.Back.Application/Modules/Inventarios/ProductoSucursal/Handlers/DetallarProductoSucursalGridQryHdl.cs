using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Handlers
{
    class DetallarProductoSucursalGridQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<DetallarProductoSucursalQry, DetallarProductoSucursalDto>
    {
                /// <inheritdoc/>
public async Task<DetallarProductoSucursalDto> Handle(
            DetallarProductoSucursalQry request, 
            CancellationToken cancellationToken)
        {
            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<RlProductoSucursalEtt>()
              .Where(ps =>
                    ps.IdCtProducto == request.idProducto
                 && ps.IdCtSucursal == request.idSucursal)
              .Select(ps => new DetallarProductoSucursalDto
              {
                  // Sucursal
                  CtSuc_Id = ps.IdCtSucursal,
                  CtSuc_Nombre = ps.Nav_IdCtSucursal.Nombre,
                  // Producto
                  CtProd_Id = ps.IdCtProducto,
                  CtProd_Nombre = ps.Nav_IdCtProducto.Nombre,
                  CtProd_EspecificacionTecnica = ps.Nav_IdCtProducto.EspecificacionTecnica,
                  CtProd_Marca = ps.Nav_IdCtProducto.Marca,
                  // Unidad Base de Inventario
                  CtUnidBaseInve_Abreviatura = ps.Nav_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura,
                  // Producto en sucursal
                  StockMinimoUBI = ps.StockMinimoUBI,
                  ExistenciasUBI = ps.Nav_IdCtProducto.Coll_OpLote_Cpp_IdCtProducto.Where(i => i.CantidadDisponibleUBI > 0).Sum(i => i.CantidadDisponibleUBI),
                  StockMaximoUBI = ps.StockMaximoUBI,
                  Regularizado = ps.Regularizado,
                  Ubicacion = ps.Ubicacion
              });

            var productoSucursal = await query.SingleAsync(cancellationToken);
            return productoSucursal;
        }
    }
}
