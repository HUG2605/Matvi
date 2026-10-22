using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Inventarios.Lote.Dtos;
using Matvi.Back.Application.Modules.Inventarios.Lote.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Inventarios.Lote.Handlers
{
    class ListarLoteQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarLoteQry, DataGrd<ListarLoteDto>>
    {
                /// <inheritdoc/>
public async Task<DataGrd<ListarLoteDto>> Handle(
            ListarLoteQry request,
            CancellationToken cancellationToken)
        {
            var filter = request.filter;

            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);
            var query = db.Set<OpLoteEtt>()
                .Where(e => e.IdCtSucursal == request.idSucursal)
                .Select(e => new ListarLoteDto()
                {
                    // IIdDto
                    Id = e.Id,
                    // IFolioDto
                    CtSucu_Clave = e.Nav_IdCtSucursal.Clave,
                    Numero = e.Numero,
                    // IProductoBaseDto
                    CtProd_Nombre = e.Nav_Cpp_IdCtProducto.Nombre,
                    CtProd_EspecificacionTecnica = e.Nav_Cpp_IdCtProducto.EspecificacionTecnica,
                    CtProd_Marca = e.Nav_Cpp_IdCtProducto.Marca,
                    // IPresentacionProductoBaseDto
                    CtPresProd_IdCtProducto = e.Cpp_IdCtProducto,
                    CtPresProd_Nombre = e.Cpp_Nombre,
                    CtPresProd_FactorUBI = e.Cpp_FactorUBI,
                    // Otros
                    CtTipoLote_Nombre = e.Nav_IdCtTipoLote.Nombre,
                    CantidadInicialUBI = e.CantidadInicialUBI,
                    CantidadDisponibleUBI = e.CantidadDisponibleUBI,
                    CtUnidBaseInve_Abreviatura = e.Nav_Cpp_IdCtProducto.Nav_IdCtUnidadBaseInventario.Abreviatura.ToLower(),
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var searchLower = filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Numero.ToString()!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtTipoLote_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProd_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProd_EspecificacionTecnica!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtProd_Marca!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtPresProd_Nombre!, $"%{searchLower}%")
                );
            }

            #endregion

            #region Total antes de paginar (y antes de ordenar)

            var totalItems = await query.CountAsync(cancellationToken);

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
             
                var sort = filter.SortBy.ToLower() switch
                {
                    "folio" => "Numero",
                    "ctprod_nombrecompleto" => "CtProd_Nombre",
                    _ => filter.SortBy.ToLower()
                };
                query = query.ApplyOrdering(sort, filter.SortDesc);
            }

            #endregion

            #region Paginación

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync(cancellationToken);

            #endregion

            #region Resultado

            return new DataGrd<ListarLoteDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}