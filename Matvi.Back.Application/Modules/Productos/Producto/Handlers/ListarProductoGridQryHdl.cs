using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using Matvi.Back.Application.Modules.Productos.Producto.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Producto.Handlers
{
    class ListarProductoGridQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarProductoGridQry, DataGrd<ListarProductoDto>>
    {
        public async Task<DataGrd<ListarProductoDto>> Handle(
            ListarProductoGridQry request, 
            CancellationToken cancellationToken)
        {
            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtProductoEtt>()
                .Select(cp => new ListarProductoDto
                {
                    Id = cp.Id,
                    Nombre = cp.Nombre,
                    EspecificacionTecnica = cp.EspecificacionTecnica,
                    Marca = cp.Marca,
                    CtSecc_Nombre = cp.Nav_IdCtGrupo!.Nav_IdCtSeccion!.Nombre,
                    CtGrup_Nombre = cp.Nav_IdCtGrupo!.Nombre,
                    Activo = cp.Activo
                });

            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(request.filter.Search))
            {
                var searchLower = request.filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.EspecificacionTecnica!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Marca!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtSecc_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.CtGrup_Nombre!, $"%{searchLower}%") ||
                    EF.Functions.Like(x.Activo!.ToString(), $"%{searchLower}%")
                );
            }

            #endregion

            #region Ordenamiento

            if (!string.IsNullOrWhiteSpace(request.filter.SortBy))
            {
                var sortBy = request.filter.SortBy.ToLower();
                var descending = request.filter.SortDesc;
                query = query.ApplyOrdering(sortBy.ToLowerInvariant(), descending);
            }

            #endregion

            #region Total antes de paginar

            var totalItems = await query.CountAsync(cancellationToken);

            #endregion

            #region Paginación

            var items = await query
                .Skip((request.filter.Page - 1) * request.filter.PageSize)
                .Take(request.filter.PageSize)
                .ToListAsync(cancellationToken);

            #endregion

            #region Resultado

            return new DataGrd<ListarProductoDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}
