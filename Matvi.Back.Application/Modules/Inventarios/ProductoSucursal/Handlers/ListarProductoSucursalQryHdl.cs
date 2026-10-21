using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Dtos;
using Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Inventarios.ProductoSucursal.Handlers
{
    class ListarProductoSucursalQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarProductoSucursalQry, DataGrd<ListarProductoSucursalDto>>
    {
        public async Task<DataGrd<ListarProductoSucursalDto>> Handle(
            ListarProductoSucursalQry request, 
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<RlProductoSucursalEtt>()
                .Where(ps => ps.IdCtSucursal == request.idSucursal)
                .Select(ps => new ListarProductoSucursalDto()
                {
                    // Producto
                    CtProd_Id = ps.IdCtProducto,
                    CtProd_Nombre = ps.Nav_IdCtProducto.Nombre,
                    CtProd_EspecificacionTecnica = ps.Nav_IdCtProducto.EspecificacionTecnica,
                    CtProd_Marca = ps.Nav_IdCtProducto.Marca,
                    CtSecc_Nombre = ps.Nav_IdCtProducto.Nav_IdCtGrupo.Nav_IdCtSeccion.Nombre,
                    CtGrup_Nombre = ps.Nav_IdCtProducto.Nav_IdCtGrupo.Nombre,
                    // Unidad Base de Inventario
                    CtUnidBaseInve_Abreviatura = ps.Nav_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura,
                    // Producto en sucursal
                    StockMinimoUBI = ps.StockMinimoUBI,
                    ExistenciasUBI = ps.Nav_IdCtProducto.Coll_OpLote_Cpp_IdCtProducto.Where(i => i.CantidadDisponibleUBI > 0).Sum( i => i.CantidadDisponibleUBI),
                    StockMaximoUBI = ps.StockMaximoUBI,
                    Regularizado = ps.Regularizado
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.CtProd_Nombre.ToString()!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProd_EspecificacionTecnica!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProd_Marca!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtSecc_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtGrup_Nombre!, $"%{searchLower}%")  
                );
            }

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                var sort = filter.SortBy.ToLower() == "ctprod_nombrecompleto" 
                    ? "ctprod_nombre"
                    : filter.SortBy.ToLower();
                query = query.ApplyOrdering(sort, filter.SortDesc);
            }

            #endregion

            #region Total antes de paginar

            var totalItems = await query.CountAsync(cancellationToken);

            #endregion

            #region Paginación

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            #endregion

            #region Resultado

            return new DataGrd<ListarProductoSucursalDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
