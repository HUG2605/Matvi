using Matvi.Back.Application.Common.Grid;
using Matvi.Back.Application.Modules.Productos.Seccion.Dtos;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Back.Infrastructure.Common.Extensions;
using Matvi.Back.Infrastructure.Persistence.Contexts;
using Matvi.Back.Infrastructure.Persistence.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Matvi.Back.Application.Modules.Productos.Seccion.Handlers
{
    class ListarSeccionGridQryHdl(
        IDbContextFactory<MatviQryDbCtx> _factory
        ) : IRequestHandler<ListarSeccionGridQry, DataGrd<ListarSeccionDto>>
    {
        public async Task<DataGrd<ListarSeccionDto>> Handle(
            ListarSeccionGridQry request, 
            CancellationToken cancellationToken)
        {
            #region Query base

            using var db = await _factory.CreateDbContextAsync(cancellationToken);

            var query = db.Set<CtSeccionEtt>()
                .Select(cs => new ListarSeccionDto {
                    Id = cs.Id,
                    Nombre = cs.Nombre
                });
            
            #endregion

            #region Filtro por texto

            if (!string.IsNullOrWhiteSpace(request.filter.Search))
            {
                var searchLower = request.filter.Search.ToLower();
                query = query.Where(x =>
                    EF.Functions.Like(x.Nombre!, $"%{searchLower}%")
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

            return new DataGrd<ListarSeccionDto>
            {
                TotalItems = totalItems,
                Items = items
            };

            #endregion
        }
    }
}